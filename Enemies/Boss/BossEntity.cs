using UnityEngine;

public abstract class BossEntity : InvulnerableAble {

    protected Vector2 initialPosition;
    protected bool active = false;
    protected int hits = GameplayValues.GetBossHits();
    protected int phase = 1;
    protected int phases = 1;
    protected int phasesDone = 0;
    protected bool isMovingLeft = true;
    protected float invulnerableTime = 1f;
    private GameObject player;
    protected bool nextAct = true;

    public virtual void Activate() {
        GameState.reachedBoss = true;
        active = true;
        initialPosition = transform.position;
        if (GameState.difficulty == Difficulty.VERY_EASY) {
            invulnerableTime = 0.75f;
        }
    }

    private void Start() {
        player = ObjectLocator.GetPlayer();
    }

    protected void Update() {
        if (invulnerable) {
            // HMMMMMM
            StartCoroutine("Flash");
        }

        if (GameState.debugActive && Input.GetKeyDown(KeyCode.K)) {
            ReceiveHit(8);
        }
    }

    public virtual void OnDeath() {
        // TODO: Refactor.
    }

    public void OnTriggerEnter2D(Collider2D collision) {
        if (!GameState.isGameLocked && active) {
            if (!invulnerable && collision.CompareTag(Tags.ENEMY_HITTER)) {
                if (collision.GetComponent<CrossThrow>() != null) {
                    collision.GetComponent<CrossThrow>().Break();
                }
                ReceiveHit();
            }

            // TODO: Hit management for the boss and enemies can be done through an event.
            // Add an event subscription to hitters on here and call it through the super class.
            // Me cago en mi puta vida pero cómo hice este pedazo de basura
            if (collision.gameObject.name == Constants.FEET_COLLIDER) {
                if (!invulnerable) {
                    ReceiveHit();
                }
                if (player == null) {
                    player = ObjectLocator.GetPlayer();
                }
                if (player != null) {
                    var playerMovement = player.GetComponent<PlatformerMovement2D>();
                    if (playerMovement != null) {
                        playerMovement.FeetCollided();
                        playerMovement.Jump(true);
                        playerMovement.GetComponent<Invulnerability>().SetInvulnerableNoBlink(0.1f);
                    }
                }
            }
        }
    }

    protected void ReceiveHit(int number) {
        hits -= number;
        if (hits > 0) {
            AudioManager.GetInstance().PlayEffect(Sfx.BOSS_HIT);
            StartCoroutine("SetInvulnerable", invulnerableTime);
        } else {
            phasesDone++;
            if (phases == phasesDone) {
                Die();
            } else {
                // Me cago en Dios, Juanma. Ni Senior ni pollas, eres un MIERDAS.
                // Extremely hacky...
                hits = GameplayValues.GetBossHits();
                phase++;
            }
        }
    }

    protected void ReceiveHit() {
        ReceiveHit(1);
    }

    protected void Die() {
        AudioManager.GetInstance().PlayEffect(Sfx.ENEMY_HIT);
        // TODO: Refactor. Final boss depende de esto.
        OnDeath();
        GameState.bossActive = false;
        Instantiate(Resources.Load<GameObject>(Visuals.BLOOD), transform.position, transform.rotation);
        var player = GameObject.FindWithTag(Tags.PLAYER);
        GameState.score += 1000; // Change
        if (player != null) {
            var pos = new Vector2(player.transform.position.x, player.transform.position.y + 0.3f);
            Instantiate(Resources.Load<GameObject>(Visuals.SCORE_1000), pos, player.transform.rotation);
        }
        Destroy(gameObject);

        // This code is repeated in Altar. Refactor.
        // Super hacky
        if (nextAct) {
            var blackBg = GameObject.Find(Constants.BLACK_BG);
            if (blackBg != null) {
                blackBg.AddComponent<EndAct>();
                blackBg.GetComponent<EndAct>().act = 2;
            }
        }
    }
}

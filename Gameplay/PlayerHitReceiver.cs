using UnityEngine;

public class PlayerHitReceiver : MonoBehaviour {

    public int hits = 1;
    public DeathType deathType = DeathType.BLOOD;
    private bool isBeingHit = false;
    private EnemyInvulnerability invuln;

    private void Start() {
        invuln = GetComponent<EnemyInvulnerability>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (gameObject.GetComponent<IEnemyEntity>() == null || gameObject.GetComponent<IEnemyEntity>().IsActive()) {
            var hitterGo = collision.gameObject;
            var isExplosion = hitterGo.GetComponent<ExplosionAnimation>() != null;
            if (hitterGo.CompareTag(Tags.ENEMY_HITTER) || isExplosion) {
                if (isExplosion || hitterGo.GetComponent<InvulnerabilityHalo>() != null) {
                    AudioManager.GetInstance().PlayEffect(Sfx.ENEMY_HIT);
                    KillEnemy(false);
                } else {
                    ReceiveHit(false);
                    if (hitterGo.GetComponent<CrossThrow>() != null) {
                        hitterGo.GetComponent<CrossThrow>().Break();
                    }
                }
            }
        }
    }

    public void ReceiveHit(bool throughFeet) {
        if (invuln != null && !invuln.IsInvulnerable()) {
            AudioManager.GetInstance().PlayEffect(Sfx.ENEMY_HIT);
            if (deathType == DeathType.SLIME || deathType == DeathType.DIVIDE) {
                if (hits > 0) {
                    var enemyComponent = GetComponent<Enemy>();
                    SpawnChildren(enemyComponent);
                } else {
                    KillEnemy(throughFeet);
                }
            } else {
                hits--;
                if (hits <= 0) {
                    KillEnemy(throughFeet);
                } else {
                    invuln.SetInvulnerable(GameplayValues.GetEnemyInvulnerableTime());
                }
            }
        }
    }

    public bool IsBeingHit() {
        return isBeingHit;
    }

    public void SetNotBeingHit() {
        isBeingHit = false;
    }

    private void KillEnemy(bool throughFeet) {
        GameState.score += 100;

        var player = ObjectLocator.GetPlayer();
        if (player != null) {
            var pos = new Vector2(player.transform.position.x, player.transform.position.y + 0.3f);
            // Uh...
            // Make a resource manager with a pool of scores
            // Also add more scoring stuff
            Instantiate(Resources.Load<GameObject>("Prefabs/Scores/Score100"), pos, player.transform.rotation);
        }

        // Type of death
        // TODO: Improve
        if (deathType == DeathType.ICE || deathType == DeathType.DIVIDE) {
            var iceCube = Resources.Load<GameObject>("Prefabs/Props/IceCube");
            var pos = gameObject.transform.position;
            Instantiate(iceCube, new Vector3(pos.x - 0.02f, pos.y - 0.01f, pos.z), gameObject.transform.rotation);
            Instantiate(iceCube, new Vector3(pos.x - 0.01f, pos.y - 0.01f, pos.z), gameObject.transform.rotation);
            Instantiate(iceCube, new Vector3(pos.x, pos.y - 0.01f, pos.z), gameObject.transform.rotation);
            Instantiate(iceCube, new Vector3(pos.x + 0.01f, pos.y - 0.01f, pos.z), gameObject.transform.rotation);
            Instantiate(iceCube, new Vector3(pos.x + 0.02f, pos.y - 0.01f, pos.z), gameObject.transform.rotation);
            MarkAsDead();
        } else {
            var prefab = "Blood";
            // Pff I mean this is utter crap
            if (deathType == DeathType.EXPLOSION) {
                prefab = "Explosion";
                AudioManager.GetInstance().PlayEffect(Sfx.EXPLOSION, 1f);
            }
            if (deathType == DeathType.SLIME) {
                prefab = "Poison";
            }
            Instantiate(Resources.Load<GameObject>("Prefabs/" + prefab), gameObject.transform.position, gameObject.transform.rotation);
            MarkAsDead();
        }

        if (throughFeet) {
            AchievementManager.AddJumpKillCount();
        }

        gameObject.SetActive(false);
    }

    // Specific death that creates smaller children with 1 hit point less.
    private void SpawnChildren(Enemy enemyComponent) {
        MarkAsDead();

        var pos = enemyComponent != null? enemyComponent.initialPosition : gameObject.transform.position;
        var firstChild = Instantiate(gameObject, new Vector2(pos.x - 0.23f, pos.y - 0.07f), gameObject.transform.rotation);
        var secondChild = Instantiate(gameObject, new Vector2(pos.x + 0.23f, pos.y - 0.07f), gameObject.transform.rotation);
        
        // Make them smaller.
        var theScale = firstChild.transform.localScale;
        firstChild.transform.localScale = new Vector3(theScale.x * 0.75f, theScale.y * 0.75f, theScale.z);
        theScale = secondChild.transform.localScale;
        secondChild.transform.localScale = new Vector3(theScale.x * 0.75f, theScale.y * 0.75f, theScale.z);
        gameObject.SetActive(false);

        // Make the children invulnerable.
        firstChild.GetComponent<EnemyInvulnerability>().SetInvulnerable(GameplayValues.GetEnemyInvulnerableTime());
        secondChild.GetComponent<EnemyInvulnerability>().SetInvulnerable(GameplayValues.GetEnemyInvulnerableTime());

        // Give them the hitpoints.
        firstChild.GetComponent<PlayerHitReceiver>().hits = hits - 1;
        secondChild.GetComponent<PlayerHitReceiver>().hits = hits - 1;
    }

    private void MarkAsDead() {
        var enemyEntity = gameObject.GetComponent<IEnemyEntity>();
        // So we can use this with destructibles.
        if (enemyEntity != null) {
            GameState.GetInstance().enemiesKilled.Add(enemyEntity.GetUid());
        }
    }
}

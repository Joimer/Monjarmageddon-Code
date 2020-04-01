using UnityEngine;

public class Scientist : BossEntity {

    public GameObject frontRay;

    private GameObject target;
    // It's separate because the sprite is bigger than the Scientist's original.
    // We don't want the original scientist sprite to be bigger than necessary.
    // Though it could be an animation and move the transform... Should check that.
    private GameObject transformationObject;
    // The clone is a separate object which acts as a regular enemy.
    private GameObject transformingClone;
    private GameObject clone;
    private float lastShot = 0f;
    private float lastBlueShot = 0f;
    private float fireRate = 2f;
    private float blueFireRate = 1f;
    private float lastHomingShot = 0f;
    private float homingShotRate = 1f;
    private float transformationStart = 0f;
    private bool oddShot = true;
    private bool switchedPhase = false;
    private bool transforming = false;
    private bool cloneTransforming = false;
    private float shotSpeed = GameplayValues.GetEnemyShootSpeed() + 100f;
    private Vector2 cloneShotPos = new Vector2(118.86f, -9.9f);
    private Vector2 transformationPos = new Vector2(123.925f, -9.931f);
    private Vector2 clonePos = new Vector2(118.88f, -9.88f);
    private SpriteRenderer spriteRenderer;
    private float rayActivationTime = 0.5f;
    private float interphaseTime = 2f;
    // Projectile object pools
    private Vector2 globePos = new Vector2(123.22f, -9.05f);
    private GameObject[] globes = new GameObject[5];
    private int globePoolPos = 0;
    private bool activatedCommieFire = false;
    private ObjectPool fireballPool;
    // REFACTOR
    private GameObject blueRayPrefab;
    // This is just for decorative purposes. Looks good.
    public GameObject counter1;
    public GameObject counter2;
    private Sprite[] counters;
    // Clone stuff
    private GameObject fireFlask;

    private void Awake() {
        // Set up object pools.
        // Globe projectiles.
        var prefab = Resources.Load<GameObject>(Hazards.GLOBE);
        fireFlask = Instantiate(Resources.Load<GameObject>(Hazards.FIRE_FLASK), cloneShotPos, transform.rotation);
        fireFlask.SetActive(false);
        globes[0] = Instantiate(prefab, globePos, transform.rotation);
        globes[0].SetActive(false);
        globes[1] = Instantiate(prefab, globePos, transform.rotation);
        globes[1].SetActive(false);
        globes[2] = Instantiate(prefab, globePos, transform.rotation);
        globes[2].SetActive(false);
        globes[3] = Instantiate(prefab, globePos, transform.rotation);
        globes[3].SetActive(false);
        globes[4] = Instantiate(prefab, globePos, transform.rotation);
        globes[4].SetActive(false);
        var fireball = Resources.Load<GameObject>(Hazards.FIREBALL);
        fireballPool = new ObjectPool(fireball, 12);
        blueRayPrefab = Resources.Load<GameObject>(Hazards.BLUE_RAY_SHOT);
        counters = new Sprite[] {
            Resources.Load<Sprite>("Sprites/Lab/Counter/marcador_0"),
            Resources.Load<Sprite>("Sprites/Lab/Counter/marcador_1"),
            Resources.Load<Sprite>("Sprites/Lab/Counter/marcador_2"),
            Resources.Load<Sprite>("Sprites/Lab/Counter/marcador_3"),
            Resources.Load<Sprite>("Sprites/Lab/Counter/marcador_4"),
            Resources.Load<Sprite>("Sprites/Lab/Counter/marcador_5"),
            Resources.Load<Sprite>("Sprites/Lab/Counter/marcador_6"),
            Resources.Load<Sprite>("Sprites/Lab/Counter/marcador_7"),
            Resources.Load<Sprite>("Sprites/Lab/Counter/marcador_8"),
            Resources.Load<Sprite>("Sprites/Lab/Counter/marcador_9")
        };
    }

    private void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        target = ObjectLocator.GetPlayer();
        phases = 2;
        if (GameState.difficulty == Difficulty.VERY_EASY) {
            rayActivationTime = 1f;
            interphaseTime = 3f;
            blueFireRate = 1.5f;
            shotSpeed -= 50f;
        }
        if (GameState.difficulty == Difficulty.EASY) {
            rayActivationTime = 0.75f;
            interphaseTime = 2.5f;
            blueFireRate = 1.3f;
            shotSpeed -= 25f;
        }
        if (GameState.difficulty == Difficulty.HARD) {
            interphaseTime = 1.75f;
        }
        if (GameState.difficulty == Difficulty.EXTREME) {
            interphaseTime = 1.5f;
        }
    }

    private void FixedUpdate() {
        if (!GameState.isGameLocked && active) {
            if (!activatedCommieFire && target.transform.position.x >= 123.2f) {
                activatedCommieFire = true;
                gameObject.AddComponent<CommieFire>();
            }

            // Alternate fire rate slightly so there's no obvious safe position.
            var currFireRate = oddShot ? fireRate : fireRate - 0.2f;
            if (target != null && lastShot + currFireRate < Time.time) {
                // The front ray that blocks incoming crosses disappears for a moment when the Scientist shoots.
                if (frontRay != null) {
                    frontRay.SetActive(false);
                }
                if (phase == 1) {
                    Shoot();
                } else {
                    ShootPhase3();
                }
            }

            // Regular blue ray shot that provides difficulty moving.
            if (target != null && lastBlueShot + blueFireRate < Time.time) {
                if (target.transform.position.x < transform.position.x) {
                    BlueShot();
                } else {
                    HomingBlueShot();
                }
            }

            // Re-activate the ray after some time, depending on the difficulty.
            if (Time.time - lastShot > rayActivationTime && frontRay != null) {
                frontRay.SetActive(true);
            }

            // This state means the phase has just switched.
            if (phase == 2 && !switchedPhase) {
                switchedPhase = true;
                frontRay.SetActive(true);
                transformationStart = Time.time;
                transforming = true;
                spriteRenderer.enabled = false;
                // The animation lasts 1 sec 10 ms.
                // Interphase time lasts a little longer.
                transformationObject = Instantiate(Resources.Load<GameObject>(Animations.SCIENTIST_TRANSFORMATION), transformationPos, transform.rotation);
                // Explode globes that aren't just in front of the player (probably).
                for (var i = 0; i < globes.Length -1; i++) {
                    if (globes[i].transform.position.x < 119f && globes[i].activeSelf) {
                        globes[i].GetComponent<Globe>().ShootCircle();
                    }
                }
            }

            if (transforming && Time.time - transformationStart > interphaseTime) {
                // Finish drinking animation and start clone instancing. This anim lasts 0.5 secs.
                Destroy(transformationObject);
                transformingClone = Instantiate(Resources.Load<GameObject>(Animations.SCIENTIST_CLONE_APPEAR), clonePos, transform.rotation);
                spriteRenderer.enabled = true;
                transforming = false;
                cloneTransforming = true;
                transformationStart = Time.time;
                GetComponent<Animator>().Play(Animator.StringToHash(Animations.SCIENTIST_HULK));
            }

            if (cloneTransforming && Time.time - transformationStart > interphaseTime) {
                Destroy(transformingClone);
                // Instance clone in the opposite side
                clone = Instantiate(Resources.Load<GameObject>(Bosses.LAB_CLONE), clonePos, transform.rotation);
                transforming = false;
                cloneTransforming = false;
                phase = 3;
                lastHomingShot = Time.time;
                lastShot = Time.time;
                // Explode remaining globes active.
                for (var i = 0; i < globes.Length - 1; i++) {
                    if (globes[i].activeSelf) {
                        globes[i].GetComponent<Globe>().ShootCircle();
                    }
                }
            }

            if (phase == 3 && clone != null && target != null && target.transform.position.x <= clone.transform.position.x && lastHomingShot + homingShotRate < Time.time) {
                HomingShot();
            }
        }
    }

    // Overrides base so feet jump triggers an attack.
    public new void OnTriggerEnter2D(Collider2D collision) {
        if (!GameState.isGameLocked && active) {
            if (!invulnerable && collision.CompareTag(Tags.ENEMY_HITTER)) {
                ReceiveHit();
            }

            // TODO: Hit management for the boss and enemies can be done through an event.
            // Add an event subscription to hitters on here and call it through the super class.
            // Me cago en mi puta vida pero cómo hice este pedazo de basura
            if (collision.gameObject.name == Constants.FEET_COLLIDER) {
                if (!invulnerable) {
                    ReceiveHit();
                }
                if (target == null) {
                    target = ObjectLocator.GetPlayer();
                }
                if (target != null) {
                    var playerMovement = target.GetComponent<PlatformerMovement2D>();
                    if (playerMovement != null) {
                        playerMovement.FeetCollided();
                        playerMovement.Jump(true);
                        playerMovement.GetComponent<Invulnerability>().SetInvulnerableNoBlink(0.1f);
                        ShootCircle();
                    }
                }
            }
        }
    }

    // Blue ray shot that the main boss shoots in regular intervals.
    private void BlueShot() {
        var fireBall = Instantiate(blueRayPrefab, transform.position, transform.rotation);
        fireBall.GetComponent<Rigidbody2D>().AddForce(Vector2.left * shotSpeed);
        fireBall.GetComponent<EnemyProjectile>().ignoreCollisions = true;
        fireBall.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90));
        lastBlueShot = Time.time;
    }

    // Homing blue shot that takes place instead of the regular if you go behind the boss.
    private void HomingBlueShot() {
        lastBlueShot = Time.time;
        var fireBall = Instantiate(blueRayPrefab, transform.position, transform.rotation);
        Vector2 direction = (target.transform.position - fireBall.transform.position).normalized;
        fireBall.GetComponent<Rigidbody2D>().AddForce(direction * shotSpeed);
    }

    // Globe spawn for phase 1.
    private void Shoot() {
        if (phase == 1) {
            lastShot = Time.time;
            var globe = globes[globePoolPos];
            globePoolPos++;
            if (globePoolPos > globes.Length - 1) {
                globePoolPos = 0;
            }
            globe.transform.position = globePos;
            globe.SetActive(true);
            oddShot = !oddShot;
        }
    }   

    // Frog shooting that happens in second phase (phase 2 is technically transformation and 3 the attacking one).
    private void ShootPhase3() {
        if (phase == 3) {
            lastShot = Time.time;
            // Scientist shoots frogs
            // TODO: Move to a pool or reuse object.
            var frog = Instantiate(Resources.Load<GameObject>(Hazards.FROG), new Vector2(123.4933f, -10.22661f), transform.rotation);
            frog.GetComponent<Rigidbody2D>().AddForce(Vector2.left * shotSpeed);
            Destroy(frog, 6f);
            oddShot = !oddShot;

            // Clone shoots exploding flasks
            if (clone != null && clone.activeSelf && clone.GetComponent<PlayerHitReceiver>().hits > 0) {
                fireFlask.transform.position = cloneShotPos;
                fireFlask.GetComponent<FireFlask>().SetPositions(cloneShotPos);
                fireFlask.SetActive(true);
            }
        }
    }

    // Clone's homing shot if you get behind him.
    private void HomingShot() {
        lastHomingShot = Time.time;
        var fireBall = Instantiate(blueRayPrefab, cloneShotPos, transform.rotation);
        Vector2 direction = (target.transform.position - fireBall.transform.position).normalized;
        fireBall.GetComponent<Rigidbody2D>().AddForce(direction * shotSpeed);
    }

    private GameObject GetNextFireball() {
        var fireball = fireballPool.RetrieveNext();
        fireball.transform.position = transform.position;
        fireball.SetActive(true);
        fireball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        fireball.GetComponent<Rigidbody2D>().angularVelocity = 0f;
        return fireball;
    }

    private void ShootCircle() {
        var fireBall = GetNextFireball();
        var shootingSpeed = 100f;
        fireBall.GetComponent<Rigidbody2D>().AddForce(Vector2.right * shootingSpeed);
        fireBall = GetNextFireball();
        fireBall.GetComponent<Rigidbody2D>().AddForce(Vector2.left * shootingSpeed);
        fireBall = GetNextFireball();
        fireBall.GetComponent<Rigidbody2D>().AddForce(Vector2.up * shootingSpeed);
        fireBall = GetNextFireball();
        fireBall.GetComponent<Rigidbody2D>().AddForce(Vector2.down * shootingSpeed);
        fireBall = GetNextFireball();
        fireBall.GetComponent<Rigidbody2D>().AddForce(new Vector2(0.66f, 0.66f) * shootingSpeed);
        fireBall = GetNextFireball();
        fireBall.GetComponent<Rigidbody2D>().AddForce(new Vector2(0.66f, -0.66f) * shootingSpeed);
        fireBall = GetNextFireball();
        fireBall.GetComponent<Rigidbody2D>().AddForce(new Vector2(-0.66f, 0.66f) * shootingSpeed);
        fireBall = GetNextFireball();
        fireBall.GetComponent<Rigidbody2D>().AddForce(new Vector2(-0.66f, -0.66f) * shootingSpeed);
    }

    public void SetCounter() {
        var n = phases - phasesDone == 2? hits + 8 : hits;
        var first = n > 9 ? 1 : 0;
        var second = n > 9 ? n - 10 : n;
        if (second < 0) {
            second = 0;
        }
        SetCounter(first, second);
    }

    public void SetCounter(int a, int b) {
        counter1.GetComponent<SpriteRenderer>().sprite = counters[a];
        counter2.GetComponent<SpriteRenderer>().sprite = counters[b];
    }

    // kimochi warui
    // This is just for the counter
    // Should just rewrite the boss system.
    protected new void ReceiveHit() {
        ReceiveHit(1);
    }

    protected new void ReceiveHit(int number) {
        hits -= number;
        if (hits > 0) {
            AudioManager.GetInstance().PlayEffect(Sfx.BOSS_HIT);
            StartCoroutine("SetInvulnerable", invulnerableTime);
        } else {
            phasesDone++;
            if (phases == phasesDone) {
                Die();
            } else {
                hits = GameplayValues.GetBossHits();
                phase++;
            }
        }
        SetCounter();
    }

    public override void OnDeath() {
        AchievementManager.UnlockAchievement(Achievements.BOSS_LAB_VEZ);
        if (GameState.difficulty == Difficulty.EXTREME) {
            AchievementManager.UnlockAchievement(Achievements.BOSS_LAB_EXTREME);
        }
        if (GameState.difficulty >= Difficulty.HARD) {
            AchievementManager.UnlockAchievement(Achievements.BOSS_LAB_HARD);
        }
        if (GameState.difficulty >= Difficulty.MEDIUM) {
            AchievementManager.UnlockAchievement(Achievements.BOSS_LAB);
        }
        if (GameState.difficulty >= Difficulty.EASY) {
            AchievementManager.UnlockAchievement(Achievements.BOSS_LAB_EZ);
        }
    }
}

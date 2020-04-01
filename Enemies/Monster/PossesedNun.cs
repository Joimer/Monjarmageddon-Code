using UnityEngine;

public class PossesedNun : MonoBehaviour, IEnemyEntity {

    private Vector3 initialPosition;
    private float fireRate = 1f;
    private float movementRate = 0.5f;
    private float lastShot = 0f;
    private bool shooting = false;
    private bool activating = false;
    private float spawnTime = 0f;
    private bool active = false;
    private GameObject target;
    private string uid;
    private float activeDistance = 2.6f;
    private float shootingSpeed = GameplayValues.GetEnemyShootSpeed();
    private float activatingTime = 0.65f;
    private float moveSpeed = -0.03f;
    private float pathLength = 5f;
    // Cambiar a pool
    private GameObject fireball;

    private void Awake() {
        uid = transform.position.ToString();
        if (GameState.difficulty == Difficulty.VERY_EASY) {
            activeDistance = 2f;
            activatingTime = 1.1f;
            moveSpeed = -0.015f;
            pathLength = 3f;
        }
        if (GameState.difficulty == Difficulty.EASY) {
            activeDistance = 2.3f;
            activatingTime = 0.9f;
            pathLength = 4f;
        }
        if (GameState.difficulty == Difficulty.HARD) {
            activatingTime = 0.55f;
        }
        if (GameState.difficulty == Difficulty.EXTREME) {
            activatingTime = 0.33f;
        }
        fireball = Resources.Load<GameObject>(Hazards.FIREBALL);
    }

    private void Start() {
        initialPosition = GetComponent<Transform>().position;
        Color color = GetComponent<SpriteRenderer>().color;
        color.a = GameState.difficulty == Difficulty.EXTREME ? 0.2f : 0.33f;
        GetComponent<SpriteRenderer>().color = color;
        target = ObjectLocator.GetPlayer();

        if (GameState.lastCheckpoint != null && GameState.lastCheckpoint.enemiesKilled.Contains(uid)) {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate() {
        if (!GameState.isGameLocked && target && target.GetComponent<Transform>() != null) {
            var distance = Vector2.Distance(transform.position, target.transform.position);
            if (!active && !activating && distance < activeDistance) {
                Activate();
            }

            if (activating) {
                Color color = GetComponent<SpriteRenderer>().color;
                color.a += 0.03f;
                GetComponent<SpriteRenderer>().color = color;
            }

            if (activating && Time.time > spawnTime + activatingTime) {
                activating = false;
                active = true;
            }

            if (active) {
                if (lastShot + fireRate < Time.time) {
                    Shoot();
                }
                if (!shooting) {
                    transform.Translate(new Vector2(moveSpeed, 0f));
                }
            }

            if (lastShot + movementRate < Time.time) {
                shooting = false;
            }
        }
	}

    private void Update() {
        if (!GameState.isGameLocked && (active || activating) && transform.position.x < initialPosition.x - pathLength) {            
            GameState.GetInstance().enemiesKilled.Add(uid);
            Destroy(gameObject);
        }
    }

    public string GetUid() {
        return uid;
    }

    private void Activate() {
        activating = true;
        spawnTime = Time.time;
        GetComponent<Renderer>().enabled = true;
    }

    private void Shoot() {
        if (gameObject.activeSelf && target != null && GameState.difficulty != Difficulty.VERY_EASY && Vector2.Distance(transform.position, target.transform.position) <= activeDistance) {
            lastShot = Time.time;
            var fireBall = Instantiate(fireball, transform.position, transform.rotation);
            Vector2 direction = (target.transform.position - fireBall.transform.position).normalized;
            fireBall.GetComponent<Rigidbody2D>().AddForce(direction * shootingSpeed);
        }
    }

    public bool IsActive() {
        return activating || active;
    }
}

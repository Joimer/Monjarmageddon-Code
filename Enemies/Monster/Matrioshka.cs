using UnityEngine;

public class Matrioshka : MonoBehaviour, IEnemyEntity {

    private bool isMovingUp = true;
    private float movementRange = 0.4f;
    private string uid;
    private float moveSpeed = 0.075f;
    [HideInInspector]
    public bool facingRight = false;
    private float fireRate = 1f;
    private float lastShot = 0f;
    private float activeDistance = 3f;
    private float shootingSpeed = GameplayValues.GetEnemyShootSpeed() + 100f;
    private GameObject target;
    private Enemy enemyComponent;

    private void Awake() {
        uid = transform.position.ToString();
        if (GameState.difficulty == Difficulty.VERY_EASY) {
            moveSpeed = 0.01f;
            movementRange = 1.3f;
        }
    }

    private void Start() {
        target = ObjectLocator.GetPlayer();
        enemyComponent = GetComponent<Enemy>();

        if (GameState.lastCheckpoint != null && GameState.lastCheckpoint.enemiesKilled.Contains(uid)) {
            Destroy(gameObject);
        }
    }

    void FixedUpdate() {
        if (!GameState.isGameLocked) {
            if (isMovingUp) {
                if (transform.position.y < enemyComponent.initialPosition.y + movementRange) {
                    transform.Translate(new Vector2(0f, moveSpeed));
                } else {
                    isMovingUp = false;
                }
            }

            if (!isMovingUp) {
                if (transform.position.y > enemyComponent.initialPosition.y) {
                    transform.Translate(new Vector2(0f, -moveSpeed / 2));
                } else {
                    isMovingUp = true;
                }
            }

            if (target != null) {
                var distance = Vector2.Distance(transform.position, target.transform.position);
                if (distance < activeDistance && lastShot + fireRate < Time.time) {
                    Shoot();
                }
            }
        }
    }

    private void Shoot() {
        if (gameObject.activeSelf && Vector2.Distance(transform.position, target.transform.position) <= activeDistance) {
            lastShot = Time.time;
            var fireBall = Instantiate(Resources.Load<GameObject>(Hazards.HAMMER), transform.position, transform.rotation);
            Vector2 direction = target.transform.position.x < transform.position.x ? Vector2.left : Vector2.right;
            fireBall.GetComponent<Rigidbody2D>().AddForce(direction * shootingSpeed);
            Destroy(fireBall, 8f);
        }
    }

    public string GetUid() {
        return uid;
    }

    public bool IsActive() {
        return true;
    }
}

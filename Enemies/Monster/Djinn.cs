using UnityEngine;

public class Djinn : MonoBehaviour, IEnemyEntity {

    private string uid;
    private Vector3 initialPosition;
    private bool isMovingLeft = true;
    private bool isMovingUp = true;
    private float movementRange = 0.5f;
    private float verticalMoveSpeed = 0.02f;
    private float horizontalSpeed = 0.007f;
    private float lastShot = 0f;
    private float shootingSpeed = GameplayValues.GetEnemyShootSpeed();
    public GameObject target;
    private float activeDistance = 2.8f;
    private float fireRate = 1f;

    private void Awake() {
        uid = transform.position.ToString();
        if (GameState.difficulty == Difficulty.VERY_EASY) {
            verticalMoveSpeed = 0.01f;
            fireRate = 1.5f;
        }
    }

    void Start() {
        initialPosition = transform.position;
        target = ObjectLocator.GetPlayer();

        if (GameState.lastCheckpoint != null && GameState.lastCheckpoint.enemiesKilled.Contains(uid)) {
            Destroy(gameObject);
        }
    }

    void FixedUpdate() {
        if (!GameState.isGameLocked) {
            if (isMovingLeft) {
                if (transform.position.x > initialPosition.x - movementRange / 4) {
                    transform.Translate(new Vector2(-horizontalSpeed, 0f));
                } else {
                    isMovingLeft = !isMovingLeft;
                }
            }

            if (!isMovingLeft) {
                if (transform.position.x < initialPosition.x + movementRange / 4) {
                    transform.Translate(new Vector2(horizontalSpeed, 0f));
                } else {
                    isMovingLeft = !isMovingLeft;
                }
            }

            if (isMovingUp) {
                if (transform.position.y < initialPosition.y + movementRange) {
                    transform.Translate(new Vector2(0f, verticalMoveSpeed));
                } else {
                    isMovingUp = !isMovingUp;
                }
            }

            if (!isMovingUp) {
                if (transform.position.y > initialPosition.y) {
                    transform.Translate(new Vector2(0f, -verticalMoveSpeed));
                } else {
                    isMovingUp = !isMovingUp;
                }
            }

            if (lastShot + fireRate < Time.time) {
                Shoot();
            }
        }
    }

    private void Shoot() {
        if (gameObject.activeSelf && target != null && Vector2.Distance(transform.position, target.transform.position) <= activeDistance) {
            lastShot = Time.time;
            var fbp = Resources.Load<GameObject>(Hazards.FIREBALL);
            var fireBall = Instantiate(fbp, transform.position, transform.rotation);
            var fireBallTwo = Instantiate(fbp, transform.position, transform.rotation);
            var fireBallThree = Instantiate(fbp, transform.position, transform.rotation);
            fireBall.GetComponent<Rigidbody2D>().AddForce(Vector2.down * shootingSpeed);
            fireBallTwo.GetComponent<Rigidbody2D>().AddForce(new Vector2(0.75f, -1f) * shootingSpeed);
            fireBallThree.GetComponent<Rigidbody2D>().AddForce(new Vector2(-0.75f, -1f) * shootingSpeed);
        }
    }

    public string GetUid() {
        return uid;
    }

    public bool IsActive() {
        return true;
    }
}

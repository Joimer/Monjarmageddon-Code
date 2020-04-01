using UnityEngine;

public class Burkashabh : MonoBehaviour, IEnemyEntity {

    private Vector3 initialPosition;
    private float fireRate = 1f;
    private float movementRate = 0.5f;
    private float lastShot = 0f;
    private bool shooting = false;
    private bool activating = false;
    private float spawnTime = 0f;
    private bool active = false;
    public GameObject target;
    private string uid;
    private float activeDistance = 2.6f;
    private float shootingSpeed = GameplayValues.GetEnemyShootSpeed() + 15f;
    private float activatingTime = 0.5f;
    private float moveSpeed = 0.03f;
    private float movementRange = 2.5f;
    private bool isMovingLeft = true;
    private bool facingRight = false;

    private void Awake() {
        uid = transform.position.ToString();
        if (GameState.difficulty == Difficulty.VERY_EASY) {
            activeDistance = 2f;
            activatingTime = 1f;
            moveSpeed = 0.015f;
            movementRange = 2f;
        }
    }

    private void Start() {
        initialPosition = GetComponent<Transform>().position;
        Color color = GetComponent<SpriteRenderer>().color;
        color.a = GameState.difficulty == Difficulty.EXTREME ? 0.1f : 0.2f;
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
                color.a += 0.05f;
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
                    if (isMovingLeft) {
                        if (transform.position.x > initialPosition.x - movementRange) {
                            transform.Translate(new Vector2(-moveSpeed, 0f));
                        } else {
                            Turn();
                        }
                    }

                    if (!isMovingLeft) {
                        if (transform.position.x < initialPosition.x + movementRange) {
                            transform.Translate(new Vector2(moveSpeed, 0f));
                        } else {
                            Turn();
                        }
                    }
                }
            }

            if (lastShot + movementRate < Time.time) {
                shooting = false;
            }
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

    // TODO: Refactor (shooting component)
    private void Shoot() {
        if (gameObject.activeSelf && Vector2.Distance(transform.position, target.transform.position) <= activeDistance) {
            lastShot = Time.time;
            var fireBall = Instantiate(Resources.Load<GameObject>(Hazards.FIREBALL), transform.position, transform.rotation);
            Vector2 direction = (target.transform.position - fireBall.transform.position).normalized;
            fireBall.GetComponent<Rigidbody2D>().AddForce(direction * shootingSpeed);
        }
    }

    public bool IsActive() {
        return activating || active;
    }

    void Flip() {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void Turn() {
        isMovingLeft = !isMovingLeft;
        Flip();
    }
}

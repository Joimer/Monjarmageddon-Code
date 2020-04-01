using UnityEngine;

public class TumorNun : MonoBehaviour, IEnemyEntity {

    private Vector3 initialPosition;
    private float fireRate = 1.1f;
    private float longFireRate = 1.1f;
    private float shortFireRate = 0.3f;
    private float movementRate = 0.5f;
    private float lastShot = 0f;
    private bool shooting = false;
    private bool activating = false;
    private float spawnTime = 0f;
    private bool active = false;
    private GameObject target;
    private string uid;
    private float activeDistance = 2.6f;
    private float shootingSpeed = GameplayValues.GetEnemyShootSpeed() + 15f;
    private float activatingTime = 0.5f;
    private float moveSpeed = 0.04f;
    private float movementRange = 2.5f;
    private bool isMovingLeft = true;
    private bool facingRight = false;
    private int fired = 0;

    private void Awake() {
        uid = transform.position.ToString();
        if (GameState.difficulty == Difficulty.VERY_EASY) {
            activeDistance = 2f;
            activatingTime = 1f;
            moveSpeed = 0.025f;
            movementRange = 2f;
        }

        fireRate = longFireRate;
    }

    private void Start() {
        if (GameState.lastCheckpoint != null && GameState.lastCheckpoint.enemiesKilled.Contains(uid)) {
            Destroy(gameObject);
        } else {
            initialPosition = GetComponent<Transform>().position;
            Color color = GetComponent<SpriteRenderer>().color;
            color.a = GameState.difficulty == Difficulty.EXTREME ? 0.1f : 0.2f;
            GetComponent<SpriteRenderer>().color = color;
            target = ObjectLocator.GetPlayer();
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
                    fired++;
                    if (fired == 1) {
                        fireRate = shortFireRate;
                    }
                    if (fired == 2) {
                        fired = 0;
                        fireRate = longFireRate;
                    }
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

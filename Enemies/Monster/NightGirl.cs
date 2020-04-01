using UnityEngine;

public class NightGirl : MonoBehaviour, IEnemyEntity {

    private Vector3 initialPosition;
    private bool isMovingLeft = true;
    private float movementRange = 1.5f;
    private string uid;
    // Shooting properties
    public GameObject target;
    private float fireRate = 1f;
    private float lastShot = 0f;
    private float activeDistance = 3f;
    private float shootingSpeed = GameplayValues.GetEnemyShootSpeed() + 20f;
    [HideInInspector]
    public bool facingRight = false;
    private float moveSpeed = 0.03f;
    private float projectileDuration = 8f;

    private void Awake() {
        uid = transform.position.ToString();
        if (GameState.difficulty == Difficulty.VERY_EASY) {
            moveSpeed = 0.02f;
            movementRange = 1.3f;
            projectileDuration = 5f;
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
        if (!GameState.isGameLocked && target != null) {
            var distance = Vector2.Distance(transform.position, target.transform.position);
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

            if (distance < activeDistance && lastShot + fireRate < Time.time) {
                Shoot();
            }

        }
    }

    public string GetUid() {
        return uid;
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

    public void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag(Tags.ENEMY_COLLISION)) {
            Turn();
        }
    }

    private void Shoot() {
        if (gameObject.activeSelf && Vector2.Distance(transform.position, target.transform.position) <= activeDistance) {
            lastShot = Time.time;
            var proj = Resources.Load<GameObject>(Hazards.RAINBOW);
            if (!isMovingLeft || GameState.difficulty == Difficulty.EXTREME) {
                var rainbow = Instantiate(proj, transform.position, transform.rotation);
                rainbow.GetComponent<Rigidbody2D>().AddForce(Vector2.right * shootingSpeed);
                Destroy(rainbow, projectileDuration);
            }
            if (isMovingLeft || GameState.difficulty == Difficulty.EXTREME) {
                var rainbow2 = Instantiate(proj, transform.position, transform.rotation);
                rainbow2.GetComponent<Rigidbody2D>().AddForce(Vector2.left * shootingSpeed);
                Destroy(rainbow2, projectileDuration);
            }
            if (GameState.difficulty != Difficulty.VERY_EASY) {
                var rainbow3 = Instantiate(proj, transform.position, transform.rotation);
                rainbow3.GetComponent<Rigidbody2D>().AddForce(Vector2.up * shootingSpeed);
                Destroy(rainbow3, projectileDuration);
            }
        }
    }

    public bool IsActive() {
        return true;
    }
}

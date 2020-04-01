using UnityEngine;

public class Doctor : MonoBehaviour, IEnemyEntity {

    private Vector3 initialPosition;
    private bool isMovingLeft = true;
    private float movementRange = 1.5f;
    private string uid;
    // Shooting properties
    public GameObject target;
    private float fireRate = 1f;
    private float lastShot = 0f;
    private float activeDistance = 3f;
    private float shootingSpeed = GameplayValues.GetEnemyShootSpeed();

    // Delete
    [HideInInspector]
    public bool facingRight = false;

    private void Awake() {
        uid = transform.position.ToString();
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
                    transform.Translate(new Vector2(-0.03f, 0f));
                } else {
                    Turn();
                }
            }

            if (!isMovingLeft) {
                if (transform.position.x < initialPosition.x + movementRange) {
                    transform.Translate(new Vector2(0.03f, 0f));
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

    // TODO: Delete
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
            var fireBall = Instantiate(Resources.Load<GameObject>(Hazards.VACCINE), transform.position, transform.rotation);
            Vector2 direction = Vector2.right;
            if (isMovingLeft) {
                direction = -direction;
            } else {
                Vector3 theScale = fireBall.transform.localScale;
                theScale.x *= -1;
                fireBall.transform.localScale = theScale;
            }
            fireBall.GetComponent<Rigidbody2D>().AddForce(direction * shootingSpeed);
            Destroy(fireBall, 8f);
        }
    }

    public bool IsActive() {
        return true;
    }
}

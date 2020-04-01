using UnityEngine;

public class Robot : MonoBehaviour, IEnemyEntity {

    private string uid;
    private bool isMovingLeft = true;
    private bool lookingLeft = true;
    private float horizontalSpeed = 0.03f;
    private float lastShot = 0f;
    private float shootingSpeed = GameplayValues.GetEnemyShootSpeed();
    private GameObject target;
    private float activeDistance = 2.6f;
    private float fireRate = 1f;
    private bool rightLocked = false;
    private bool leftLocked = false;
    private bool targetToTheLeft = true;

    private void Awake() {
        uid = transform.position.ToString();
    }

    void Start() {
        target = ObjectLocator.GetPlayer();

        if (GameState.lastCheckpoint != null && GameState.lastCheckpoint.enemiesKilled.Contains(uid)) {
            Destroy(gameObject);
        }
    }

    void FixedUpdate() {
        if (!GameState.isGameLocked) {
            if (target != null && Vector2.Distance(transform.position, target.transform.position) <= activeDistance) {
                targetToTheLeft = target.transform.position.x < transform.position.x;
                // Nun is to the left (lesser x), moves to the right
                if (targetToTheLeft && !rightLocked) {
                    transform.Translate(new Vector2(horizontalSpeed, 0f));
                    isMovingLeft = false;
                    leftLocked = false;
                    if (lookingLeft) {
                        Flip();
                    }
                }

                // Nun is to the right (higher x), moves to the left.
                if (!targetToTheLeft && !leftLocked) {
                    transform.Translate(new Vector2(-horizontalSpeed, 0f));
                    isMovingLeft = true;
                    rightLocked = false;
                    if (!lookingLeft) {
                        Flip();
                    }
                }

                if (lastShot + fireRate < Time.time) {
                    Shoot();
                }
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag(Tags.ENEMY_COLLISION)) {
            if (isMovingLeft) {
                leftLocked = true;
            } else {
                rightLocked = true;
            }
        }
    }

    private void Shoot() {
        if (!gameObject.activeSelf) {
            return;
        }
        lastShot = Time.time;
        var fireBall = Instantiate(Resources.Load<GameObject>(Hazards.FIREBALL), transform.position, transform.rotation);
        fireBall.transform.localScale = new Vector3(fireBall.transform.localScale.x * 2f, fireBall.transform.localScale.y * 2f, fireBall.transform.localScale.z);
        var force = targetToTheLeft ? Vector2.left : Vector2.right;
        fireBall.GetComponent<Rigidbody2D>().AddForce(force * shootingSpeed);
        fireBall.AddComponent<PartedShot>();
    }

    public string GetUid() {
        return uid;
    }

    public bool IsActive() {
        return true;
    }

    private void Flip() {
        lookingLeft = !lookingLeft;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}

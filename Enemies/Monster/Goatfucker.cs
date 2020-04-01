using UnityEngine;

public class Goatfucker : MonoBehaviour, IEnemyEntity {

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
    [HideInInspector] public bool facingRight = false;
    private float moveSpeed = 0.03f;
    private GameObject[] goats = new GameObject[2];
    private int currGoat = 0;

    private void Awake() {
        uid = transform.position.ToString();
        if (GameState.difficulty == Difficulty.VERY_EASY) {
            moveSpeed = 0.02f;
            movementRange = 1.3f;
        }
        var goatResource = Resources.Load<GameObject>(Hazards.GOAT);
        goats[0] = Instantiate(goatResource, transform.position, transform.rotation);
        goats[1] = Instantiate(goatResource, transform.position, transform.rotation);
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
            var goat = GetGoat();
            Vector2 direction = Vector2.left;
            if (transform.position.x < target.transform.position.x) {
                direction = -direction;
                Vector3 theScale = goat.transform.localScale;
                theScale.x *= -1;
                goat.transform.localScale = theScale;
            }
            direction += new Vector2(0f, 0.8f);
            goat.GetComponent<Rigidbody2D>().AddForce(direction * shootingSpeed);
        }
    }

    public bool IsActive() {
        return true;
    }

    private GameObject GetGoat() {
        var goat = goats[currGoat];
        goat.transform.position = transform.position;
        goat.SetActive(true);
        currGoat = currGoat == 1 ? 0 : 1;

        return goat;
    }
}

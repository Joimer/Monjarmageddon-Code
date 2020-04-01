using UnityEngine;

public class Priest : MonoBehaviour, IEnemyEntity {

    private Vector3 initialPosition;
    private bool active = false;
    public GameObject target;
    private float movementRange = 2.5f;
    private string uid;
    private bool isMovingUp = true;
    private float activatingDistance = 3f;

    private void Awake() {
        uid = transform.position.ToString();
        if (GameState.difficulty == Difficulty.VERY_EASY) {
            activatingDistance = 3.5f;
        }
    }

    private void Start() {
        initialPosition = GetComponent<Transform>().position;
        target = ObjectLocator.GetPlayer();

        if (GameState.lastCheckpoint != null && GameState.lastCheckpoint.enemiesKilled.Contains(uid)) {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate() {
        if (!GameState.isGameLocked && target && target.GetComponent<Transform>() != null) {
            var distance = Vector2.Distance(transform.position, target.transform.position);
            if (!active && distance < activatingDistance) {
                Activate();
            }

            Color color = GetComponent<SpriteRenderer>().color;
            if (active) {
                if (isMovingUp) {
                    if (transform.position.y < initialPosition.y + movementRange) {
                        transform.Translate(new Vector2(0f, 0.05f));
                    } else {
                        isMovingUp = !isMovingUp;
                    }
                }

                if (!isMovingUp) {
                    if (transform.position.y > initialPosition.y) {
                        transform.Translate(new Vector2(0f, -0.05f));
                    } else {
                        isMovingUp = !isMovingUp;
                        active = false;
                    }
                }
                color.a = 1f;
            } else {
                color.a = 0.3f;
            }
            GetComponent<SpriteRenderer>().color = color;
        }
    }

    private void Update() {
        if (!GameState.isGameLocked && active && transform.position.x < initialPosition.x - 5f) {
            GameState.GetInstance().enemiesKilled.Add(uid);
            Destroy(gameObject);
        }
    }

    public string GetUid() {
        return uid;
    }

    private void Activate() {
        active = true;
        if (isMovingUp) {
            Shoot();
        }
    }

    private void Shoot() {
        if (gameObject.activeSelf) {
            var suspiciousShot = Instantiate(Resources.Load<GameObject>(Hazards.SUSPICIOUS_SHOT), transform.position, transform.rotation);
            suspiciousShot.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 1f) * 200f);
        }
    }

    public bool IsActive() {
        return active;
    }
}

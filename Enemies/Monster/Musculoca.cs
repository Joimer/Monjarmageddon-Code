using UnityEngine;

public class Musculoca : MonoBehaviour, IEnemyEntity {


    private Vector2 initialPosition;
    private bool isMovingLeft = true;
    private float movementRange = 1.5f;
    private string uid;
    private bool jumping = false;
    private bool falling = false;
    private float jumpApex;
    private bool facingRight = false;
    private Animator animator;

    private void Awake() {
        uid = transform.position.ToString();
        animator = GetComponent<Animator>();
    }

    void Start() {
        initialPosition = transform.position;
        jumpApex = initialPosition.y + 1f;

        if (GameState.lastCheckpoint != null && GameState.lastCheckpoint.enemiesKilled.Contains(uid)) {
            Destroy(gameObject);
        }
    }

    void FixedUpdate() {
        if (!GameState.isGameLocked) {
            if (!jumping && !falling) {
                jumping = true;
                animator.Play(Animator.StringToHash(Animations.MUSCULOCA_JUMP));
            }

            var yMovemenent = 0f;
            if (jumping) {
                if (transform.position.y < jumpApex) {
                    yMovemenent = 0.05f;
                } else {
                    jumping = false;
                    falling = true;
                }
            }

            if (falling) {
                if (transform.position.y > initialPosition.y) {
                    yMovemenent = -0.05f;
                } else {
                    falling = false;
                    animator.Play(Animator.StringToHash(Animations.MUSCULOCA_STILL));
                }
            }

            if (isMovingLeft) {
                if (transform.position.x > initialPosition.x - movementRange) {
                    transform.Translate(new Vector2(-0.03f, yMovemenent));
                } else {
                    Turn();
                }
            }

            if (!isMovingLeft) {
                if (transform.position.x < initialPosition.x + movementRange) {
                    transform.Translate(new Vector2(0.03f, yMovemenent));
                } else {
                    Turn();
                }
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

    public bool IsActive() {
        return true;
    }
}

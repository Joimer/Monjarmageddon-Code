using UnityEngine;

public class Slime : MonoBehaviour, IEnemyEntity {

    private Vector3 initialPosition;
    private bool isMovingLeft = true;
    private float movementRange = 1.5f;
    private string uid;
    private float moveSpeed = 0.02f;
    [HideInInspector]
    public bool facingRight = false;

    private void Awake() {
        uid = transform.position.ToString();
        if (GameState.difficulty == Difficulty.VERY_EASY) {
            moveSpeed = 0.01f;
            movementRange = 1.3f;
        }
    }

    void Start() {
        initialPosition = transform.position;

        if (GameState.lastCheckpoint != null && GameState.lastCheckpoint.enemiesKilled.Contains(uid)) {
            Destroy(gameObject);
        }
    }

    void FixedUpdate() {
        if (!GameState.isGameLocked) {
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

    public string GetUid() {
        return uid;
    }

    // TODO: Move out to a flippable component or whatfuckingever
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

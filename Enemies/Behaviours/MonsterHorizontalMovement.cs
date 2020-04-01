using UnityEngine;

public class MonsterHorizontalMovement : MonoBehaviour {

    public float range = 1.5f;
    public float speed = 0.03f;
    private bool isMovingLeft = true;
    private Vector2 initialPosition;
    private Vector2 advance;
    private Vector2 retrocede;

    public void Awake() {
        advance = new Vector2(-speed, 0f);
        retrocede = new Vector2(speed, 0f);
    }

    public void Start() {
        initialPosition = transform.position;
    }

    private void Turn() {
        isMovingLeft = !isMovingLeft;
        var flipper = GetComponent<Flipper>();
        if (flipper != null) {
            flipper.Flip();
        }
    }

    public void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag(Tags.ENEMY_COLLISION)) {
            Turn();
        }
    }

    public void FixedUpdate() {
        if (!GameState.isGameLocked) {
            if (isMovingLeft) {
                if (transform.position.x > initialPosition.x - range) {
                    transform.Translate(advance);
                } else {
                    Turn();
                }
            }

            if (!isMovingLeft) {
                if (transform.position.x < initialPosition.x + range) {
                    transform.Translate(retrocede);
                } else {
                    Turn();
                }
            }
        }
    }
}

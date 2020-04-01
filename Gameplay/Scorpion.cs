using UnityEngine;

public class Scorpion : MonoBehaviour {

    private Vector3 initialPosition;
    private bool isMovingLeft = true;
    private float movementRange = 0.3f;
    private float moveSpeed = 0.01f;
    [HideInInspector]
    public bool facingRight = false;

    void Start() {
        initialPosition = transform.position;
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

    public void Turn() {
        isMovingLeft = !isMovingLeft;
        Flip();
    }

    void Flip() {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            AudioManager.GetInstance().PlayEffect(Sfx.INSECT_STEP, 1f);
            Instantiate(Resources.Load<GameObject>("Prefabs/Props/DeadScorpion"), gameObject.transform.position, gameObject.transform.rotation);
            Destroy(gameObject);
        }
    }
}

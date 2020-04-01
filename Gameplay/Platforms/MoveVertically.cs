using UnityEngine;

public class MoveVertically : MonoBehaviour {

    private Vector3 initialPosition;
    private bool firstMove = true;
    public bool isMovingDown = true;
    public float movementRange = 0.75f;
    public float speed = 0.01f;
    public bool dontRepeat = false;

    public void Start() {
        initialPosition = transform.position;
    }

    public void FixedUpdate() {
        if (!GameState.isGameLocked) {
            if (isMovingDown) {
                if (transform.position.y > initialPosition.y - movementRange) {
                    transform.Translate(new Vector2(0f, -speed));
                } else {
                    isMovingDown = !isMovingDown;
                    if (firstMove) {
                        firstMove = false;
                    }
                }
            } else {
                if (transform.position.y < initialPosition.y + movementRange) {
                    transform.Translate(new Vector2(0f, speed));
                } else {
                    isMovingDown = !isMovingDown;
                    if (firstMove) {
                        firstMove = false;
                    }
                }
            }

            if (!firstMove && dontRepeat) {
                Destroy(gameObject);
            }
        }
    }
}

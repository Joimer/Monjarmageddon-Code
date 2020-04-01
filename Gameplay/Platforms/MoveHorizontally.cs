using UnityEngine;

public class MoveHorizontally : MonoBehaviour {

    private Vector3 initialPosition;
    private bool firstMove = true;
    public bool isMovingLeft = true;
    public float movementRange = 0.75f;
    public float speed = 0.01f;

    void Start() {
        initialPosition = transform.position;
    }

    void FixedUpdate() {
        if (!GameState.isGameLocked) {
            if (isMovingLeft) {
                if (transform.position.x > initialPosition.x - movementRange) {
                    transform.Translate(new Vector2(-speed, 0f));
                } else {
                    isMovingLeft = !isMovingLeft;
                    if (firstMove) {
                        firstMove = false;
                    }
                }
            } else {
                if (transform.position.x < initialPosition.x + movementRange) {
                    transform.Translate(new Vector2(speed, 0f));
                } else {
                    isMovingLeft = !isMovingLeft;
                }
                if (firstMove) {
                    firstMove = false;
                }
            }
        }
    }
}

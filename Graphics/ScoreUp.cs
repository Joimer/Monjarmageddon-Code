using UnityEngine;

public class ScoreUp : MonoBehaviour {

    private Vector3 initialPosition;
    private float movementRange = 1f;
    private float speed = 0.02f;

    void Start() {
        initialPosition = transform.position;
    }

    void FixedUpdate() {
        if (!GameState.isGameLocked) {
            if (transform.position.y < initialPosition.y + movementRange) {
                transform.Translate(new Vector2(0f, speed));
            } else {
                Destroy(gameObject);
            }
        }
    }
}

using UnityEngine;

public class MoveUpwards : MonoBehaviour {

    private Vector3 initialPosition;
    public float movementRange = 10.5f;
    public float speed = 0.07f;
    private bool started = false;
    private float triggerTime;

    void Start() {
        initialPosition = transform.position;
    }

    void FixedUpdate() {
        if (!GameState.isGameLocked) {
            if (started && Time.time - triggerTime > 0.6f && transform.position.y < initialPosition.y + movementRange) {
                transform.Translate(new Vector2(0f, speed));
            }
        }
    }

    public void Touch() {
        if (!started) {
            started = true;
            triggerTime = Time.time;
        }
    }
}

using UnityEngine;

public class AutoScrollHorizontal : MonoBehaviour {

    public float speed = 1f;
    public bool rightToLeft = true;
    private Vector2 lastPosition;
    private Vector2 anchor;

    private void FixedUpdate() {
        // Get the difference of initial position with current
        var movement = speed / GameState.pixelsPerUnit;
        if (rightToLeft) {
            movement = -movement;
        }
        transform.Translate(new Vector2(movement, 0f));

        if (rightToLeft && transform.localPosition.x < -640f) {
            transform.localPosition = new Vector2(640f, transform.localPosition.y);
        }

        if (!rightToLeft && transform.localPosition.x > 640f) {
            transform.localPosition = new Vector2(-640f, transform.localPosition.y);
        }
    }
}

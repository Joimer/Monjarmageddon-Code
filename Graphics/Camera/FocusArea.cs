using UnityEngine;

public class FocusArea : MonoBehaviour {

    [HideInInspector]
    public Vector2 centre;
    private Vector2 size = new Vector2(1, 2);
    [HideInInspector]
    public Vector2 velocity;
    private float left, right;
    private float top, bottom;
    private Bounds targetBounds;
    private GameObject player;

    private void OnDrawGizmos() {
        Gizmos.color = new Color(1, 0, 0, .5f);
        Gizmos.DrawCube(centre, size);
    }

    private void Start() {
        player = ObjectLocator.GetPlayer();
        targetBounds = player.GetComponent<BoxCollider2D>().bounds;
        left = targetBounds.center.x - size.x / 2;
        right = targetBounds.center.x + size.x / 2;
        bottom = targetBounds.min.y;
        top = targetBounds.min.y + size.y;
        velocity = Vector2.zero;
        centre = new Vector2((left + right) / 2, (top + bottom) / 2);
    }

    private void Update() {
        if (player != null) {
            targetBounds = player.GetComponent<BoxCollider2D>().bounds;
            var shiftX = 0f;
            if (targetBounds.min.x < left) {
                shiftX = targetBounds.min.x - left;
            } else if (targetBounds.max.x > right) {
                shiftX = targetBounds.max.x - right;
            }
            left += shiftX;
            right += shiftX;

            var shiftY = 0f;
            if (targetBounds.min.y < bottom) {
                shiftY = targetBounds.min.y - bottom;
            } else if (targetBounds.max.y > top) {
                shiftY = targetBounds.max.y - top;
            }
            top += shiftY;
            bottom += shiftY;
            centre = new Vector2((left + right) / 2, (top + bottom) / 2);
            velocity = new Vector2(shiftX, shiftY);
        }
    }
}

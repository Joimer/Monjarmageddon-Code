using UnityEngine;

public class Sunset : MonoBehaviour {

    private float speed = 13f;
    private float lastX;
    private GameObject reference;

    private void Start() {
        reference = ObjectLocator.GetCamera();
        lastX = reference.transform.position.x;
    }

    private void FixedUpdate() {
        var isCameraLocked = GameState.GetInstance().isCameraLocked;
        var xDifference = isCameraLocked ? 0f : -(reference.transform.position.x - lastX) / speed;
        transform.Translate(new Vector2(xDifference, 0f));
        lastX = reference.transform.position.x;
    }
}

using UnityEngine;

public class FallDown : MonoBehaviour {

    private bool touched = false;
    private float triggerTime;
    private float fallingTime = 0.01f;

    void FixedUpdate() {
        if (!GameState.isGameLocked) {
            if (touched && Time.time - triggerTime > 0.3f) {
                transform.Translate(new Vector2(0f, -0.05f - fallingTime / 10));
                fallingTime += Time.deltaTime;
                if (!GetComponent<Renderer>().isVisible) {
                    gameObject.SetActive(false);
                }
            }
        }
    }

    public void Touch() {
        if (!touched) {
            touched = true;
            triggerTime = Time.time;
        }
    }
}

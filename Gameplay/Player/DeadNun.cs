using UnityEngine;

public class DeadNun : MonoBehaviour {

    // The distance the dead nun sprite will travel before the level is reloaded.
    public float upwardsDistance = 4.2f;
    private float initialY;
    private bool activated = false;
    private Vector2 movement = new Vector2(0f, 0.06f);

    private void Start() {
        initialY = transform.position.y;
    }

    public void Activate() {
        activated = true;
    }

    private void FixedUpdate() {
        if (!GameState.isGameLocked && activated) {
            transform.Translate(movement);
            if (transform.position.y - initialY >= upwardsDistance) {
                FinishDyingAnimation();
            }
        }
	}

    private void FinishDyingAnimation() {
        if (activated) {
            activated = false;
            GameState.GetInstance().DeathFinished();
            gameObject.SetActive(false);
        }
    }
}

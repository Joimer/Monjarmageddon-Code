using UnityEngine;

public class BloodSplatter : MonoBehaviour {

    private float started;
    private bool halved = false;
    private float offset;
    private GameObject player;

	private void Start () {
        started = Time.time;
        player = ObjectLocator.GetPlayer();
	}

    private void Update() {
        if (Time.time > started + 5f) {
            Destroy(gameObject);
        }

        if (player != null) {
            var movemenet = player.GetComponent<PlatformerMovement2D>();
            if (movemenet.IsCrouching() && !halved) {
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y / 2, transform.localScale.z);
                halved = true;
            } else if (halved && !movemenet.IsCrouching()) {
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * 2, transform.localScale.z);
                halved = false;
            }
        }
	}
}

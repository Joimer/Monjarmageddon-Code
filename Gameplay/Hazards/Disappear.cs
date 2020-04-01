using UnityEngine;

public class Disappear : MonoBehaviour {

    public Direction direction = Direction.LEFT;
    private float activateTime = 0f;
    private bool activated = false;

	private void Update () {
		if (activated && Time.time - activateTime > 0.25f) {
            transform.localScale = new Vector3(transform.localScale.x - transform.localScale.x / 10f, transform.localScale.y, transform.localScale.z);
        }
	}

    public void OnTriggerEnter2D(Collider2D collision) {
        if (collision.GetComponent<FeetCollider>() != null) {
            activated = true;
            activateTime = Time.time;
        }
    }

    public void OnTriggerExit2D(Collider2D collision) {
        if (collision.GetComponent<FeetCollider>() != null) {
            activated = false;
            transform.localScale = new Vector3(1, 1, transform.localScale.z);
        }
    }
}

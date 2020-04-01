using UnityEngine;

public class Globe : MonoBehaviour {

    private Vector2 initialPos;
    private bool goingUp = true;
    private float verticalMovement = 1.5f;
    private int bounces = 0;

    // Use this for initialization
    private void Start() {
        initialPos = transform.position;
    }
	
	// Update is called once per frame
	private void FixedUpdate() {
		if (goingUp) {
            if (transform.position.y < initialPos.y + verticalMovement) {
                transform.Translate(new Vector2(0f, 0.05f));
            } else {
                goingUp = false;
                bounces++;
            }
        }

        if (bounces == 4) {
            ShootCircle();
        }

        if (!goingUp && transform.position.y > initialPos.y - 1f) {
            transform.Translate(new Vector2(-0.025f, -0.05f));
        } else {
            goingUp = true;
        }
	}

    public void ShootCircle() {
        GetComponent<FireCircleShot>().Shoot();
        // Object deactivates when it's finished.
        bounces = 0;
        gameObject.SetActive(false);
    }
}

using UnityEngine;

public class LabShooter : MonoBehaviour {

    private float fireRate = 0.5f;
    private float lastShot = 0f;
    private bool activating = false;
    private float activateTime = 0f;
    private bool active = false;
    private GameObject target;
    private float shootDelay = 0.25f;
    public float activeDistance = 4f;
    public Direction direction;
    private float shootSpeed = 200f;
    public bool fast = true;

    private void Start() {
        target = ObjectLocator.GetPlayer();
        if (!fast) {
            shootDelay = 0.66f;
            fireRate = 0.9f;
        }
    }

    private void FixedUpdate() {
        if (!GameState.isGameLocked && target && target.GetComponent<Transform>() != null) {
            var distance = Vector2.Distance(transform.position, target.transform.position);

            if (active && distance > activeDistance) {
                Deactive();
            }

            if (!active && !activating && distance < activeDistance) {
                Activate();
            }

            if (activating && Time.time > activateTime + shootDelay) {
                activating = false;
                active = true;
            }

            if (active && lastShot + fireRate < Time.time) {
                Shoot();
            }
        }
    }

    private void Activate() {
        activating = true;
        activateTime = Time.time;
    }

    private void Deactive() {
        activating = false;
        activateTime = 0f;
        active = false;
    }

    private void Shoot() {
        if (Vector2.Distance(transform.position, target.transform.position) <= activeDistance) {
            lastShot = Time.time;
            var yOffset = 0f;
            var xOffset = 0f;
            var force = Vector2.up;
            if (direction == Direction.UP) {
                yOffset = 0.1f;
            }
            if (direction == Direction.DOWN) {
                force = Vector2.down;
                yOffset = -0.1f;
            }
            if (direction == Direction.LEFT) {
                force = Vector2.left;
                xOffset = -0.15f;
            }
            if (direction == Direction.RIGHT) {
                force = Vector2.right;
                xOffset = 0.15f;
            }
            var fireBall = Instantiate(Resources.Load<GameObject>(Hazards.BLUE_RAY_SHOT), transform.position + new Vector3(xOffset, yOffset, 0f), transform.rotation);
            fireBall.GetComponent<Rigidbody2D>().AddForce(force * shootSpeed);
        }
    }
}

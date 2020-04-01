using UnityEngine;

public class JosyfFire : MonoBehaviour {

    private bool rotating = true;
    private bool moving = false;
    private float xMove = 0.05f;
    private float rotateSpeed = 1.5f;
    private float radius = 1.2f;
    private float angle;
    [HideInInspector]
    public Vector2 centre;
    private Vector2 objective;
    private JosyfPotzedong josyf;
    private Vector2 initialPos;

    private void Start() {
        centre = transform.parent.transform.position;
        initialPos = transform.position;
    }

    public void SetJosyf(GameObject josefo) {
        josyf = josefo.GetComponent<JosyfPotzedong>();
    }

    private void FixedUpdate() {
        if (rotating) {
            angle += rotateSpeed * Time.deltaTime;
            var offset = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)) * radius;
            transform.position = centre + offset;
        } else {
            if (!moving) {
                xMove = objective.x > transform.position.x ? 0.05f : -0.05f;
                moving = true;
            }
            if (moving) {
                transform.Translate(new Vector2(xMove, -0.035f));
            }
        }
    }

    public void ShootTowards(Vector2 posTo) {
        objective = posTo;
        rotating = false;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag(Tags.GROUND_COLLISION)) {
            josyf.FireHasBeenHit(transform.position);
            rotating = true;
            moving = false;
            gameObject.SetActive(false);
            transform.position = initialPos;
        }
    }
}

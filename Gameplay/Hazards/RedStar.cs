using UnityEngine;

public class RedStar : MonoBehaviour {

    private Vector2 direction;

    private void Start() {
        direction = Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.layer == 0) {
            direction = Vector2.zero;
            gameObject.SetActive(false);
        }
    }

    private void FixedUpdate() {
        if (direction != Vector2.zero) {
            transform.Translate(direction * 0.05f);
        }
    }

    public void ShootTowards(Vector2 pos) {
        direction = pos - (Vector2)transform.position;
    }
}

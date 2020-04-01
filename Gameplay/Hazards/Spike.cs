using UnityEngine;

public class Spike : MonoBehaviour {

    private bool moving = false;
    private Vector2 move = new Vector2(0f, -0.1f);

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.layer == 0) {
            GetComponent<FireCircleShot>().Shoot(new Vector2(transform.position.x, transform.position.y + 0.3f));
            moving = false;
            gameObject.SetActive(false);
        }
    }

    private void FixedUpdate() {
        if (moving) {
            transform.Translate(move);
        }
    }

    public void Shoot() {
        moving = true;
    }
}

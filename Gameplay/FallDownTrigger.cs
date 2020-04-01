using UnityEngine;

public class FallDownTrigger : MonoBehaviour {

    public GameObject toThrowDown;

    public void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player") && toThrowDown != null) {
            toThrowDown.AddComponent<Rigidbody2D>();
            var rb2d = toThrowDown.GetComponent<Rigidbody2D>();
            rb2d.freezeRotation = true;
            Destroy(gameObject);
        }
    }
}

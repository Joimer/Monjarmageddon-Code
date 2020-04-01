using UnityEngine;

public class FeetCollider : MonoBehaviour {

	private PlatformerMovement2D playerMovement;

	private void Start() {
        var player = ObjectLocator.GetPlayer();
        if (player != null) {
            playerMovement = player.GetComponent<PlatformerMovement2D>();
        }
    }

    private void Update() {
        if (transform.localPosition.x != 0 || transform.localPosition.y != 0) {
            transform.localPosition = Vector2.zero;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (playerMovement != null) {
            playerMovement.FeetCollided(collision.gameObject);
        }
    }
}

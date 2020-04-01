using UnityEngine;

public class DamageReceiver : MonoBehaviour {

    private GameObject player;
    public float crouchHitboxOffset = 0.1f;

    private void Start() {
        player = ObjectLocator.GetPlayer();
    }

    private void Update() {
        if (transform.localPosition.x != 0 || transform.localPosition.y != 0) {
            transform.localPosition = Vector2.zero;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (player != null) {
            var move = player.GetComponent<PlatformerMovement2D>();
            if (move != null) {
                var overlaps = collision.gameObject;
                var playerHitReceiver = overlaps.GetComponent<PlayerHitReceiver>();
                var anEnemyBeingHit = playerHitReceiver != null && playerHitReceiver.IsBeingHit();
                move.overlapping = overlaps;

                // The hitbox triggers a hit when overlapping against a hostile object.
                // This object is stored, so if nun keeps overlapping after the collision is finished, the hit is repeated.
                if (overlaps.CompareTag(Tags.ENEMY_HIT) && !move.IsInvulnerable() && !anEnemyBeingHit) {
                    move.EnemyHit();
                }
            }
        }
    }
}

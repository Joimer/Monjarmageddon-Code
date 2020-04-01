using UnityEngine;

public class EnemyHitReceiver : MonoBehaviour {

    private Invulnerability invuln;
    private PlatformerMovement2D movement;

    public void Start() {
        invuln = GetComponent<Invulnerability>();
        movement = GetComponent<PlatformerMovement2D>();
    }

    public void ReceiveHit() {
        if (invuln.IsInvulnerable() || !movement.CanMove()) {
            return;
        }

        AudioManager.GetInstance().PlayEffect(Sfx.NUN_DAMAGED, 1f);
        GameState.holyWaters -= GameplayValues.watersPerHit;
        if (GameState.holyWaters > -1) {
            invuln.SetInvulnerable(GameplayValues.GetInvulnerableTime());
        }
    }
}

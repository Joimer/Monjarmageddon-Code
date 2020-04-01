using UnityEngine;

public class Footprint : MonoBehaviour {

    private bool keep = false;
    
    public void Update() {
        if (!keep) {
            gameObject.SetActive(false);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision) {
        if (gameObject.activeSelf && !collision.CompareTag(Tags.PLAYER) && !collision.CompareTag(Tags.PLAYER_HITBOX)) {
            keep = true;
        }
    }
}

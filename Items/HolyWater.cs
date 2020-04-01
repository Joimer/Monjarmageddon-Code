using UnityEngine;

public class HolyWater : MonoBehaviour {

    private string uid;

    private void Awake() {
        uid = transform.position.ToString();
    }

    void Start() {
        if (GameState.lastCheckpoint != null && GameState.lastCheckpoint.objectsUsed.Contains(uid)) {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.GetComponent<PlatformerMovement2D>() != null) {
            AudioManager.GetInstance().PlayEffect(Sfx.PICK_ITEM);
            Destroy(gameObject);
            GameState.holyWaters += 1;
            GameState.GetInstance().objectsUsed.Add(uid);
        }
    }
}

using UnityEngine;

public class BabyStealing : MonoBehaviour {

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
            AudioManager.GetInstance().PlayEffect(Sfx.BABY_PICKUP);
            Destroy(gameObject);
            GameState.lives += 1;
            GameState.score += 300;
            GameState.GetInstance().objectsUsed.Add(uid);
        }
    }
}

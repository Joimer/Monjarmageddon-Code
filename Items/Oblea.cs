using UnityEngine;

public class Oblea : MonoBehaviour {

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
            AudioManager.GetInstance().PlayEffect(Sfx.PICK_WAFER);
            Destroy(gameObject);
            var gs = GameState.GetInstance();
            gs.obleas++;
            GameState.score += 5 * (int) GameState.difficulty;
            if (gs.obleas % GameplayValues.GetObleasForHolyWater() == 0) {
                GameState.holyWaters += 1;
            }
            GameState.GetInstance().objectsUsed.Add(uid);
        }
    }
}

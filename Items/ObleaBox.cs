using UnityEngine;

public class ObleaBox : MonoBehaviour {

    private string uid;

    private void Awake() {
        uid = transform.position.ToString();
    }

    private void Start() {
        if (GameState.lastCheckpoint != null && GameState.lastCheckpoint.objectsUsed.Contains(uid)) {
            Destroy(gameObject);
        }
    }

    public int boxSize = 10;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.GetComponent<PlatformerMovement2D>() != null) {
            AudioManager.GetInstance().PlayEffect(Sfx.PICK_ITEM);
            Destroy(gameObject);
            var gs = GameState.GetInstance();
            for (var i = 0; i < boxSize; i++) {
                gs.obleas++;
                GameState.score += 5 * (int)GameState.difficulty;
                if (gs.obleas % GameplayValues.GetObleasForHolyWater() == 0) {
                    GameState.holyWaters += 1;
                }
            }
            gs.objectsUsed.Add(uid);
        }
    }
}

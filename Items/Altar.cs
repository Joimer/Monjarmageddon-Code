using UnityEngine;

public class Altar : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.GetComponent<PlatformerMovement2D>() != null) {
            var gs = GameState.GetInstance();
            gs.isCameraLocked = false;
            var blackBg = GameObject.Find("black_bg");
            if (blackBg != null) {
                blackBg.AddComponent<EndAct>();
            }
        }
    }
}

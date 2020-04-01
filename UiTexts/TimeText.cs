using UnityEngine;
using UnityEngine.UI;

public class TimeText : MonoBehaviour {

    Text text;
    GameState gs;

    void Awake() {
        text = GetComponent<Text>();
        gs = GameState.GetInstance();
    }

    void Start() {
        if (GameState.lang == SystemLanguage.Japanese) {
            text.resizeTextForBestFit = false;
            text.fontSize = 24;
        }
    }

    void Update() {
        if (!GameState.isGameLocked && !GameState.activatingBoss) {
            // Should this be here...?
            gs.currentSceneTime += Time.deltaTime;
            string minutes = Mathf.Floor(gs.currentSceneTime / 60).ToString("00");
            string seconds = (gs.currentSceneTime % 60).ToString("00");
            text.text = TextManager.GetText("time") + ": " + minutes + ":" + seconds;
        }
    }
}

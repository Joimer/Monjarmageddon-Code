using UnityEngine;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour {

    Text text;

    void Awake() {
        text = GetComponent<Text>();
    }

    void Start() {
        if (GameState.lang == SystemLanguage.Japanese) {
            text.resizeTextForBestFit = false;
            text.fontSize = 24;
        }
    }

    void Update() {
        text.text = TextManager.GetText("score") + ": " + GameState.score;
    }
}

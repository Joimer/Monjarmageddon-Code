using UnityEngine;
using UnityEngine.UI;

public class WafersText : MonoBehaviour {

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
        text.text = TextManager.GetText("wafers") + ": " + gs.obleas;
    }
}

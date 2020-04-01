using UnityEngine;
using UnityEngine.UI;

public class MenuExitGameText : MonoBehaviour {

    Text text;

    void Awake() {
        text = GetComponent<Text>();
        text.fontSize = GameState.lang == SystemLanguage.Japanese ? UiTexts.MENU_JAPANESE_TEXT_SIZE : UiTexts.MENU_TEXT_SIZE;
    }

    void Update() {
        text.text = TextManager.GetText("menu exit game");
    }
}

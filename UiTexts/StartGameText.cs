using UnityEngine;
using UnityEngine.UI;

public class StartGameText : MonoBehaviour {

    Text text;

    void Awake() {
        text = GetComponent<Text>();
    }

    void Update() {
        text.text = TextManager.GetText("start game");
    }
}

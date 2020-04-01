using UnityEngine;
using UnityEngine.UI;

public class SubtitleText : MonoBehaviour {
    Text text;

    void Awake() {
        text = GetComponent<Text>();
    }

    void Update() {
        text.text = TextManager.GetText("subtitle");
    }
}

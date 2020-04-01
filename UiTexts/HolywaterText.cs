using UnityEngine;
using UnityEngine.UI;

public class HolywaterText : MonoBehaviour {

    Text text;

    void Awake() {
        text = GetComponent<Text>();
    }

    void Update() {
        var hw = GameState.holyWaters;
        // When you're about to die, holy waters could be briefly displayed as -1.
        if (hw < 0) {
            hw = 0;
        }
        text.text = "x " + hw;
    }
}

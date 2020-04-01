using UnityEngine;
using UnityEngine.UI;

public class LivesText : MonoBehaviour {

    Text text;

    void Awake() {
        text = GetComponent<Text>();
    }

    void Update() {
        text.text = "x " + GameState.lives;
    }
}

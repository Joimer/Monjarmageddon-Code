using UnityEngine;
using UnityEngine.UI;

public class PressEnter : MonoBehaviour {

	private void Start() {
        GetComponent<Text>().text = string.Format(TextManager.GetText("press"), InputManager.keyboardConfig[GameCommand.ACCEPT].ToString());
    }
}

using UnityEngine;
using UnityEngine.UI;

public class Intro : MonoBehaviour {

    private GameState gs;
    private InputManager inputManager;
    private RectTransform rt;
    private float startYPos;
    private float speed = 0.008f;
    private float yTreshold;

	private void Start() {
        gs = GameState.GetInstance();
        inputManager = InputManager.GetInstance();
        GetComponent<Text>().text = TextManager.GetText("intro");
        rt = GetComponent<RectTransform>();
        startYPos = rt.position.y;
        yTreshold = (Screen.height + 5 + rt.sizeDelta.y) / 100f;
        // Japanese text is full of kanji and seems slower to be read.
        // So just in case.
        if (GameState.lang == SystemLanguage.Japanese) {
            speed = 0.006f;
        }
    }

    private void Update() {
        // Both game commands and hardcoded keys work to pass the intro so no one has a problem with it.
        if (Input.GetKeyDown(KeyCode.Return)
            || Input.GetKeyDown(KeyCode.Escape)
            || inputManager.IsActionPressed(GameCommand.ACCEPT)
            || inputManager.IsActionPressed(GameCommand.SHOOT)
            || inputManager.IsActionPressed(GameCommand.JUMP)
        ) {
            GoToMenu();
        }
    }

    private void FixedUpdate() {
        if (rt.position.y < startYPos + yTreshold) {
            rt.position = new Vector3(rt.position.x, rt.position.y + speed, rt.position.z);
        } else {
            GoToMenu();
        }
    }

    private void GoToMenu() {
        gs.LoadScene(Scenes.MAIN_MENU);
    }
}

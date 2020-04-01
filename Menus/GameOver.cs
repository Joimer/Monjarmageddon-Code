using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour {

    private float start = 0f;

    private void Awake() {
        GameState.GetInstance();
        start = Time.time;
    }

    private IEnumerator Start() {
        yield return new WaitForSeconds(23.3f);
        Restart();
    }

    private void Update() {
        var inputManager = InputManager.GetInstance();
        if (Time.time - start > 0.5f && (Input.GetKeyDown(KeyCode.Return)
            || Input.GetKeyDown(KeyCode.Escape)
            || inputManager.IsActionPressed(GameCommand.ACCEPT)
            || inputManager.IsActionPressed(GameCommand.SHOOT)
            || inputManager.IsActionPressed(GameCommand.JUMP)
        )) {
            Restart();
        }
    }

    private void Restart() {
        var gs = GameState.GetInstance();
        gs.LoadScene(Scenes.SPLASH_SCREEN);
    }
}

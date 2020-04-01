using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SplashScreen : MonoBehaviour {

    private SpriteRenderer spriteRenderer;
    private InputManager inputManager;
    private GameState gameState;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        inputManager = InputManager.GetInstance();
        gameState = GameState.GetInstance();
    }

    private IEnumerator Start() {
        gameState.CheckGamePadUpdate();
        yield return new WaitForSeconds(4f);
        NextScene();
    }

    private void Update() {
        var color = spriteRenderer.color;
        color.a -= 0.005f;
        spriteRenderer.color = color;
        if (Input.GetKeyDown(KeyCode.Return)
            || Input.GetKeyDown(KeyCode.Escape)
            || inputManager.IsActionPressed(GameCommand.ACCEPT)
            || inputManager.IsActionPressed(GameCommand.SHOOT)
            || inputManager.IsActionPressed(GameCommand.JUMP)
        ) {
            NextScene();
        }
    }

    private void NextScene() {
        gameState.LoadScene(Scenes.INTRO);
    }
}

using UnityEngine;
using Steamworks;

public class GameMenu : MonoBehaviour {

    [HideInInspector]
    public static bool menuActive = false;

    private GameObject menu;
    private GameObject indexIcon;
    private int optionIndex = 1;
    private Vector2 initialIconposition;
    private AudioManager audioManager;
    private Callback<GameOverlayActivated_t> gameOverlayActivated;

    private void Awake() {
        audioManager = AudioManager.GetInstance();
        Instantiate(Resources.Load<GameObject>("Prefabs/Menus/ActEnd"), transform);
        menu = Instantiate(Resources.Load<GameObject>("Prefabs/Menus/GameMenu"), transform);
        // TODO: Refactor.
        indexIcon = menu.transform.GetChild(4).gameObject;
        if (indexIcon != null) {
            initialIconposition = indexIcon.transform.localPosition;
        }
        menu.SetActive(false);
    }

    private void OnEnable() {
        if (GameState.withSteam && SteamManager.Initialized && gameOverlayActivated == null) {
            gameOverlayActivated = Callback<GameOverlayActivated_t>.Create(OnGameOverlayActivated);
        }
    }

    private void OnGameOverlayActivated(GameOverlayActivated_t pCallback) {
        ActivateMenu();
    }

    private void ActivateMenu() {
        menuActive = true;
        GameState.isGameLocked = true;
        menu.SetActive(true);
        Time.timeScale = 0;
    }

    private void Update() {
        var gs = GameState.GetInstance();
        var inputManager = InputManager.GetInstance();

        // Activate and deactivate menu.
        // This is here because if it was in the latter block, menu would be activated and deactivated as soon as it appears.
        if (menuActive && GameState.menuAvailable && Input.GetKeyDown(KeyCode.Escape)) {
            DeactivateMenu();
            return;
        }

        if (!menuActive && GameState.menuAvailable && (Input.GetKeyDown(KeyCode.Escape) || inputManager.IsActionPressedOnce(GameCommand.ACCEPT))) {
            ActivateMenu();
            // Manage option changes in the next frame at the earliest.
            return;
        }

        // Control the index to navigate the options.
        if (menuActive) {
            if (inputManager.IsMenuPressUp()) {
                audioManager.PlayEffect(Sfx.MENU_BEEP);
                optionIndex--;
            }

            if (inputManager.IsMenuPressDown()) {
                audioManager.PlayEffect(Sfx.MENU_BEEP);
                optionIndex++;
            }

            if (optionIndex == 0) {
                optionIndex = 3;
            }

            if (optionIndex == 4) {
                optionIndex = 1;
            }

            // Show the icon in the proper place.
            if (optionIndex == 1) {
                indexIcon.transform.localPosition = new Vector2(initialIconposition.x, initialIconposition.y);
            }
            if (optionIndex == 2) {
                indexIcon.transform.localPosition = new Vector2(initialIconposition.x, initialIconposition.y - 30f);
            }
            if (optionIndex == 3) {
                indexIcon.transform.localPosition = new Vector2(initialIconposition.x, initialIconposition.y - 60f);
            }

            // Using the options.
            if (inputManager.IsActionPressedOnce(GameCommand.JUMP) || inputManager.IsActionPressedOnce(GameCommand.ACCEPT)) {
                audioManager.PlayEffect(Sfx.MENU_ACCEPT);

                if (optionIndex == 1) {
                    DeactivateMenu();
                }
                if (optionIndex == 2) {
                    DeactivateMenu();
                    gs.SetGameReadyValues();
                    gs.LoadScene(Scenes.MAIN_MENU);
                }
                if (optionIndex == 3) {
                    DeactivateMenu();
                    Application.Quit();
                }
            }
        }

        if (GameState.withSteam && SteamManager.Initialized) {
            SteamAPI.RunCallbacks();
        }
    }

    private void DeactivateMenu() {
        menuActive = false;
        GameState.isGameLocked = false;
        menu.SetActive(false);
        optionIndex = 1;
        Time.timeScale = 1;
    }
}

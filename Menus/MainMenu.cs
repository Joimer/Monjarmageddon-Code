using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using Steamworks;

public class MainMenu : MonoBehaviour {

    private GameState gs;
    private Texture2D icon;
    // Static so it's in the same position when we come back from options.
    private static int optionIndex = 0;
    private bool switchingScene = false;
    private bool continueAllowed = false;
    private SaveState savedState;
    private InputManager inputManager;
    private SpriteRenderer spriteRenderer;
    private AudioManager audioManager;

    // Menu texts
    public Text tutorial;
    private Text startGame;
    private Text continueGame;
    private Text options;
    private Text exitGame;
    private RectTransform nunIcon;
    private Dictionary<int, Vector2> iconPositions;
    public GameObject japaneseSubTitle;

    // Choose level cheat
    private int chooseLevelPos = 0;
    private bool activedLevelSelect = false;

    // Debug mode activation
    private int debugModePos = 0;

    // Choose difficulty (new game option)
    private bool choosingDifficulty = false;
    private bool choosingForNewGame = true;
    public RectTransform difficultyNunIcon;
    public GameObject chooseDifficultyMenu;
    public Text difficultyText;
    private int difficultyIndex = 2;
    private Vector2[] difficultyNunIconPositions = new Vector2[] {
        new Vector2(-109, 50),
        new Vector2(-109, 25),
        new Vector2(-109, 0),
        new Vector2(-109, -25),
        new Vector2(-109, -50)
    };

    public void Awake() {
        gs = GameState.GetInstance();
        audioManager = AudioManager.GetInstance();
        inputManager = InputManager.GetInstance();
        chooseDifficultyMenu.SetActive(false);

        // TODO: Pass in editor by reference
        startGame = GameObject.Find("start_game").GetComponent<Text>();
        continueGame = GameObject.Find("continue_game").GetComponent<Text>();
        options = GameObject.Find("options").GetComponent<Text>();
        exitGame = GameObject.Find("exit_game").GetComponent<Text>();
        nunIcon = GameObject.Find("nun_icon").GetComponent<RectTransform>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        var initialY = nunIcon.anchoredPosition.y;
        iconPositions = new Dictionary<int, Vector2>() {
            { -1, new Vector2(nunIcon.anchoredPosition.x, initialY + 30) },
            { 0, new Vector2(nunIcon.anchoredPosition.x, initialY) },
            { 1, new Vector2(nunIcon.anchoredPosition.x, initialY - 30) },
            { 2, new Vector2(nunIcon.anchoredPosition.x, initialY - 60) },
            { 3, new Vector2(nunIcon.anchoredPosition.x, initialY - 90) }
        };
    }

    public void Start() {
        // Get menutexts.
        tutorial.text = TextManager.GetText("tutorial");
        startGame.text = TextManager.GetText("start game");
        continueGame.text = TextManager.GetText("continue game");
        options.text = TextManager.GetText("options");
        exitGame.text = TextManager.GetText("exit game");
        difficultyText.text = TextManager.GetText("veryeasy") + "\n"
            + TextManager.GetText("easy") + "\n"
            + TextManager.GetText("medium") + "\n"
            + TextManager.GetText("hard") + "\n"
            + TextManager.GetText("extreme");

        // Check if there's a saved game.
        savedState = SaveManager.LoadGameData();
        continueAllowed = savedState.saved;
        if (!continueAllowed) {
            continueGame.color = Color.gray;
        }

        if (japaneseSubTitle != null && GameState.lang != SystemLanguage.Japanese) {
            japaneseSubTitle.SetActive(false);
        }
    }

    public void Update() {
        // Pressing up, down, left, right, and jump + enter leads you to the select level screen.
        CheckLevelSelect();
        CheckDebug();

        // Choosing a difficulty level for a new game.
        if (choosingDifficulty && choosingForNewGame && inputManager.IsMenuStart() && !switchingScene && (!activedLevelSelect || inputManager.IsActionPressed(GameCommand.JUMP))) {
            switchingScene = true;
            GameState.difficulty = (Difficulty)difficultyIndex;
            audioManager.PlayEffect(Sfx.MENU_ACCEPT);
            gs.LoadScene(Scenes.MONASTERY_ACT_1, 0.5f);
        }

        // Choosing difficulty setting to continue the game.
        // Changing it overwrites the waters you get.
        if (choosingDifficulty && inputManager.IsMenuStart() && !switchingScene && (!activedLevelSelect || inputManager.IsActionPressed(GameCommand.JUMP))) {
            switchingScene = true;
            // Adapt holy waters to new difficulty.
            if (difficultyIndex != (int) savedState.difficulty) {
                var level = savedState.level == (Scenes.CREDITS) ? Scenes.FINAL_ZONE : savedState.level;
                savedState.level = level;
                savedState.holyWaters = GameplayValues.GetHolyWatersByLevel(level);
                savedState.difficulty = (Difficulty)difficultyIndex;
            }
            audioManager.PlayEffect(Sfx.MENU_ACCEPT);
            gs.LoadGame(savedState);
        }

        // Choosing a main option.
        if (!choosingDifficulty && inputManager.IsMenuStart() && !switchingScene && (!activedLevelSelect || inputManager.IsActionPressed(GameCommand.JUMP))) {
            audioManager.PlayEffect(Sfx.MENU_ACCEPT);

            if (optionIndex == 1) {
                difficultyIndex = (int) savedState.difficulty;
                chooseDifficultyMenu.SetActive(true);
                UpdateNunDifficultyIcon();
                choosingDifficulty = true;
                choosingForNewGame = false;
            } else if (optionIndex == 2) {
                switchingScene = true;
                gs.LoadScene(Scenes.OPTIONS, 0.5f);
            } else if (optionIndex == 3) {
                Application.Quit();
            } else if (optionIndex == -1) {
                switchingScene = true;
                gs.LoadScene(Scenes.TUTORIAL);
            } else {
                difficultyIndex = 2;
                chooseDifficultyMenu.SetActive(true);
                UpdateNunDifficultyIcon();
                choosingDifficulty = true;
                choosingForNewGame = true;
            }
        }

        if (choosingDifficulty && (Input.GetKeyDown(KeyCode.Escape) || inputManager.IsActionPressed(GameCommand.SHOOT))) {
            chooseDifficultyMenu.SetActive(false);
            choosingDifficulty = false;
        }

        if (!choosingDifficulty) {
            CheckMenuNavigation();
        } else {
            CheckDifficultyNavegation();
        }

        // Fade out.
        if (switchingScene) {
            var color = spriteRenderer.color;
            color.a += 0.03f;
            spriteRenderer.color = color;
        }

        // Nun Icon position.
        nunIcon.anchoredPosition = iconPositions[optionIndex];
    }

    private void CheckMenuNavigation() {
        if (inputManager.IsMenuPressDown() && !switchingScene) {
            audioManager.PlayEffect(Sfx.MENU_BEEP);
            optionIndex++;

            if (optionIndex == 1 && !continueAllowed) {
                optionIndex = 2;
            }

            if (optionIndex == 4) {
                optionIndex = 0;
            }

            UpdateNunIcon();
        }

        if (inputManager.IsMenuPressUp() && !switchingScene) {
            audioManager.PlayEffect(Sfx.MENU_BEEP);
            optionIndex--;

            if (optionIndex == 1 && !continueAllowed) {
                optionIndex = 0;
            }

            if (optionIndex == -1) {
                optionIndex = 3;
            }

            UpdateNunIcon();
        }
    }

    private void CheckDifficultyNavegation() {
        if (inputManager.IsMenuPressDown() && !switchingScene) {
            audioManager.PlayEffect(Sfx.MENU_BEEP);
            difficultyIndex++;

            if (difficultyIndex == 5) {
                difficultyIndex = 0;
            }

            UpdateNunDifficultyIcon();
        }

        if (inputManager.IsMenuPressUp() && !switchingScene) {
            audioManager.PlayEffect(Sfx.MENU_BEEP);
            difficultyIndex--;

            if (difficultyIndex == -1) {
                difficultyIndex = 4;
            }

            UpdateNunDifficultyIcon();
        }
    }

    private void UpdateNunIcon() {
        nunIcon.anchoredPosition = iconPositions[optionIndex];
    }

    private void UpdateNunDifficultyIcon() {
        difficultyNunIcon.anchoredPosition = difficultyNunIconPositions[difficultyIndex];
    }

    // Method to manage the state of the level select key entrance.
    private void CheckLevelSelect() {
        if (inputManager.IsActionPressed(GameCommand.SHOOT) && chooseLevelPos == 4) {
            if (inputManager.IsMenuStart()) {
                AchievementManager.UnlockAchievement(Achievements.SELECT_LEVEL_FOUND);
                switchingScene = true;
                chooseLevelPos = 5;
                gs.LoadScene(Scenes.LEVEL_SELECT, 0.5f);
            } else if (!activedLevelSelect && inputManager.AnyActionPressedOnce(new List<GameCommand>() { GameCommand.JUMP, GameCommand.SHOOT, GameCommand.ACCEPT })) {
                chooseLevelPos = 0;
            }
        }

        if (chooseLevelPos == 3) {
            if (inputManager.IsMenuPressRight()) {
                chooseLevelPos = 4;
                audioManager.PlayEffect(Sfx.PICK_WAFER);
            } else if (!activedLevelSelect && inputManager.AnyActionPressedOnce(new List<GameCommand>() { GameCommand.JUMP, GameCommand.RIGHT })) {
                chooseLevelPos = 0;
            }
        }

        if (chooseLevelPos == 2) {
            if (inputManager.IsMenuPressLeft()) {
                chooseLevelPos = 3;
            } else if (!activedLevelSelect && inputManager.AnyActionPressedOnce(new List<GameCommand>() { GameCommand.JUMP, GameCommand.LEFT })) {
                chooseLevelPos = 0;
            }
        }

        if (chooseLevelPos == 1) {
            if (inputManager.IsActionPressedOnce(GameCommand.DOWN)) {
                chooseLevelPos = 2;
            } else if (!activedLevelSelect && inputManager.AnyActionPressedOnce(new List<GameCommand>() { GameCommand.JUMP, GameCommand.DOWN })) {
                chooseLevelPos = 0;
            }
        }

        if (chooseLevelPos == 0 && inputManager.IsActionPressedOnce(GameCommand.UP)) {
            chooseLevelPos = 1;
        }
    }

    private void CheckDebug() {
        CheckDebugModeActivation();
        if (GameState.debugActive && Input.GetKeyDown(KeyCode.L)) {
            GameState.GetInstance().achieved = new List<Achievements>();
            foreach (Achievements achievement in (Achievements[])Enum.GetValues(typeof(Achievements))) {
                SteamUserStats.ClearAchievement(achievement.ToString());
            }
        }
    }

    private void CheckDebugModeActivation() {
        return;
    }
}

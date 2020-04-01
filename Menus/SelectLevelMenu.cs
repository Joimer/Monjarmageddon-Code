using UnityEngine;
using System.Collections.Generic;

public class SelectLevelMenu : MonoBehaviour {

    private GameState gs;
    private int optionIndex = 0;
    private bool switchingScene = false;
    private RectTransform nunIcon;
    private InputManager inputManager;
    private AudioManager audioManager;

    private void Awake() {
        gs = GameState.GetInstance();
        nunIcon = GameObject.Find("nun_icon").GetComponent<RectTransform>();
        UpdateNunIconPosition();
        inputManager = InputManager.GetInstance();
        audioManager = AudioManager.GetInstance();
    }

    void Update() {
        // Going up and down in levels.
        if (inputManager.IsActionPressedOnce(GameCommand.UP) && !switchingScene) {
            audioManager.PlayEffect(Sfx.MENU_BEEP);
            optionIndex--;
            if (optionIndex == -1) {
                optionIndex = 12;
            }
            UpdateNunIconPosition();
        }

        if (inputManager.IsActionPressedOnce(GameCommand.DOWN) && !switchingScene) {
            audioManager.PlayEffect(Sfx.MENU_BEEP);
            optionIndex++;
            if (optionIndex == 13) {
                optionIndex = 0;
            }
            UpdateNunIconPosition();
        }

        // Loading a level.
        if (inputManager.IsActionPressed(GameCommand.ACCEPT) && !switchingScene) {
            audioManager.PlayEffect(Sfx.MENU_ACCEPT);
            switchingScene = true;
            gs.SetGameReadyValues();
            SetWaters();
            GameState.difficulty = Difficulty.MEDIUM;
            // Get scene from index.
            gs.LoadScene(GetSceneToLoad(), 0.5f);
        }

        // Fade out.
        if (switchingScene) {
            var color = GetComponent<SpriteRenderer>().color;
            color.a += 0.03f;
            GetComponent<SpriteRenderer>().color = color;
        }
    }

    private void UpdateNunIconPosition() {
        float y = 226.88f - optionIndex * 37f;
        float x = 198.4f;
        if (optionIndex % 2 == 0) {
            var positions = new Dictionary<int, float>() {
                { 0, -23f },
                { 2, -4.8f },
                { 4, 12.2f },
                { 6, 26.6f },
                { 8, 73.1f },
                { 10, -57f },
                { 12, 131.9f }
            };
            x = positions[optionIndex];
        }
        nunIcon.anchoredPosition = new Vector2(x, y);
    }

    private string GetSceneToLoad() {
        var scenes = new Dictionary<int, string>() {
                { 0, Scenes.MONASTERY_ACT_1 },
                { 1, Scenes.MONASTERY_ACT_2 },
                { 2, Scenes.NIGHT_BAR_ACT_1 },
                { 3, Scenes.NIGHT_BAR_ACT_2 },
                { 4, Scenes.HOSPITAL_ACT_1 },
                { 5, Scenes.HOSPITAL_ACT_2 },
                { 6, Scenes.DESERT_ACT_1 },
                { 7, Scenes.DESERT_ACT_2 },
                { 8, Scenes.LAB_ACT_1 },
                { 9, Scenes.LAB_ACT_2 },
                { 10, Scenes.COMMIE_HQ_ACT_1 },
                { 11, Scenes.COMMIE_HQ_ACT_2 },
                { 12, Scenes.FINAL_ZONE }
            };
        return scenes[optionIndex];
    }

    private void SetWaters() {
        var waters = new Dictionary<int, int>() {
            { 0, 2 },
            { 1, 3 },
            { 2, 4 },
            { 3, 5 },
            { 4, 6 },
            { 5, 7 },
            { 6, 8 },
            { 7, 9 },
            { 8, 10 },
            { 9, 11 },
            { 10, 12 },
            { 11, 13 },
            { 12, 14 }
        };
        GameState.holyWaters = waters[optionIndex];
    }
}

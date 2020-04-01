using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using Steamworks;
using System;

public class GameState : MonoBehaviour {

    // Singleton instance of the GameState.
    public static GameState instance;

    // Game configuration.
    public static SystemLanguage lang { private set; get; }
    public static Difficulty difficulty = Difficulty.MEDIUM;
    public static float sfxVolume = GameplayValues.defaultSfxVolume;
    public static float musicVolume = GameplayValues.defaultMusicVolume;
    public static float pixelsPerUnit = 100f;
    public static bool debugActive = false;

    // Steamworks stuff.
    public static bool withSteam = true;
    private static SteamManager steamManager;
    public List<Achievements> achieved = new List<Achievements>();

    // In-game values.
    public static int lives = 3;
    public static int holyWaters = GameplayValues.GetHolyWaters();
    public static bool isGameLocked = false;
    public static int score = 0;
    public int obleas = 0;
    public float currentSceneTime = 0f;
    public bool isCameraLocked = false;

    // Checkpoint stuff. Refactor?
    public static Checkpoint lastCheckpoint;
    public List<string> enemiesKilled = new List<string>();
    public Dictionary<string, int> enemySlimeHits = new Dictionary<string, int>();
    public List<string> objectsUsed = new List<string>();

    // Boss state, if any.
    // TODO: Separate the boss stuff on its own class? Or something.
    public static bool activatingBoss = false;
    public static bool bossActive = false;
    public bool dying = false;
    public static bool reachedBoss = false;

    // Hacky, revisar en el futuro.
    public static bool menuAvailable = true;
    private static string[] pastGamepads = new string[0];

    // Hm.
    public static string nextScene = Scenes.LOADING;

    // Singleton instance manager.
    public static GameState GetInstance() {
        if (instance == null) {
            instance = new GameObject("Game State Manager").AddComponent<GameState>();
            instance.AutopickLanguage();
            instance.LoadConfig();
            if (withSteam) {
                steamManager = new GameObject().AddComponent<SteamManager>();
            }
        }
        return instance;
    }

    public void Awake() {
        if (instance == null) {
            instance = this;
        }
        DontDestroyOnLoad(instance);
    }

    public void AutopickLanguage() {
        if (!LanguageManager.wasLanguageChosen) {
            SetLanguage(LanguageManager.GetLanguage());
        }
    }

    public void LoadConfig() {
        GameConfig gc = SaveManager.LoadGameConfig();
        musicVolume = gc.musicVolume;
        sfxVolume = gc.sfxVolume;
        SetLanguage(gc.language);
        var ip = InputManager.GetInstance();
        ip.UpdateKeybinds(gc.keyboardConfig);
        ip.UpdateKeybinds(gc.padConfig);
    }

    public void Start() {
        SceneManager.sceneLoaded += OnSceneLoaded;
        CheckAchievements();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        pastGamepads = Input.GetJoystickNames();
        AutopickLanguage();
        var player = ObjectLocator.GetPlayer();

        if (lastCheckpoint != null) {
            currentSceneTime = lastCheckpoint.timeEllapsed;
            score = lastCheckpoint.score;
            obleas = lastCheckpoint.wafers;
            holyWaters = lastCheckpoint.holyWaters;
            if (player) {
                player.GetComponent<Transform>().position = lastCheckpoint.position;
            }
        } else {
            // If there's a checkpoint, it means the player just played the level.
            // Boss teleport is only for the continue option.
            if (reachedBoss && player) {
                player.GetComponent<PlatformerMovement2D>().TeleportToBoss();
            }
        }

        // Hmmm...
        var am = AudioManager.GetInstance();
        if (scene.name == Scenes.FINAL_ZONE) {
            am.ResetFinalBossSong();
        }
        am.PlaySceneTheme(true);
    }

    // I guess.
    private void Update() {
        CheckGamePadUpdate();

        if (debugActive && Input.GetKeyDown(KeyCode.K)) {
            SteamUserStats.ClearAchievement(Achievements.JUMPKILL_ONE.ToString());
        }
    }

    private void LoadConfig(GameConfig gameConfig) {
        sfxVolume = gameConfig.sfxVolume;
        musicVolume = gameConfig.musicVolume;
        SetLanguage(gameConfig.language);
        var im = InputManager.GetInstance();
        im.UpdateKeybinds(gameConfig.keyboardConfig);
        im.UpdateKeybinds(gameConfig.padConfig);
    }

    public void CheckGamePadUpdate() {
        if (Input.GetJoystickNames().Length != pastGamepads.Length) {
            InputManager.GetInstance().AutoloadGamepadConfiguration();
            pastGamepads = Input.GetJoystickNames();
        }
    }

    public void SetLanguage(SystemLanguage chosenLang) {
        lang = LanguageManager.LanguageOrDefault(chosenLang);
    }

    public string GetCurrentScene() {
        return SceneManager.GetActiveScene().name;
    }

    public void GameOver() {
        SetGameReadyValues();
        if (difficulty != Difficulty.EXTREME) {
            SetWatersForContinue();
        }

        // Overwrite continue, if any, with new gameplay values.
        var savedState = SaveManager.LoadGameData();
        if (savedState.saved) {
            SaveGame(savedState.level);
        }
        LoadScene(Scenes.GAME_OVER);
    }

    private void SetWatersForContinue() {
        var sc = GetCurrentScene();
        var watersPerLevel = new Dictionary<string, int>() {
            { Scenes.MONASTERY_ACT_1, 0 },
            { Scenes.MONASTERY_ACT_2, 1 },
            { Scenes.NIGHT_BAR_ACT_2, 3 },
            { Scenes.HOSPITAL_ACT_2, 5 },
            { Scenes.DESERT_ACT_2, 7 },
            { Scenes.LAB_ACT_2, 9 },
            { Scenes.COMMIE_HQ_ACT_2, 11 },
            { Scenes.FINAL_ZONE, 12 }
        };
        if (difficulty != Difficulty.HARD) {
            watersPerLevel.Add(Scenes.NIGHT_BAR_ACT_1, 2);
            watersPerLevel.Add(Scenes.HOSPITAL_ACT_1, 4);
            watersPerLevel.Add(Scenes.DESERT_ACT_1, 6);
            watersPerLevel.Add(Scenes.LAB_ACT_1, 8);
            watersPerLevel.Add(Scenes.COMMIE_HQ_ACT_1, 10);
        }
        if (watersPerLevel.ContainsKey(sc)) {
            holyWaters += watersPerLevel[sc];
        }
    }

    public void LoadScene(string scene) {
        LoadScreenScene(scene);
    }

    public void LoadScene(string scene, float delay) {
        StartCoroutine(LoadSceneDelay(scene, delay));
    }

    private IEnumerator LoadSceneDelay(string scene, float delay) {
        yield return new WaitForSeconds(delay);
        LoadScreenScene(scene);
    }

    // Stores the name of the next scene and then switches to the loading screen.
    // The loading screen checks the next scene to pre-load assets and allows for GC.
    private void LoadScreenScene(string scene) {
        nextScene = scene;
        AudioManager.GetInstance().StopAllMusic();
        SceneManager.LoadScene(Scenes.LOADING);
    }

    public void ReloadScene() {
        LoadScene(SceneManager.GetActiveScene().name);
    }

    // These are the default new game values.
    public void SetGameReadyValues() {
        holyWaters = GameplayValues.GetHolyWaters();
        score = 0;
        lives = 3;
        lastCheckpoint = null;
        SetLevelReadyValues();
    }

    // These are the values that must be reset every time a level is played.
    public void SetLevelReadyValues() {
        isGameLocked = false;
        currentSceneTime = 0f;
        obleas = 0;
        bossActive = false;
        isCameraLocked = false;
        dying = false;
        activatingBoss = false;
        enemiesKilled = new List<string>();
        objectsUsed = new List<string>();
    }

    public void DeathFinished() {
        AudioManager.GetInstance().StopAllMusic();
        if (lives > 0) {
            if (GameplayValues.ReloadWatersOnDeath()) {
                holyWaters = GameplayValues.GetHolyWaters();
            }
            lives -= 1;
            ReloadScene();
        } else {
            GameOver();
        }
    }

    // Move this to a state saving manager or something?
    public void LoadGame(SaveState savedState) {
        SetGameReadyValues();
        difficulty = savedState.difficulty;
        lang = savedState.language;
        score = savedState.score;
        lives = savedState.lives;
        holyWaters = savedState.holyWaters;
        LoadScene(savedState.level);
    }

    public void SaveGame(string sceneName) {
        SaveManager.SaveGameData(new SaveState(sceneName, difficulty, lang, score, lives, holyWaters, reachedBoss));
    }

    private void CheckAchievements() {
        if (withSteam && !SteamManager.disabled && SteamManager.Initialized) {
            try {
                foreach (Achievements achievement in (Achievements[]) Enum.GetValues(typeof(Achievements))) {
                    var gottem = false;
                    SteamUserStats.GetAchievement(achievement.ToString(), out gottem);
                    if (gottem) {
                        achieved.Add(achievement);
                    }
                }
            } catch (Exception e) {
                Debug.Log("Error trying to get achivements: " + e.ToString());
            }
        }
    }
}

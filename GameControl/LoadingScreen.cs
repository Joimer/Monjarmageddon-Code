using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Text;

public class LoadingScreen : MonoBehaviour {

    public Text loadingText;
    private int points = 3;
    private float tick;
    private AudioManager audioManager;

    private void Start() {
        audioManager = AudioManager.GetInstance();
        LoadAssets(GameState.nextScene);
        SetGameValues();
        StartCoroutine(UnloadAssets());
        StartCoroutine(LoadNewScene());
        tick = Time.time;
    }

    private void Update() {
        if (Time.time - tick > 0.25f) {
            points++;
            tick = Time.time;
        }

        if (points == 4) {
            points = 0;
        }

        var theText = new StringBuilder(TextManager.GetText("loading"));
        for (var i = 0; i < points; i++) {
            theText.Append(".");
        }
        loadingText.text = theText.ToString();
    }

    IEnumerator UnloadAssets() {
        var unload = Resources.UnloadUnusedAssets();
        while (!unload.isDone) {
            yield return null;
        }
    }

    IEnumerator LoadNewScene() {
        var async = SceneManager.LoadSceneAsync(GameState.nextScene, LoadSceneMode.Single);
        while (!async.isDone) {
            yield return null;
        }
    }

    private void SetGameValues() {
        GameState.GetInstance().SetLevelReadyValues();
    }

    // Pre-load things we need on the next scene and unload the ones we don't need anymore.
    // TODO: Refactor something here
    private void LoadAssets(string scene) {
        if (scene == Scenes.INTRO) {
            audioManager.LoadMusic(Music.INTRO_TEXT);
        }

        if (scene == Scenes.MAIN_MENU) {
            audioManager.UnloadClip(Music.INTRO_TEXT);
            audioManager.LoadMusic(Music.MAIN_MENU);
            audioManager.LoadSounds();
        }

        if (scene == Scenes.MONASTERY_ACT_1) {
            audioManager.UnloadClip(Music.MAIN_MENU);
            audioManager.LoadMusic(Music.MONASTERY_ACT_1);
        }

        if (scene == Scenes.MONASTERY_ACT_2) {
            audioManager.UnloadClip(Music.MONASTERY_ACT_1);
            audioManager.LoadMusic(Music.MONASTERY_ACT_2);
        }

        if (scene == Scenes.NIGHT_BAR_ACT_1 || scene == Scenes.NIGHT_BAR_ACT_2) {
            audioManager.UnloadClip(Music.MONASTERY_ACT_2);
            audioManager.LoadMusic(Music.NIGHT_BAR_INTRO);
            audioManager.LoadMusic(Music.NIGHT_BAR_ACT_1);
        }

        if (scene == Scenes.DESERT_ACT_1 || scene == Scenes.DESERT_ACT_2) {
            audioManager.UnloadClip(Music.NIGHT_BAR_INTRO);
            audioManager.UnloadClip(Music.NIGHT_BAR_ACT_1);
            audioManager.LoadMusic(Music.DESERT_ACT_1);
            audioManager.LoadSfx(Sfx.EXPLOSION);
        }

        if (scene == Scenes.LAB_ACT_1 || scene == Scenes.LAB_ACT_2) {
            audioManager.UnloadClip(Music.DESERT_ACT_1);
            audioManager.LoadMusic(Music.LAB_ACT_1);
            audioManager.LoadSfx(Sfx.EXPLOSION);
        }

        if (scene == Scenes.COMMIE_HQ_ACT_1 || scene == Scenes.COMMIE_HQ_ACT_2) {
            audioManager.UnloadClip(Sfx.EXPLOSION);
            audioManager.UnloadClip(Music.LAB_ACT_1);
            audioManager.LoadMusic(Music.COMMIE_HQ_ACT_1);
        }

        if (scene == Scenes.GAME_OVER) {
            audioManager.LoadMusic(Music.GAME_OVER);
        }
    }
}

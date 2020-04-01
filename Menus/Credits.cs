using UnityEngine;
using UnityEngine.UI;

public class Credits : MonoBehaviour {

    private Text text;
    private string[] texts = new string[5] {
        "<b><color=#FFFF00>Game Design, Programming, Art</color></b>\nJuan Manuel Serrano",
        "<b><color=#FFFF00>Music</color></b>\nAbstraction (www.abstractionmusic.com)\nLophex\nSamuel Brennan",
        "<b><color=#FFFF00>Localization</color></b>\nJavier Arrieta Piñeiro\nJuan Manuel Serrano\nMarina Gutiérrez\nMarkus Wieczorek\n",
        "<b><color=#FFFF00>Testing and Special Thanks</color></b>\nChris Monsanto\nGareth Kingle\nJess Bermudes\nJorge Lázaro Molina\nSamuel \"Venturno\" Falcón Fernández",
        "<b>The End</b>"
    };
    private int textIndex = 0;
    private float passed = 0f;
    
    public void Awake() {
        text = GetComponent<Text>();
        text.text = texts[0];
    }

    public void Start() {
        var am = AudioManager.GetInstance();
        am.PlayMusicOnce(Music.MAIN_MENU);
        passed = 0f;
    }

    public void Update() {
        passed += Time.deltaTime;

        if (passed > 5f) {
            textIndex = 1;
        }

        if (passed > 10f) {
            textIndex = 2;
        }

        if (passed > 17f) {
            textIndex = 3;
        }

        if (passed > 23f) {
            textIndex = 4;
        }

        if (passed > 30f) {
            GameState.GetInstance().LoadScene(Scenes.SPLASH_SCREEN);
        }

        text.text = texts[textIndex];
    }
}

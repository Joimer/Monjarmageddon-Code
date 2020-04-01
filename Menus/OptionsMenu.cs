using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class OptionsMenu : MonoBehaviour {

    private GameState gs;
    private Font font;
    private Texture2D icon;
    private int optionIndex = 0;
    private int langIndex = 1;
    private Dictionary<SystemLanguage, string> langNames = new Dictionary<SystemLanguage, string>() {
        { SystemLanguage.Spanish, "Español" },
        { SystemLanguage.English, "English" },
        { SystemLanguage.Portuguese, "Portugês" },
        { SystemLanguage.French, "Français" },
        { SystemLanguage.German, "Deutsche" },
        { SystemLanguage.Catalan, "Català" },
        { SystemLanguage.Basque, "Euskara" },
        { SystemLanguage.Japanese, "日本語" },
    };
    private bool switchingScene = false;

    // The text elements:
    private Text optionsTitle;
    private Text language;
    private Text languageName;
    private Text sfxVolume;
    private Text sfxVolumeChosen;
    private Text musicVolume;
    private Text musicVolumeChosen;
    private Text configureControls;
    private Text back;
    private RectTransform nunIcon;

    public void Awake() {
        gs = GameState.GetInstance();
        // Change to public properties.
        optionsTitle = GameObject.Find("options_title").GetComponent<Text>();
        language = GameObject.Find("language").GetComponent<Text>();
        languageName = GameObject.Find("language_name").GetComponent<Text>();
        sfxVolume = GameObject.Find("sfx_volume").GetComponent<Text>();
        sfxVolumeChosen = GameObject.Find("sfx_volume_chosen").GetComponent<Text>();
        musicVolume = GameObject.Find("music_volume").GetComponent<Text>();
        musicVolumeChosen = GameObject.Find("music_volume_chosen").GetComponent<Text>();
        configureControls = GameObject.Find("configure_controls").GetComponent<Text>();
        back = GameObject.Find("back").GetComponent<Text>();
        nunIcon = GameObject.Find("nun_icon").GetComponent<RectTransform>();
    }

    public void Start() {
        if (LanguageManager.languageToIndex.ContainsKey(GameState.lang)) {
            langIndex = LanguageManager.languageToIndex[GameState.lang];
        } else {
            // This is just to make the scene work independently, without a pre-set of language.
            langIndex = 1;
            gs.SetLanguage(SystemLanguage.English);
        }
        UpdateMenuLabels();
        UpdateLanguageOption();
        UpdateSfxOption();
        UpdateMusicOption();
    }

    public void Update() {
        var inputManager = InputManager.GetInstance();
        if (inputManager.IsMenuPressRight() && !switchingScene) {
            AudioManager.GetInstance().PlayEffect(Sfx.MENU_BEEP);

            // Option to change the language.
            if (optionIndex == 0) {
                TextManager.CleanCache();
                langIndex++;
                if (langIndex == LanguageManager.supportedLangs.Length) {
                    langIndex =  0;
                }
                UpdateLang(LanguageManager.supportedLangs[langIndex]);
                UpdateMenuLabels();
                UpdateLanguageOption();
            }

            // Adjusting the SFX volume.
            if (optionIndex == 1 && GameState.sfxVolume < 10) {
                GameState.sfxVolume++;
                UpdateSfxOption();
            }

            // Adjusting the music volume.
            if (optionIndex == 2 && GameState.musicVolume < 10) {
                GameState.musicVolume++;
                UpdateMusicOption();
            }
        }

        if (inputManager.IsMenuPressLeft() && !switchingScene) {
            AudioManager.GetInstance().PlayEffect(Sfx.MENU_BEEP);

            // Option to change the language.
            if (optionIndex == 0) {
                TextManager.CleanCache();
                langIndex--;
                if (langIndex == -1) {
                    langIndex = LanguageManager.supportedLangs.Length - 1;
                }
                UpdateLang(LanguageManager.supportedLangs[langIndex]);
                UpdateMenuLabels();
                UpdateLanguageOption();
            }

            // Adjusting the SFX volume.
            if (optionIndex == 1 && GameState.sfxVolume > 0) {
                GameState.sfxVolume--;
                UpdateSfxOption();
            }

            // Adjusting the music volume.
            if (optionIndex == 2 && GameState.musicVolume > 0) {
                GameState.musicVolume--;
                UpdateMusicOption();
            }
        }

        // Going up and down in options.
        if (inputManager.IsMenuPressUp() && !switchingScene) {
            AudioManager.GetInstance().PlayEffect(Sfx.MENU_BEEP);
            optionIndex--;
            if (optionIndex == -1) {
                optionIndex = 4;
            }
            UpdateNunIconPosition();
        }

        if (inputManager.IsMenuPressDown() && !switchingScene) {
            AudioManager.GetInstance().PlayEffect(Sfx.MENU_BEEP);
            optionIndex++;
            if (optionIndex == 5) {
                optionIndex = 0;
            }
            UpdateNunIconPosition();
        }

        // Loading configure controls options.
        if (optionIndex == 3 && inputManager.IsMenuStart() && !switchingScene) {
            AudioManager.GetInstance().PlayEffect(Sfx.MENU_ACCEPT);
            switchingScene = true;
            gs.LoadScene(Scenes.CONFIGURE_CONTROLS, 0.5f);
        }

        // Going back to main menu.
        if (optionIndex == 4 && inputManager.IsMenuStart() && !switchingScene) {
            TextManager.CleanCache();
            AudioManager.GetInstance().PlayEffect(Sfx.MENU_ACCEPT);
            switchingScene = true;
            GameConfig gc = SaveManager.LoadGameConfig();
            gc.musicVolume = GameState.musicVolume;
            gc.sfxVolume = GameState.sfxVolume;
            SaveManager.SaveGameConfig(gc);
            gs.SetGameReadyValues();
            gs.LoadScene(Scenes.MAIN_MENU, 0.5f);
        }

        // Fade out.
        if (switchingScene) {
            var color = GetComponent<SpriteRenderer>().color;
            color.a += 0.03f;
            GetComponent<SpriteRenderer>().color = color;
        }
    }

    private void UpdateLang(SystemLanguage lang) {
        gs.SetLanguage(LanguageManager.supportedLangs[langIndex]);
        LanguageManager.wasLanguageChosen = true;
    }

    private void UpdateMenuLabels() {
        optionsTitle.text = TextManager.GetText("options");
        language.text = TextManager.GetText("language");
        sfxVolume.text = TextManager.GetText("sfx volume");
        musicVolume.text = TextManager.GetText("music volume");
        configureControls.text = TextManager.GetText("configure controls");
        back.text = TextManager.GetText("back");
    }

    private void UpdateLanguageOption() {
        languageName.text = "< " + langNames[LanguageManager.supportedLangs[langIndex]] + " >";
    }

    private void UpdateNunIconPosition() {
        var x = nunIcon.anchoredPosition.x;
        var y = 157 - optionIndex * 33;
        nunIcon.anchoredPosition = new Vector2(x, y);
    }

    private void UpdateSfxOption() {
        string text = "";
        if (GameState.sfxVolume > 0) {
            text = "< ";
        }
        text += GameState.sfxVolume.ToString();
        if (GameState.sfxVolume < 10) {
            text += " >";
        }
        sfxVolumeChosen.text = text;
    }

    private void UpdateMusicOption() {
        string text = "";
        if (GameState.musicVolume > 0) {
            text = "< ";
        }
        text += GameState.musicVolume.ToString();
        if (GameState.musicVolume < 10) {
            text += " >";
        }
        musicVolumeChosen.text = text;
    }
}

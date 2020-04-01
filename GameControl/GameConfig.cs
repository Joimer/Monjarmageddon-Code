using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class GameConfig {

    public float musicVolume = GameplayValues.defaultMusicVolume;
    public float sfxVolume = GameplayValues.defaultSfxVolume;
    public Dictionary<GameCommand, KeyCode> keyboardConfig = GameplayValues.defaultKeyboardConfig;
    public Dictionary<GameCommand, GamepadKey> padConfig = GameplayValues.defaultPadConfig;
    public SystemLanguage language = LanguageManager.DetectLanguage();

    public GameConfig() {}

    public GameConfig(
        float musicVolume,
        float sfxVolume, Dictionary<GameCommand, KeyCode> keyboardConfig,
        Dictionary<GameCommand, GamepadKey> padConfig,
        SystemLanguage language
    ) {
        this.musicVolume = musicVolume;
        this.sfxVolume = sfxVolume;
        this.keyboardConfig = keyboardConfig;
        this.padConfig = padConfig;
        this.language = language;
    }
}

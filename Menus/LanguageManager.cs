using UnityEngine;
using System;
using System.Collections.Generic;

public static class LanguageManager {

    public static bool wasLanguageChosen = false;

    public static SystemLanguage[] supportedLangs = new SystemLanguage[] {
        SystemLanguage.Spanish,
        SystemLanguage.English,
        SystemLanguage.Portuguese,
        SystemLanguage.French,
        SystemLanguage.German,
        SystemLanguage.Catalan,
        SystemLanguage.Basque,
        SystemLanguage.Japanese
    };

    public static Dictionary<SystemLanguage, int> languageToIndex = new Dictionary<SystemLanguage, int>() {
        { SystemLanguage.Spanish, 0 },
        { SystemLanguage.English, 1 },
        { SystemLanguage.Portuguese, 2 },
        { SystemLanguage.French, 3 },
        { SystemLanguage.German, 4 },
        { SystemLanguage.Catalan, 5 },
        { SystemLanguage.Basque, 6 },
        { SystemLanguage.Japanese, 7 }
    };

    public static SystemLanguage DetectLanguage() {
        return Application.systemLanguage;
    }

    public static SystemLanguage GetDefault() {
        return SystemLanguage.English;
    }

    public static SystemLanguage GetLanguage() {
        var lang = DetectLanguage();
        if (!IsValid(lang))  {
            lang = GetDefault();
        }

        return lang;
    }

    private static bool IsValid(SystemLanguage lang) {
        return Array.IndexOf(supportedLangs, lang) != -1;
    }

    public static SystemLanguage LanguageOrDefault(SystemLanguage lang) {
        if (!IsValid(lang)) {
            return GetDefault();
        }

        return lang;
    }
}

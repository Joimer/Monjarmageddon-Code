using UnityEngine;
using System.Collections.Generic;

// TODO: Improve
public static class GameplayValues {

    public static int watersPerHit = 1;
    public static float jumpHeight = 1.5f;
    public static float gravity = -30f;
    public static float runSpeed = 3f;
    public static float shootingCooldown = 0.33f;
    public static float defaultSfxVolume = 7f;
    public static float defaultMusicVolume = 7f;
    public static Dictionary<GameCommand, KeyCode> defaultKeyboardConfig = new Dictionary<GameCommand, KeyCode>() {
        { GameCommand.JUMP, KeyCode.Z },
        { GameCommand.SHOOT, KeyCode.X },
        { GameCommand.UP, KeyCode.UpArrow },
        { GameCommand.DOWN, KeyCode.DownArrow },
        { GameCommand.LEFT, KeyCode.LeftArrow },
        { GameCommand.RIGHT, KeyCode.RightArrow },
        { GameCommand.ACCEPT, KeyCode.Return }
    };
    public static Dictionary<GameCommand, GamepadKey> defaultPadConfig = new Dictionary<GameCommand, GamepadKey>() {
        { GameCommand.JUMP, new GamepadKey(KeyCode.JoystickButton1) },
        { GameCommand.SHOOT, new GamepadKey(KeyCode.JoystickButton2) },
        { GameCommand.UP, new GamepadKey(GamepadAxis.VERTICAL, true) },
        { GameCommand.DOWN, new GamepadKey(GamepadAxis.VERTICAL, false) },
        { GameCommand.LEFT, new GamepadKey(GamepadAxis.HORIZONTAL, false) },
        { GameCommand.RIGHT, new GamepadKey(GamepadAxis.HORIZONTAL, true) },
        { GameCommand.ACCEPT, new GamepadKey(KeyCode.JoystickButton9) }
    };

    public static int GetObleasForHolyWater() {
        // Make static for performance? or something
        var levelToObleas = new Dictionary<Difficulty, int>() {
            { Difficulty.VERY_EASY, 20 },
            { Difficulty.EASY, 40 },
            { Difficulty.MEDIUM, 50 },
            { Difficulty.HARD, 60 },
            { Difficulty.EXTREME, 100 }
        };

        // I mean, this should be cached or something.
        return levelToObleas[GameState.difficulty];
    }

    public static float GetInvulnerableTime() {
        var invulnTime = new Dictionary<Difficulty, float>() {
            { Difficulty.VERY_EASY, 2f },
            { Difficulty.EASY, 1.5f },
            { Difficulty.MEDIUM, 1f },
            { Difficulty.HARD, 0.75f },
            { Difficulty.EXTREME, 0.5f }
        };

        return invulnTime[GameState.difficulty];
    }

    public static int GetHolyWaters() {
        var waters = new Dictionary<Difficulty, int>() {
            { Difficulty.VERY_EASY, 3 },
            { Difficulty.EASY, 3 },
            { Difficulty.MEDIUM, 2 },
            { Difficulty.HARD, 1 },
            { Difficulty.EXTREME, 0 }
        };

        return waters[GameState.difficulty];
    }

    public static int GetHolyWatersByLevel(string level) {
        var baseWaters = GetHolyWaters();

        // Extreme difficulty rewards no waters.
        if (GameState.difficulty == Difficulty.EXTREME) {
            return baseWaters;
        }

        // Waters given in hard difficulty and the rest.
        Dictionary<string, int> addWaters;
        if (GameState.difficulty == Difficulty.HARD) {
            addWaters = new Dictionary<string, int>() {
                { Scenes.MONASTERY_ACT_1, 0 },
                { Scenes.MONASTERY_ACT_2, 0 },
                { Scenes.NIGHT_BAR_ACT_1, 1 },
                { Scenes.NIGHT_BAR_ACT_2, 1 },
                { Scenes.HOSPITAL_ACT_1, 2 },
                { Scenes.HOSPITAL_ACT_2, 2 },
                { Scenes.DESERT_ACT_1, 3 },
                { Scenes.DESERT_ACT_2, 3 },
                { Scenes.LAB_ACT_1, 4 },
                { Scenes.LAB_ACT_2, 4 },
                { Scenes.COMMIE_HQ_ACT_1, 5 },
                { Scenes.COMMIE_HQ_ACT_2, 5 },
                { Scenes.FINAL_ZONE, 6 }
            };
        } else {
            addWaters = new Dictionary<string, int>() {
                { Scenes.MONASTERY_ACT_1, 0 },
                { Scenes.MONASTERY_ACT_2, 1 },
                { Scenes.NIGHT_BAR_ACT_1, 2 },
                { Scenes.NIGHT_BAR_ACT_2, 3 },
                { Scenes.HOSPITAL_ACT_1, 4 },
                { Scenes.HOSPITAL_ACT_2, 5 },
                { Scenes.DESERT_ACT_1, 6 },
                { Scenes.DESERT_ACT_2, 7 },
                { Scenes.LAB_ACT_1, 8 },
                { Scenes.LAB_ACT_2, 9 },
                { Scenes.COMMIE_HQ_ACT_1, 10 },
                { Scenes.COMMIE_HQ_ACT_2, 11 },
                { Scenes.FINAL_ZONE, 12 }
            };
        }

        // In case of corrupted save file.
        if (!addWaters.ContainsKey(level)) {
            return 0;
        }

        return baseWaters + addWaters[level];
    }

    public static float GetInvulnerableBoostTime() {
        var invBoostTime = new Dictionary<Difficulty, float>() {
            { Difficulty.VERY_EASY, 30f },
            { Difficulty.EASY, 20f },
            { Difficulty.MEDIUM, 15f },
            { Difficulty.HARD, 10f },
            { Difficulty.EXTREME, 5f }
        };

        return invBoostTime[GameState.difficulty];
    }

    public static float GetEnemyShootSpeed() {
        var enemyShotSpeed = new Dictionary<Difficulty, float>() {
            { Difficulty.VERY_EASY, 40f },
            { Difficulty.EASY, 50f },
            { Difficulty.MEDIUM, 100f },
            { Difficulty.HARD, 200f },
            { Difficulty.EXTREME, 300f }
        };

        return enemyShotSpeed[GameState.difficulty];
    }

    public static bool ReloadWatersOnDeath() {
        var reloadWaters = new Dictionary<Difficulty, bool>() {
            { Difficulty.VERY_EASY, true },
            { Difficulty.EASY, true },
            { Difficulty.MEDIUM, true },
            { Difficulty.HARD, false },
            { Difficulty.EXTREME, false }
        };

        return reloadWaters[GameState.difficulty];
    }

    public static float GetSpeedBuffDuration() {
        var speedBuff = new Dictionary<Difficulty, float>() {
            { Difficulty.VERY_EASY, 20f },
            { Difficulty.EASY, 15f },
            { Difficulty.MEDIUM, 10f },
            { Difficulty.HARD, 7f },
            { Difficulty.EXTREME, 4f }
        };

        return speedBuff[GameState.difficulty];
    }

    public static int GetBossHits() {
        var speedBuff = new Dictionary<Difficulty, int>() {
            { Difficulty.VERY_EASY, 6 },
            { Difficulty.EASY, 8 },
            { Difficulty.MEDIUM, 8 },
            { Difficulty.HARD, 8 },
            { Difficulty.EXTREME, 10 }
        };

        return speedBuff[GameState.difficulty];
    }

    public static float GetEnemyInvulnerableTime() {
        var invulnTime = new Dictionary<Difficulty, float>() {
            { Difficulty.VERY_EASY, 0.5f },
            { Difficulty.EASY, 0.75f },
            { Difficulty.MEDIUM, 1f },
            { Difficulty.HARD, 1.15f },
            { Difficulty.EXTREME, 1.25f }
        };

        return invulnTime[GameState.difficulty];
    }

    public static float GetBonusTimeForStage(string stage) {
        var times = new Dictionary<string, float>() {
            { Scenes.MONASTERY_ACT_1, 120f },
            { Scenes.MONASTERY_ACT_2, 140f },
            { Scenes.NIGHT_BAR_ACT_1, 120f },
            { Scenes.NIGHT_BAR_ACT_2, 160f },
            { Scenes.HOSPITAL_ACT_1, 160f },
            { Scenes.HOSPITAL_ACT_2, 200f },
            { Scenes.DESERT_ACT_1, 300f },
            { Scenes.DESERT_ACT_2, 360f },
            { Scenes.LAB_ACT_1, 350f },
            { Scenes.LAB_ACT_2, 500f },
            { Scenes.COMMIE_HQ_ACT_1, 330f },
            { Scenes.COMMIE_HQ_ACT_2, 480f },
            { Scenes.FINAL_ZONE, 200f }
        };
        if (times.ContainsKey(stage)) {
            return times[stage];
        } else {
            return 120f;
        }
    }
}

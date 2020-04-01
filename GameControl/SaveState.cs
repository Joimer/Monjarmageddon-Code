using UnityEngine;
using System;

[Serializable]
public class SaveState {
    public bool saved = false;
    public string level;
    public Difficulty difficulty;
    public SystemLanguage language;
    public int score;
    public int lives;
    public int holyWaters;
    public bool reachedBoss;

    public SaveState() {}

    public SaveState(
        string level, Difficulty difficulty, SystemLanguage language,
        int score, int lives, int holyWaters, bool reachedBoss
    ) {
        saved = true;
        this.level = level;
        this.difficulty = difficulty;
        this.language = language;
        this.score = score;
        this.lives = lives;
        this.holyWaters = holyWaters;
        this.reachedBoss = reachedBoss;
    }
}

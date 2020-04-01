using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text;

public class EndAct : MonoBehaviour {

    private int step = 0;
    private GameObject hasPassed;
    private GameObject timeBonus;
    private GameObject clearBonus;
    private GameObject waferBonus;
    private GameObject blackBg;
    private GameState gs;
    private float timePoints = 0;
    private float timePointsShown = 0;
    private int enemyPoints = 0;
    private int enemyPointsShown = 0;
    private int waferPoints = 0;
    private int waferPointsShown = 0;
    private float endPointCalc = 0;
    public int act = 1;
    private float started = 0f;
    private Dictionary<string, string> nextScenes = new Dictionary<string, string>() {
        { Scenes.MONASTERY_ACT_1, Scenes.MONASTERY_ACT_2 },
        { Scenes.MONASTERY_ACT_2, Scenes.NIGHT_BAR_ACT_1 },
        { Scenes.NIGHT_BAR_ACT_1, Scenes.NIGHT_BAR_ACT_2 },
        { Scenes.NIGHT_BAR_ACT_2, Scenes.HOSPITAL_ACT_1 },
        { Scenes.HOSPITAL_ACT_1, Scenes.HOSPITAL_ACT_2 },
        { Scenes.HOSPITAL_ACT_2, Scenes.DESERT_ACT_1 },
        { Scenes.DESERT_ACT_1, Scenes.DESERT_ACT_2 },
        { Scenes.DESERT_ACT_2, Scenes.LAB_ACT_1 },
        { Scenes.LAB_ACT_1, Scenes.LAB_ACT_2 },
        { Scenes.LAB_ACT_2, Scenes.COMMIE_HQ_ACT_1 },
        { Scenes.COMMIE_HQ_ACT_1, Scenes.COMMIE_HQ_ACT_2 },
        { Scenes.COMMIE_HQ_ACT_2, Scenes.FINAL_ZONE },
        { Scenes.FINAL_ZONE, Scenes.CREDITS }
    };

    private void Awake() {
        // TODO: Refactor.
        hasPassed = GameObject.Find("nun_has_passed");
        timeBonus = GameObject.Find("time_bonus");
        clearBonus = GameObject.Find("clear_bonus");
        waferBonus = GameObject.Find("wafer_bonus");
        blackBg = GameObject.Find("black_bg");
    }

    private void Start() {
        blackBg.GetComponent<SpriteRenderer>().enabled = true;
        gs = GameState.GetInstance();
        AudioManager.GetInstance().PlayMusicOnce(Music.END_ACT);
        started = Time.time;

        // This avoids getting hit by loose projectiles when the act is ending.
        var player = ObjectLocator.GetPlayer();
        if (player != null) {
            player.GetComponent<Invulnerability>().SetInvulnerableNoBlink(30f);
        }
    }

    // TODO: Refactor. Fade in of black bg should be in a fixedupdate method.
	private void Update() {
        if (step == 0) {
            if (act == 3) {
                hasPassed.GetComponent<Text>().text = TextManager.GetText("game finished");
            } else {
                if (act == 2) {
                    // When boss is cleared, new level is saved so clear boss reached flag.
                    GameState.reachedBoss = false;
                }
                hasPassed.GetComponent<Text>().text = string.Format(TextManager.GetText("act finished"), act);
            }
            GameState.isGameLocked = true;

            // This makes the previous lines to be executed a lot of times. Not good, especially managing the UI.
            Color color = blackBg.GetComponent<SpriteRenderer>().color;
            if (color.a < 1) {
                color.a += 0.01f;
                blackBg.GetComponent<SpriteRenderer>().color = color;
            } else {
                step = 1;
            }
        }

        // TODO: Textos
        if (step < 5) {
            var sbt = new StringBuilder(TextManager.GetText("time bonus"));
            sbt.Append(": ");
            sbt.Append(timePointsShown);
            var sbc = new StringBuilder(TextManager.GetText("clear bonus"));
            sbc.Append(": ");
            sbc.Append(enemyPointsShown);
            var sbw = new StringBuilder(TextManager.GetText("wafer bonus"));
            sbw.Append(": ");
            sbw.Append(waferPointsShown);
            timeBonus.GetComponent<Text>().text = sbt.ToString();
            clearBonus.GetComponent<Text>().text = sbc.ToString();
            waferBonus.GetComponent<Text>().text = sbw.ToString();
        }

        if (step == 5 && Time.time - started > 7f && Time.time - endPointCalc > 0.99f) {
            step = 6;
            // Reward waters for passing.
            var addWaters = 1;
            if (GameState.difficulty == Difficulty.EXTREME) {
                addWaters = 0;
            }
            if (GameState.difficulty == Difficulty.HARD && act == 1) {
                addWaters = 0;
            }
            GameState.holyWaters += addWaters;
            NextLevel();
        }
    }

    private void FixedUpdate() {
        if (step == 1) {
            var bonusLimit = GameplayValues.GetBonusTimeForStage(gs.GetCurrentScene());
            if (gs.currentSceneTime < bonusLimit) {
                timePoints = (bonusLimit - gs.currentSceneTime) * 10;
            }
            enemyPoints = gs.enemiesKilled.Count * 10;
            waferPoints = gs.obleas * 10;
            step = 2;
        }

        if (step == 2) {
            if (timePoints > 0) {
                timePoints -= 10;
                timePointsShown += 10;
                GameState.score += 10;
            } else {
                step = 3;
            }
        }

        if (step == 3) {
            if (enemyPoints > 0) {
                enemyPoints -= 10;
                enemyPointsShown += 10;
                GameState.score += 10;
            } else {
                step = 4;
            }
        }

        if (step == 4) {
            if (waferPoints > 0) {
                waferPoints -= 10;
                waferPointsShown += 10;
                GameState.score += 10;
            } else {
                step = 5;
                endPointCalc = Time.time;
            }
        }
    }

    private void NextLevel() {
        // HMMMMMMMMMMMMMMMMMmmmmmm...
        gs.SetLevelReadyValues();
        GameState.lastCheckpoint = null;
        gs.LoadScene(GetSceneToLoad());
    }

    private string GetSceneToLoad() {
        var gs = GameState.GetInstance();
        var scene = gs.GetCurrentScene();
        string nextScene;

        if (nextScenes.ContainsKey(scene)) {
            nextScene = nextScenes[scene];
        } else {
            nextScene = Scenes.CREDITS;
        }
        if (nextScene != Scenes.CREDITS) {
            gs.SaveGame(nextScene);
        }

        return nextScene;
    }
}

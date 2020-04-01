using Steamworks;
using System;
using System.Collections;
using UnityEngine;

public class AchievementManager : MonoBehaviour {

    public static AchievementManager instance;
    public static int jumpKills = 0;

    public static AchievementManager GetInstance() {
        if (instance == null) {
            instance = new GameObject("Achievement Manager").AddComponent<AchievementManager>();
        }
        return instance;
    }

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
        DontDestroyOnLoad(instance);
    }

    public static void UnlockAchievement(Achievements achievement) {
        if (!SteamManager.disabled && SteamManager.Initialized) {
            // If Steam Manager is enabled and the achivement isn't unlocked.
            if (!GameState.GetInstance().achieved.Contains(achievement)) {
                try {
                    SteamUserStats.SetAchievement(achievement.ToString());
                    GameState.GetInstance().achieved.Add(achievement);
                    SteamUserStats.StoreStats();
                } catch (Exception e) {
                    Debug.Log("Error trying to unlock achivement: " + e.ToString());
                }
            }
        }
    }

    public static void AddJumpKillCount() {
        jumpKills += 1;
        if (jumpKills == 1) {
            UnlockAchievement(Achievements.JUMPKILL_ONE);
        }
        if (jumpKills == 2) {
            UnlockAchievement(Achievements.JUMPKILL_TWO);
        }
        if (jumpKills == 3) {
            UnlockAchievement(Achievements.JUMPKILL_THREE);
        }
        if (jumpKills == 4) {
            UnlockAchievement(Achievements.JUMPKILL_FOUR);
        }
    }
}

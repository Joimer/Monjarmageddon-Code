using System.Collections.Generic;
using UnityEngine;

public class AbortingMom : BossEntity {

    private float fireRate = 1f;
    private float lastShot = 0f;
    private float lastFetus = 0f;
    private bool even = true;
    private bool down = true;
    private float vertical = 0f;
    private float moveSpeed = 0.025f;
    private Flipper flipper;
    // Altitude from which the fetus are dropped.
    private const float altitude = -4.3f;
    private GameObject[] fetuses;
    private GameObject pill;
    private static readonly Dictionary<Difficulty, float> fireRates = new Dictionary<Difficulty, float>() {
        { Difficulty.VERY_EASY, 1.4f },
        { Difficulty.EASY, 1.25f },
        { Difficulty.MEDIUM, 1f },
        { Difficulty.HARD, 0.75f },
        { Difficulty.EXTREME, 0.6f }
    };

    private void Awake() {
        flipper = GetComponent<Flipper>();

        // Fetus objects to throw to the player.
        var fetusPrefab = Resources.Load<GameObject>(Hazards.FETUS);
        fetuses = new GameObject[5];
        for (var i = 0; i < 5; i++) {
            fetuses[i] = Instantiate(fetusPrefab, Vector2.zero, transform.rotation);
            fetuses[i].SetActive(false);
        }

        // Pill that mom drops to hurt you.
        var pillPrefab = Resources.Load<GameObject>(Hazards.PILL);
        pill = Instantiate(pillPrefab, transform.position, transform.rotation);
        pill.SetActive(false);
    }

    public override void Activate() {
        GameState.reachedBoss = true;
        initialPosition = transform.position;
        lastShot = Time.time;
        lastFetus = Time.time;
        fireRate = fireRates[GameState.difficulty];
        if (GameState.difficulty == Difficulty.VERY_EASY) {
            invulnerableTime = 0.75f;
            moveSpeed = 0.017f;
        }
        active = true;
    }

    private void FixedUpdate() {
        if (!GameState.isGameLocked && active) {
            if (down && vertical <= -0.5f) {
                down = false;
            }
            if (!down && vertical >= 0.5f) {
                down = true;
            }

            var yMove = down ? -0.01f : 0.01f;
            vertical += yMove;

            if (isMovingLeft) {
                if (transform.position.x > initialPosition.x - 5f) {
                    transform.Translate(new Vector2(-moveSpeed, yMove));
                } else {
                    isMovingLeft = !isMovingLeft;
                    flipper.Flip();
                }
            }

            if (!isMovingLeft) {
                if (transform.position.x < initialPosition.x + 1f) {
                    transform.Translate(new Vector2(moveSpeed, yMove));
                } else {
                    isMovingLeft = !isMovingLeft;
                    flipper.Flip();
                }
            }

            // Pill dropping.
            if (GameState.difficulty > Difficulty.VERY_EASY && Time.time > lastShot + fireRate) {
                lastShot = Time.time;
                pill.transform.position = transform.position;
                pill.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                pill.GetComponent<Rigidbody2D>().angularVelocity = 0f;
                pill.SetActive(true);
            }

            // Fetus dropping.
            if (Time.time > lastFetus + fireRate * 2) {
                var add = even ? 0.5f : 0f;
                lastFetus = Time.time;
                fetuses[0].transform.position = new Vector2(92.08f + add, altitude);
                fetuses[0].SetActive(true);
                fetuses[1].transform.position = new Vector2(93.21f + add, altitude);
                fetuses[1].SetActive(true);
                fetuses[2].transform.position = new Vector2(94.62f + add, altitude);
                fetuses[2].SetActive(true);
                if (GameState.difficulty > Difficulty.VERY_EASY) {
                    fetuses[3].transform.position = new Vector2(95.32f + add, altitude);
                    fetuses[3].SetActive(true);
                    if (GameState.difficulty > Difficulty.EASY) {
                        fetuses[4].transform.position = new Vector2(96.52f + add, altitude);
                        fetuses[4].SetActive(true);
                    }
                }
                even = !even;
            }

            if (invulnerable) {
                // TODO: Refactor tu use a flasher component
                StartCoroutine("Flash");
            }
        }
    }

    public override void OnDeath() {
        AchievementManager.UnlockAchievement(Achievements.BOSS_HOSPITAL_VEZ);
        if (GameState.difficulty == Difficulty.EXTREME) {
            AchievementManager.UnlockAchievement(Achievements.BOSS_HOSPITAL_EXTREME);
        }
        if (GameState.difficulty >= Difficulty.HARD) {
            AchievementManager.UnlockAchievement(Achievements.BOSS_HOSPITAL_HARD);
        }
        if (GameState.difficulty >= Difficulty.MEDIUM) {
            AchievementManager.UnlockAchievement(Achievements.BOSS_HOSPITAL);
        }
        if (GameState.difficulty >= Difficulty.EASY) {
            AchievementManager.UnlockAchievement(Achievements.BOSS_HOSPITAL_EZ);
        }
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

public class Jesus : BossEntity {

    private GameObject leftHand;
    private GameObject rightHand;
    private List<GameObject> spikes = new List<GameObject>();
    private int step = 1;
    private float lastTick = 0f;
    private float initialLeftX;
    private float initialRightX;
    private float handCenterDistance = 1f;
    private List<Vector2> spikePos = new List<Vector2>() {
        new Vector2(7.48f, -2.95f),
        new Vector2(6.06f, -2.97f),new Vector2(6.6985f, -3.0621f)
    };
    private float handSpeed = 0.05f;
    private float initialJesusY = 0f;
    private float height = 1.9f;
    private GameObject leftHandClenched;
    private GameObject rightHandClenched;
    private Vector2 leftHandClenchInner = new Vector2(7.3f, -5.955179f);
    private Vector2 rightHandClenchInner = new Vector2(5.7f, -5.955179f);
    private Vector2 leftHandClenchOuter = new Vector2(8.3f, -5.955179f);
    private Vector2 rightHandClenchOuter = new Vector2(4.7f, -5.955179f);
    private GameObject rainbowRay;
    public event Action onDeathEvent;
    private Dictionary<Difficulty, float> rainbowDelay = new Dictionary<Difficulty, float>() {
        { Difficulty.VERY_EASY, 1f },
        { Difficulty.EASY, 0.5f },
        { Difficulty.MEDIUM, 0.25f },
        { Difficulty.HARD, 0.15f },
        { Difficulty.EXTREME, 0.1f }
    };
    private Dictionary<Difficulty, float> rainbowDuration = new Dictionary<Difficulty, float>() {
        { Difficulty.VERY_EASY, 0.25f },
        { Difficulty.EASY, 0.33f },
        { Difficulty.MEDIUM, 0.5f },
        { Difficulty.HARD, 0.65f },
        { Difficulty.EXTREME, 0.75f }
    };

    private void Start() {
        lastTick = Time.time;
        var spikesPrefab = Resources.Load<GameObject>(Hazards.SPIKES);
        for (var i = 0; i < 3; i++) {
            var spike = Instantiate(spikesPrefab, Vector2.zero, Quaternion.identity);
            spike.SetActive(false);
            spikes.Add(spike);
        }
        leftHandClenched = Instantiate(Resources.Load<GameObject>(Bosses.FINAL_JESUS_LEFTHANDCLENCH), Vector2.zero, Quaternion.identity);
        leftHandClenched.SetActive(false);
        rightHandClenched = Instantiate(Resources.Load<GameObject>(Bosses.FINAL_JESUS_RIGHTHANDCLENCH), Vector2.zero, Quaternion.identity);
        rightHandClenched.SetActive(false);
        rainbowRay = Instantiate(Resources.Load<GameObject>(Hazards.RAINBOW_RAY), new Vector2(6.36f, -4.27f), Quaternion.identity);
        rainbowRay.SetActive(false);
        nextAct = false;
    }

    private void FixedUpdate() {
        if (leftHand != null && rightHand != null) {
            if (initialJesusY == 0f) {
                initialJesusY = transform.position.y;
            }

            var leftOk = false;
            var rightOk = false;
            var lX = leftHand.transform.position.x;
            var rX = rightHand.transform.position.x;

            if (step == 1) {
                if (lX > initialLeftX - handCenterDistance) {
                    leftHand.transform.Translate(new Vector2(-handSpeed, 0f));
                } else {
                    leftOk = true;
                }
                if (rX < initialRightX + handCenterDistance) {
                    rightHand.transform.Translate(new Vector2(handSpeed, 0f));
                } else {
                    rightOk = true;
                }

                if (rightOk && leftOk) {
                    step++;
                }
            }

            if (step == 2) {
                ClenchFists(true);
                ShootSpikes();
                step++;
            }

            if (step == 3 && Time.time - lastTick > 1f) {
                UnclenchFists();
                if (lX < initialLeftX) {
                    leftHand.transform.Translate(new Vector2(handSpeed, 0f));
                } else {
                    leftOk = true;
                }
                if (rX > initialRightX) {
                    rightHand.transform.Translate(new Vector2(-handSpeed, 0f));
                } else {
                    rightOk = true;
                }

                if (rightOk && leftOk) {
                    step++;
                }
            }

            if (step == 4) {
                ClenchFists(false);
                ShootSpikes();
                step++;
            }

            if (step == 5 && Time.time - lastTick > 1.5f) {
                HideFists();
                
                if (transform.position.y < initialJesusY + height) {
                    transform.Translate(new Vector2(0f, handSpeed));
                } else {
                    step++;
                }
            }

            if (step == 6 && Time.time - lastTick > rainbowDelay[GameState.difficulty]) {
                ShootSpikes(true);
                lastTick = Time.time;
                rainbowRay.SetActive(true);
                step++;
            }

            if (step == 7 && Time.time - lastTick > 0.5f) {
                rainbowRay.SetActive(false);
                step++;
            }

            if (step == 8 && Time.time - lastTick > rainbowDuration[GameState.difficulty]) {
                if (transform.position.y > initialJesusY) {
                    transform.Translate(new Vector2(0f, -handSpeed));
                } else {
                    step++;
                }
            }

            if (step == 9 && Time.time - lastTick > 1f) {
                UnclenchFists();
                step = 1;
            }
        }
    }

    private void ClenchFists(bool inner) {
        leftHand.SetActive(false);
        rightHand.SetActive(false);
        if (inner) {
            leftHandClenched.transform.position = leftHandClenchInner;
            rightHandClenched.transform.position = rightHandClenchInner;
        } else {
            leftHandClenched.transform.position = leftHandClenchOuter;
            rightHandClenched.transform.position = rightHandClenchOuter;
        }
        leftHandClenched.SetActive(true);
        rightHandClenched.SetActive(true);
    }

    private void UnclenchFists() {
        leftHand.SetActive(true);
        rightHand.SetActive(true);
        leftHandClenched.SetActive(false);
        rightHandClenched.SetActive(false);
    }

    private void HideFists() {
        leftHand.SetActive(false);
        rightHand.SetActive(false);
        leftHandClenched.SetActive(false);
        rightHandClenched.SetActive(false);
    }

    private void ShootSpikes() {
        ShootSpikes(false);
    }

    private void ShootSpikes(bool high) {
        for (var i = 0; i < 3; i++) {
            var pos = spikePos[i];
            if (high) {
                pos.y = -2.279077f;
            }
            spikes[i].transform.position = pos;
            spikes[i].SetActive(true);
            spikes[i].GetComponent<Spike>().Shoot();
        }
        lastTick = Time.time;
    }

    public void AssignHands(GameObject left, GameObject right) {
        leftHand = left;
        initialLeftX = leftHand.transform.position.x;
        rightHand = right;
        initialRightX = rightHand.transform.position.x;
    }

    public override void OnDeath() {
        AchievementManager.UnlockAchievement(Achievements.FINAL_BOSS_VEZ);
        if (GameState.difficulty == Difficulty.EXTREME) {
            AchievementManager.UnlockAchievement(Achievements.FINAL_BOSS_EXTREME);
        }
        if (GameState.difficulty >= Difficulty.HARD) {
            AchievementManager.UnlockAchievement(Achievements.FINAL_BOSS_HARD);
        }
        if (GameState.difficulty >= Difficulty.MEDIUM) {
            AchievementManager.UnlockAchievement(Achievements.FINAL_BOSS);
        }
        if (GameState.difficulty >= Difficulty.EASY) {
            AchievementManager.UnlockAchievement(Achievements.FINAL_BOSS_EZ);
        }
        if (onDeathEvent != null) {
            onDeathEvent();
        }
    }
}

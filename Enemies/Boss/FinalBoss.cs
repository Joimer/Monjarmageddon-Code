using UnityEngine;
using System;
using System.Collections.Generic;

public class FinalBoss : MonoBehaviour {

    // Controls the flow of the boss.
    public static int bossStage = 0;
    public GameObject bossTextGO;
    private GameObject target;
    private float prayStart = 0f;
    private RpgDialog dialog;

    // Phase 1 sky darkener and boss obstacle on the left.
    public GameObject blackBg;
    public GameObject block;
    private Vector2 blockPos = new Vector2(1.63f, 0f);
    private float nunPrevPos = 0f;

    // Platform for phases 1 and 2.
    public GameObject platform;
    private Vector2 phase2platformPos = new Vector2(6.34f, -5.63f);

    // The shooters for phase 2.
    public GameObject shooter1;

    // Position for the flame in phase 3.
    private Vector2 flamePos = new Vector2(4.12f, -6f);
    private Vector2 flamePosRight = new Vector2(8.57f, -6f);
    private GameObject flame;
    private bool odd = true;

    // Position where Jesus spawns in phase 4.
    private Vector2 jesusSpawnPos = new Vector2(6.55f, -9.28f);
    //private Vector2 jesusRightHandPos;
    //private Vector2 jesusLeftHandPos;
    private Vector2 jesusHandsPos = new Vector2(4.2f, -7.33f);

    // Controls each boss action, marking the last one (instances, attacks, so on).
    private float lastBossTick = 0f;

    // Spawning waters between phases. They fall down.
    private GameObject water;
    private Vector2 waterPos = new Vector2(3.58f, -2.18f);
    private float waterFloor = -6.16f;
    private List<GameObject> waters = new List<GameObject>();
    private float waterFallSpeed = 0.01f;

    // Boss prefabs to instance them.
    public GameObject pablo;
    private GameObject leninface;
    private GameObject marxex;
    private GameObject jesus;
    private GameObject jesusHands;
    private GameObject jesusRightHand;
    private GameObject jesusLeftHand;
    private GameObject explosion;

    private void Awake() {
        pablo.GetComponent<PeterChurches>().onDeath += onBossDeath;
        pablo.GetComponent<PeterChurches>().blackBg = blackBg;
        water = Resources.Load<GameObject>(Items.HOLY_WATER);
        leninface = Resources.Load<GameObject>(Bosses.FINAL_LENINFACE);
        marxex = Resources.Load<GameObject>(Bosses.FINAL_MARXEX);
        jesus = Resources.Load<GameObject>(Bosses.FINAL_JESUS);
        jesusHands = Resources.Load<GameObject>(Bosses.FINAL_JESUS_HANDS);
        // Reuse the same flame. MOVE into marx-x code.
        flame = Instantiate(Resources.Load<GameObject>(Hazards.FLAME), flamePos, Quaternion.identity);
        flame.SetActive(false);
        platform.GetComponent<MoveVertically>().enabled = false;
        explosion = Resources.Load<GameObject>(Hazards.EXPLOSION);
    }

    public void Start() {
        bossStage = 0;
        var player = ObjectLocator.GetPlayer();
        nunPrevPos = player.transform.position.x;
        target = ObjectLocator.GetPlayer();
    }

    public void OnTriggerEnter2D(Collider2D collision) {
        if (bossStage == 0 && collision.gameObject.GetComponent<DamageReceiver>()) {
            bossStage = 1;
            lastBossTick = Time.time;
            dialog = gameObject.AddComponent<RpgDialog>();
            dialog.onFinish += () => { bossStage++; };
            dialog.Activate(TextManager.GetText("peter_boss_text"), TextManager.GetText("peter_boss_name"), bossTextGO);
            GameState.GetInstance().isCameraLocked = true;
            platform.GetComponent<MoveVertically>().enabled = true;
            target.GetComponent<PlatformerMovement2D>().SetCanMove(false);
        }
    }

    protected void FixedUpdate() {
        if (!GameState.isGameLocked) {
            // Check water position (easier than messing with rigidbodys as I have the game right now...)
            if (waters.Count > 0) {
                UpdateWaters();
            }

            // Move the collider behind Nun for camera trickery.
            if (bossStage == 0) {
                if (target.transform.position.x > nunPrevPos) {
                    block.transform.Translate(new Vector2(target.transform.position.x - nunPrevPos, 0f));
                    nunPrevPos = target.transform.position.x;
                }
            }

            // Pablo's dialog finishes, pray ensues.
            if (bossStage == 2) {
                target.GetComponent<PlatformerMovement2D>().BossPray();
                prayStart = Time.time;
                bossStage++;
            }

            // Finish praying.
            if (bossStage == 3 && Time.time - prayStart >= 1.6f) {
                block.transform.localPosition = blockPos;
                target.GetComponent<PlatformerMovement2D>().SetCanMove(true);
                GameState.activatingBoss = false;
                GameState.bossActive = true;
                AudioManager.GetInstance().SetFinalBossSongPastIntro();
                if (pablo != null) {
                    pablo.GetComponent<BossEntity>().Activate();
                }
                lastBossTick = 0;
                bossStage++;
            }

            // Pablo is dead. The stage is increased from PeterChurches component event call.
            if (bossStage == 5) {
                Destroy(pablo);
                InstanceWaters();
                shooter1.SetActive(true);
                lastBossTick = Time.time;
                platform.GetComponent<MoveVertically>().enabled = false;
                bossStage++;
            }

            // Move the platform to the proper middle position for the next phase.
            if (bossStage == 6) {
                // Check that the platform is in the proper position.
                var p = platform.transform.position;
                if (Math.Abs(p.x - phase2platformPos.x) < 0.05f
                    && Math.Abs(p.y - phase2platformPos.y) < 0.05f
                ) {
                    bossStage++;
                } else {
                    // Uglier than hitting a father with a sweated sock
                    platform.transform.position = Vector2.MoveTowards(platform.transform.position, phase2platformPos, 0.01f);
                }
            }

            // Spawn Leninface. TODO: Add animation.
            if (bossStage == 7 && Time.time - lastBossTick > 0.8f) {
                bossStage++;
                lastBossTick = Time.time;
                leninface = Instantiate(leninface, new Vector2(9.79f, -1.5f), Quaternion.identity);
                leninface.GetComponent<Leninface>().onDeathEvent += onBossDeath;
                platform.AddComponent<MoveHorizontally>();
                target.GetComponent<PlatformerMovement2D>().SetCanMove(false);
                dialog.Activate(TextManager.GetText("lenin_boss_text"), TextManager.GetText("lenin_boss_name"), bossTextGO);
            }

            // Finish Lenin's dialog
            if (bossStage == 9) {
                target.GetComponent<PlatformerMovement2D>().SetCanMove(true);
                leninface.GetComponent<BossEntity>().Activate();
                bossStage++;
            }

            // This stage is activated by Leninface's death event.
            if (bossStage == 11) {
                bossStage++;
                InstanceWaters();
                shooter1.SetActive(false);
                lastBossTick = Time.time;
                if (GameState.difficulty == Difficulty.EXTREME) {
                    platform.GetComponent<FireCircleShot>().Shoot();
                }
                // Deactivating or deleting it if the player is on it causes the engine to make the player disappear.
                platform.AddComponent<FallDown>();
                platform.GetComponent<FallDown>().Touch();
            }

            // Spawn Marx-X and add dialog.
            if (bossStage == 12 && Time.time - lastBossTick > 0.8f) {
                bossStage++;
                lastBossTick = Time.time;
                marxex = Instantiate(marxex, new Vector2(3.54f, -1.1514f), Quaternion.identity);
                marxex.GetComponent<Marxex>().onDeathEvent += onBossDeath;
            }

            // Move Marx-Ex to screen.
            if (bossStage == 13) {
                if (marxex.transform.position.y > -4.4f) {
                    marxex.transform.Translate(new Vector2(0f, -0.07f));
                } else {
                    bossStage = 14;
                    dialog.Activate(TextManager.GetText("marxex_boss_text"), TextManager.GetText("marxex_boss_name"), bossTextGO);
                }
            }

            // Activate Marx-X
            if (bossStage == 15) {
                bossStage++;
                lastBossTick = Time.time;
                marxex.GetComponent<BossEntity>().Activate();
            }

            // Marx-X shoots flames.
            if (bossStage == 16 && Time.time - lastBossTick > 2f) {
                // Cambiar de lado
                lastBossTick = Time.time;
                flame.transform.position = odd? flamePos : flamePosRight;
                if (odd) {
                    flame.transform.localScale = new Vector2(2, 2);
                } else {
                    flame.transform.localScale = new Vector2(-2, 2);
                }
                flame.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                flame.GetComponent<Rigidbody2D>().angularVelocity = 0f;
                flame.SetActive(true);
                flame.GetComponent<Rigidbody2D>().AddForce((odd ? Vector2.right : Vector2.left) * 250f);
                flame.GetComponent<MarxFlame>().ResetFlame();
                odd = !odd;
            }

            // Accessed from Marx-X's event, prepare for the last part.
            // We add Jesus dialog.
            if (bossStage == 17) {
                bossStage++;
                lastBossTick = Time.time;
                Destroy(flame);
                dialog.Activate(TextManager.GetText("jesus_boss_text"), TextManager.GetText("jesus_boss_name"), bossTextGO);
            }

            if (bossStage == 19) {
                InstanceWaters();
                jesus = Instantiate(jesus, jesusSpawnPos, Quaternion.identity);
                jesus.GetComponent<Jesus>().onDeathEvent += onBossDeath;
                jesusHands = Instantiate(jesusHands, jesusHandsPos, Quaternion.identity);
                bossStage++;
            }

            // Jesus activation.
            if (bossStage == 20) {
                bossStage++;
                lastBossTick = Time.time;
                jesus.GetComponent<BossEntity>().Activate();
            }

            // Move Jesus up so he's slowly shown. Then, his hands.
            if (bossStage == 21) {
                if (jesus.transform.position.y < -5.28f) {
                    jesus.transform.Translate(new Vector2(0f, 0.03f));
                } else {
                    bossStage++;
                }
            }

            if (bossStage == 22) {
                if (jesusHands.transform.position.y < -5.28f) {
                    jesusHands.transform.Translate(new Vector2(0f, 0.03f));
                } else {
                    // I don't really like this but meh.
                    jesusLeftHand = jesusHands.transform.GetChild(0).gameObject;
                    jesusRightHand = jesusHands.transform.GetChild(1).gameObject;
                    jesus.GetComponent<Jesus>().AssignHands(jesusLeftHand, jesusRightHand);
                    bossStage++;
                }
            }

            // When Jesus is dead
            if (bossStage == 24) {
                Destroy(jesusHands);
                Instantiate(explosion, new Vector2(4.2f, -3.02f), Quaternion.identity);
                lastBossTick = Time.time;
                bossStage++;
            }

            if (bossStage == 25 && Time.time - lastBossTick > 0.1f) {
                Instantiate(explosion, new Vector2(7.76654f, -3.450294f), Quaternion.identity);
                lastBossTick = Time.time;
                bossStage++;
            }

            if (bossStage == 26 && Time.time - lastBossTick > 0.1f) {
                Instantiate(explosion, new Vector2(4.739439f, -5.453946f), Quaternion.identity);
                lastBossTick = Time.time;
                bossStage++;
            }

            if (bossStage == 27 && Time.time - lastBossTick > 0.1f) {
                Instantiate(explosion, new Vector2(7.290853f, -5.064747f), Quaternion.identity);
                lastBossTick = Time.time;
                bossStage++;
            }

            if (bossStage == 28 && Time.time - lastBossTick > 0.1f) {
                Instantiate(explosion, new Vector2(6.267404f, -3.695344F), Quaternion.identity);
                lastBossTick = Time.time;
                bossStage++;
            }

            if (bossStage == 29) {
                var blackBg = GameObject.Find("black_bg");
                AudioManager.GetInstance().FinalBossSongFinished();
                if (blackBg != null) {
                    blackBg.AddComponent<EndAct>();
                    blackBg.GetComponent<EndAct>().act = 3;
                }
                bossStage++;
            }
        }
    }

    private void UpdateWaters() {
        var toRemove = new List<GameObject>();
        foreach (var water in waters) {
            if (water != null && water.transform.position.y > waterFloor) {
                water.transform.position = Vector2.MoveTowards(
                    water.transform.position,
                    new Vector2(water.transform.position.x, waterFloor),
                    waterFallSpeed
                );
            } else {
                toRemove.Add(water);
            }
        }
        if (waterFallSpeed < 0.1f) {
            waterFallSpeed += 0.001f;
        }

        if (toRemove.Count == 4) {
            foreach (var water in toRemove) {
                waters.Remove(water);
            }
            waterFallSpeed = 0.01f;
        }
    }

    private void InstanceWaters() {
        waters.Add(Instantiate(water, waterPos, Quaternion.identity));
        if (GameState.difficulty != Difficulty.EXTREME) {
            waters.Add(Instantiate(water, waterPos + new Vector2(0.6f, 0f), Quaternion.identity));
            if (GameState.difficulty != Difficulty.HARD) {
                waters.Add(Instantiate(water, waterPos + new Vector2(1.2f, 0f), Quaternion.identity));
                waters.Add(Instantiate(water, waterPos + new Vector2(1.8f, 0f), Quaternion.identity));
            }
        }
    }

    // Event to be called when one of the bosses dies.
    private void onBossDeath() {
        bossStage++;
    }
}

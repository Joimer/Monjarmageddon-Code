using UnityEngine;
using System.Collections.Generic;
using System;

public class JosyfPotzedong : BossEntity {

    private class FireQueueItem {
        public List<int> fireIndexes;
        public bool activation;
        public float time;

        public FireQueueItem(List<int> fireIndexes, bool activation, float time) {
            this.fireIndexes = fireIndexes;
            this.activation = activation;
            this.time = time;
        }
    }

    // Firing towards Nun
    private GameObject target;
    private float lastShot = 0f;
    private float fireRate = 2f;
    private static readonly Dictionary<Difficulty, float> fireRates = new Dictionary<Difficulty, float>() {
        { Difficulty.VERY_EASY, 2.5f },
        { Difficulty.EASY, 2.25f },
        { Difficulty.MEDIUM, 2f },
        { Difficulty.HARD, 1.75f },
        { Difficulty.EXTREME, 1.5f }
    };

    // Rotating commie fires
    private GameObject prefab;
    private GameObject[] fires = new GameObject[12];
    private int fireIndex = 0;
    private int instancedFires = 0;
    private float lastFireInstance = 0f;
    private int fireCreationIndex = 0;
    private int firedFires = 0;
    private bool firstInstance = true;

    // Hammer projectiles.
    private GameObject[] hammers = new GameObject[4];
    private float lastHammerShot = 0f;
    private bool shootingHammers = false;
    private int pattern = 0;
    private Vector2[][] patterns = new Vector2[3][] {
        new Vector2[4] {
            new Vector2(168.32f, -15.12f), new Vector2(169.24f, -14.98f), new Vector2(168.55f, -15.79f), new Vector2(169.4f, -15.6f)
        },
        new Vector2[4] {
            new Vector2(169.85f, -14.81f), new Vector2(170.27f, -15.07f), new Vector2(170.66f, -15.33f), new Vector2(171.09f, -15.62f)
        },
        new Vector2[4] {
            new Vector2(167.78f, -14.73f), new Vector2(168.6f, -14.56f), new Vector2(168.76f, -15.28f), new Vector2(169.57f, -14.99f)
        }
    };

    // Ground flame positions
    private float yGroundPos = -18.13f;
    private float initialFireXpos = 163.54f;
    private float firePosIncrement = 0.4124f;
    private GameObject[] groundFires = new GameObject[15];

    // Ground flame blast
    private float lastFireHit = 0f;
    private int firstFlame = 0;
    private List<FireQueueItem> fireQueue = new List<FireQueueItem>();
    private float incinerationTime = 0.15f;
    private float deincinerationTime = 0.175f;

    private void Start() {
        target = ObjectLocator.GetPlayer();
        hits *= 2;
        prefab = Resources.Load<GameObject>(Hazards.JOSYF_FIRE);
        PrepareGroundFires();
        PrepareHammerObjPool();
        fireRate = fireRates[GameState.difficulty];
    }

    private void PrepareGroundFires() {
        var prefab = Resources.Load<GameObject>(Hazards.FIRE);
        for (var i = 0; i < groundFires.Length; i++) {
            groundFires[i] = Instantiate(prefab, new Vector2(initialFireXpos + firePosIncrement * i, yGroundPos), Quaternion.identity);
            groundFires[i].gameObject.SetActive(false);
        }
    }

    private void PrepareHammerObjPool() {
        var prefab = Resources.Load<GameObject>(Hazards.JOSYF_HAMMER);
        for (var i = 0; i < hammers.Length; i++) {
            hammers[i] = Instantiate(prefab, Vector2.zero, Quaternion.identity);
            hammers[i].gameObject.SetActive(false);
        }
    }

    private new void Update() {
        if (!GameState.isGameLocked && active && fireQueue.Count > 0) {
            CheckGroundFires();
        }

        base.Update();
    }

    private void CheckGroundFires() {
        var processed = new List<FireQueueItem>();
        foreach (var fireItem in fireQueue) {
            // Check if the next item should be processed.
            if (fireItem.time > Time.time) {
                continue;
            }
            // Each queue item may queue different fires to be activated or deactivated at the same time.
            foreach (var fireIndex in fireItem.fireIndexes) {
                groundFires[fireIndex].SetActive(fireItem.activation);
            }
            processed.Add(fireItem);
        }
        foreach (var fireItem in processed) {
            fireQueue.Remove(fireItem);
        }
    }

    private void FixedUpdate() {
        if (!GameState.isGameLocked && active) {
            if (instancedFires < fires.Length && Time.time - lastFireInstance > 0.33f) {
                InstanceCommieFire();
            }

            if (target != null && phase == 2 && lastShot + fireRate < Time.time) {
                Shoot();
            }

            if (GameState.difficulty != Difficulty.VERY_EASY) {
                if (target != null && phase == 2 && hits < 9 && Time.time - lastHammerShot > 3f && !shootingHammers) {
                    ShootHammers();
                }

                if (shootingHammers) {
                    MoveHammers();
                }
            }
        }
    }

    private void InstanceCommieFire() {
        lastFireInstance = Time.time;
        if (firstInstance) {
            fires[fireCreationIndex] = Instantiate(prefab, transform.position, transform.rotation, transform);
            fires[fireCreationIndex].GetComponent<JosyfFire>().SetJosyf(gameObject);
            fireCreationIndex++;
            instancedFires++;
            if (instancedFires == fires.Length) {
                firstInstance = false;
                lastShot = Time.time;
                phase = 2;
                fireCreationIndex = 0;
            }
        } else {
            if (fireCreationIndex <= 0 || fireCreationIndex > fires.Length - 1) {
                fireCreationIndex = 0;
            }
            fires[fireCreationIndex].SetActive(true);
            fireCreationIndex++;
            instancedFires++;
            if (instancedFires == fires.Length) {
                lastShot = Time.time;
            }
        }
    }

    private void ShootHammers() {
        shootingHammers = true;
        var usePattern = patterns[pattern];
        int nhammers = GameState.difficulty == Difficulty.EASY ? 2 : GameState.difficulty == Difficulty.MEDIUM ? 3 : hammers.Length;
        for (var i = 0; i < nhammers; i++) {
            hammers[i].SetActive(true);
            hammers[i].transform.position = usePattern[i];
        }
        pattern++;
        if (pattern > patterns.Length - 1) {
            pattern = 0;
        }
        lastHammerShot = Time.time;
    }

    private void MoveHammers() {
        var hammersOut = 0;
        for (var i = 0; i < hammers.Length; i++) {
            hammers[i].transform.Translate(new Vector2(-0.05f, -0.025f));
            if (hammers[i].transform.position.y < yGroundPos) {
                hammersOut++;
            }
        }
        if (hammersOut == hammers.Length) {
            shootingHammers = false;
            for (var i = 0; i < hammers.Length - 1; i++) {
                hammers[i].SetActive(false);
            }
        }
    }

    // Called from the JosyfFire prefab when the shot commie fire touches the ground.
    public void FireHasBeenHit(Vector2 hitPos) {
        // In case the event is triggered twice on a single hit.
        if (Time.time - lastFireHit <= 0.1f) {
            return;
        }
        lastFireHit = Time.time;

        // Determine which fire to start.
        firstFlame = 0;
        var currFireX = initialFireXpos;
        while (hitPos.x > currFireX && firstFlame < groundFires.Length - 1) {
            firstFlame++;
            currFireX = initialFireXpos + firePosIncrement * firstFlame;
        }

        // Here we have the starting position, activate the first flame and queue it for deincineration.
        var firstFire = groundFires[firstFlame];
        firstFire.SetActive(true);
        fireQueue.Add(
            new FireQueueItem(new List<int>(){ firstFlame }, false, Time.time + deincinerationTime)
        );
        var leftIndex = firstFlame;
        var rightIndex = firstFlame;
        for (var i = 1; i <= Math.Max(firstFlame, groundFires.Length - firstFlame); i++) {
            var inc = new List<int>();
            var deinc = new List<int>();
            var incTime = Time.time + incinerationTime * i;
            var deincTime = Time.time + incinerationTime * 2 + deincinerationTime * i;

            // In each iteration we advance to left and right as long as there's a flame in either direction.
            leftIndex--;
            if (leftIndex > 0) {
                inc.Add(leftIndex);
                deinc.Add(leftIndex);
            }

            rightIndex++;
            if (rightIndex < groundFires.Length) {
                inc.Add(rightIndex);
                deinc.Add(rightIndex);
            }

            // Add to queue the flame indexes to be lit and unlit in the proper time.
            fireQueue.Add(new FireQueueItem(inc, true, incTime));
            fireQueue.Add(new FireQueueItem(deinc, false, deincTime));
        }
    }

    // Gets the shield fires and shoots toward nun.
    // The commie fire upon hitting the ground causes a spreading flame.
    private void Shoot() {
        var fire = fires[fireIndex];
        fireIndex++;
        if (fireIndex > fires.Length - 1) {
            fireIndex = 0;
        }
        var fireComp = fire.GetComponent<JosyfFire>();
        fireComp.ShootTowards(target.transform.position);
        lastShot = Time.time;
        firedFires++;
        if (firedFires == fires.Length) {
            instancedFires = 0;
            firedFires = 0;
        }
    }

    public override void OnDeath() {
        AchievementManager.UnlockAchievement(Achievements.BOSS_COMMIE_HQ_VEZ);
        if (GameState.difficulty == Difficulty.EXTREME) {
            AchievementManager.UnlockAchievement(Achievements.BOSS_COMMIE_HQ_EXTREME);
        }
        if (GameState.difficulty >= Difficulty.HARD) {
            AchievementManager.UnlockAchievement(Achievements.BOSS_COMMIE_HQ_HARD);
        }
        if (GameState.difficulty >= Difficulty.MEDIUM) {
            AchievementManager.UnlockAchievement(Achievements.BOSS_COMMIE_HQ);
        }
        if (GameState.difficulty >= Difficulty.EASY) {
            AchievementManager.UnlockAchievement(Achievements.BOSS_COMMIE_HQ_EZ);
        }
    }
}

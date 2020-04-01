using UnityEngine;
using System.Collections.Generic;

public class NightQueen : BossEntity {
    
    // Drag Queen attacks one of the 10 platforms every few seconds.
    // To avoid the player being unable to jump and thus unfairly losing, there are random sequences, all intercalating odd and even numbers.
    private List<int> sequence;
    private const float yPosition = -23.8f;
    private const float yFloor = -24.63f;
    // Every subsequent x pos is this + n pos
    private const float xLeftPosition = 196.325f;
    private int step = 1;
    private bool isInPosition = false;
    private bool goingDown = false;
    private float nextPos;
    private bool isMoving = false;
    private float fireRate = 1f;
    private float lastShot = 0f;
    private float shootingSpeed = 100f;
    private GameObject fireballPrefab;
    private Flipper flipper;
    private ObjectPool fireballPool;
    private static readonly Dictionary<Difficulty, float> fireRates = new Dictionary<Difficulty, float>() {
        { Difficulty.VERY_EASY, 1.5f },
        { Difficulty.EASY, 1.33f },
        { Difficulty.MEDIUM, 1f },
        { Difficulty.HARD, 0.8f },
        { Difficulty.EXTREME, 0.66f }
    };

    private void Awake() {
        var random = Random.Range(0, 4);
        List<int>[] sequences = new List<int>[5];
        sequences[0] = new List<int>() { 4, 9, 3, 7, 5, 0, 8, 2, 6, 1 };
        sequences[1] = new List<int>() { 4, 2, 6, 0, 8, 7, 9, 1, 5, 3 };
        sequences[2] = new List<int>() { 4, 5, 1, 7, 3, 6, 8, 9, 0, 2 };
        sequences[3] = new List<int>() { 4, 0, 6, 2, 8, 3, 9, 5, 7, 1 };
        sequences[4] = new List<int>() { 4, 9, 7, 1, 5, 2, 3, 0, 8, 6 };
        sequence = sequences[random];
        fireballPrefab = Resources.Load<GameObject>(Hazards.FIREBALL);
        flipper = GetComponent<Flipper>();
        var fireball = Resources.Load<GameObject>(Hazards.FIREBALL);
        fireballPool = new ObjectPool(fireball, 12);
    }

    public override void Activate() {
        GameState.reachedBoss = true;
        initialPosition = transform.position;
        lastShot = Time.time;
        fireRate = fireRates[GameState.difficulty];
        if (GameState.difficulty == Difficulty.VERY_EASY) {
            invulnerableTime = 0.75f;
            shootingSpeed = 80f;
        }
        active = true;
    }

    private void FixedUpdate() {
        if (!GameState.isGameLocked && active) {
            // He's moving from starting point to the next one.
            if (!isInPosition && !isMoving) {
                nextPos = xLeftPosition + sequence[step];
                isMoving = true;
                var isCurrentlyMovingLeft = isMovingLeft;
                isMovingLeft = nextPos < transform.position.x;
                if (isCurrentlyMovingLeft != isMovingLeft) {
                    flipper.Flip();
                }
            }

            if (isMoving && transform.position.x == nextPos) {
                isMoving = false;
                goingDown = true;
            }

            if (!isInPosition && isMoving) {
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(nextPos, transform.position.y), 0.03f);
            }

            if (goingDown) {
                if (transform.position.y > yFloor) {
                    transform.Translate(new Vector2(0f, -0.03f));
                } else {
                    goingDown = false;
                    isInPosition = true;
                }
            }

            if (isInPosition && !goingDown) {
                if (transform.position.y < yPosition) {
                    transform.Translate(new Vector2(0f, 0.03f));
                } else {
                    isInPosition = false;
                    isMoving = false;
                    step++;
                    if (step == 10) {
                        step = 0;
                    }
                }
            }

            // Shooting
            if (Time.time > lastShot + fireRate) {
                lastShot = Time.time;
                var fireBall = GetNextFireball();
                fireBall.GetComponent<Rigidbody2D>().AddForce(Vector2.right * shootingSpeed);
                fireBall = GetNextFireball();
                fireBall.GetComponent<Rigidbody2D>().AddForce(Vector2.left * shootingSpeed);
                fireBall = GetNextFireball();
                fireBall.GetComponent<Rigidbody2D>().AddForce(Vector2.down * shootingSpeed);
                if (GameState.difficulty != Difficulty.VERY_EASY) {
                    fireBall = GetNextFireball();
                    fireBall.GetComponent<Rigidbody2D>().AddForce(new Vector2(0.66f, -0.66f) * shootingSpeed);
                    fireBall = GetNextFireball();
                    fireBall.GetComponent<Rigidbody2D>().AddForce(new Vector2(-0.66f, -0.66f) * shootingSpeed);
                }
            }
        }
    }

    public override void OnDeath() {
        AchievementManager.UnlockAchievement(Achievements.BOSS_NIGHT_BAR_VEZ);
        if (GameState.difficulty == Difficulty.EXTREME) {
            AchievementManager.UnlockAchievement(Achievements.BOSS_NIGHT_BAR_EXTREME);
        }
        if (GameState.difficulty >= Difficulty.HARD) {
            AchievementManager.UnlockAchievement(Achievements.BOSS_NIGHT_BAR_HARD);
        }
        if (GameState.difficulty >= Difficulty.MEDIUM) {
            AchievementManager.UnlockAchievement(Achievements.BOSS_NIGHT_BAR);
        }
        if (GameState.difficulty >= Difficulty.EASY) {
            AchievementManager.UnlockAchievement(Achievements.BOSS_NIGHT_BAR_EZ);
        }
    }

    private GameObject GetNextFireball() {
        var fireball = fireballPool.RetrieveNext();
        fireball.transform.position = transform.position;
        fireball.SetActive(true);
        fireball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        fireball.GetComponent<Rigidbody2D>().angularVelocity = 0f;
        return fireball;
    }
}

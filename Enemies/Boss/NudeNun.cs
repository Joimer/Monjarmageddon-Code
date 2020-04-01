using System.Collections.Generic;
using UnityEngine;

public class NudeNun : BossEntity {

    private float fireRate = 1f;
    private float lastShot = 0f;
    private float shootingSpeed = 100f;
    private int pattern = 1;
    private Animator animator;
    private ObjectPool fireballPool;
    private Flipper flipper;
    private static readonly Dictionary<Difficulty, float> fireRates = new Dictionary<Difficulty, float>() {
        { Difficulty.VERY_EASY, 1.5f },
        { Difficulty.EASY, 1.33f },
        { Difficulty.MEDIUM, 1f },
        { Difficulty.HARD, 0.8f },
        { Difficulty.EXTREME, 0.66f }
    };

    private void Awake() {
        animator = GetComponent<Animator>();
        var fireball = Resources.Load<GameObject>(Hazards.FIREBALL);
        fireballPool = new ObjectPool(fireball, 12);
        flipper = GetComponent<Flipper>();
        fireRate = fireRates[GameState.difficulty];
    }

    public override void Activate() {
        GameState.reachedBoss = true;
        initialPosition = transform.position;
        lastShot = Time.time;
        if (GameState.difficulty != Difficulty.VERY_EASY) {
            gameObject.AddComponent<CommieFire>();
        } else {
            shootingSpeed = 80f;
        }
        active = true;
        animator.Play(Animations.NUDE_NUN_MOVING);
    }

    private void FixedUpdate() {
        if (!GameState.isGameLocked && active) {
            if (isMovingLeft) {
                if (transform.position.x > initialPosition.x - 5f) {
                    transform.Translate(new Vector2(-0.03f, 0f));
                } else {
                    isMovingLeft = !isMovingLeft;
                    flipper.Flip();
                }
            }

            if (!isMovingLeft) {
                if (transform.position.x < initialPosition.x + 1f) {
                    transform.Translate(new Vector2(0.03f, 0f));
                } else {
                    isMovingLeft = !isMovingLeft;
                    flipper.Flip();
                }
            }

            // Shooting
            if (Time.time > lastShot + fireRate) {
                animator.Play(Animations.NUDE_NUN_CROSS);
                lastShot = Time.time;
                if (GameState.difficulty == Difficulty.VERY_EASY || pattern == 1) {
                    var fireBall = GetNextFireball();
                    fireBall.GetComponent<Rigidbody2D>().AddForce(Vector2.right * shootingSpeed);
                    fireBall = GetNextFireball();
                    fireBall.GetComponent<Rigidbody2D>().AddForce(Vector2.left * shootingSpeed);
                    fireBall = GetNextFireball();
                    fireBall.GetComponent<Rigidbody2D>().AddForce(Vector2.up * shootingSpeed);
                    fireBall = GetNextFireball();
                    fireBall.GetComponent<Rigidbody2D>().AddForce(Vector2.down * shootingSpeed);
                    pattern = 2;
                } else {
                    var fireBall = GetNextFireball();
                    fireBall.GetComponent<Rigidbody2D>().AddForce(new Vector2(0.66f, 0.66f) * shootingSpeed);
                    fireBall = GetNextFireball();
                    fireBall.GetComponent<Rigidbody2D>().AddForce(new Vector2(0.66f, -0.66f) * shootingSpeed);
                    fireBall = GetNextFireball();
                    fireBall.GetComponent<Rigidbody2D>().AddForce(new Vector2(-0.66f, 0.66f) * shootingSpeed);
                    fireBall = GetNextFireball();
                    fireBall.GetComponent<Rigidbody2D>().AddForce(new Vector2(-0.66f, -0.66f) * shootingSpeed);
                    pattern = 1;
                }
                animator.Play(Animations.NUDE_NUN_MOVING);
            }
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

    public override void OnDeath() {
        AchievementManager.UnlockAchievement(Achievements.BOSS_MONASTERY_VEZ);
        if (GameState.difficulty == Difficulty.EXTREME) {
            AchievementManager.UnlockAchievement(Achievements.BOSS_MONASTERY_EXTREME);
        }
        if (GameState.difficulty >= Difficulty.HARD) {
            AchievementManager.UnlockAchievement(Achievements.BOSS_MONASTERY_HARD);
        }
        if (GameState.difficulty >= Difficulty.MEDIUM) {
            AchievementManager.UnlockAchievement(Achievements.BOSS_MONASTERY);
        }
        if (GameState.difficulty >= Difficulty.EASY) {
            AchievementManager.UnlockAchievement(Achievements.BOSS_MONASTERY_EZ);
        }
    }
}

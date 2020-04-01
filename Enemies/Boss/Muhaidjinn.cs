using System.Collections.Generic;
using UnityEngine;

public class Muhaidjinn : BossEntity {

    private GameObject target;
    // Counts how many turns it's done on the current trajectory.
    private int step = 0;
    // Whether it's going left or right.
    private bool going = true;
    // The speed at which the boss advances.
    private const float speed = 0.025f;
    // Even or odd turn. Even turns are for vertical movement, odd turns are for horizontal.
    private bool even = true;
    // If it's going vertically up or down.
    private bool goingUp = true;
    // Set in the position in which the boss spawns.
    private Vector2 currentPoint = new Vector2(196.92f, -2.84f);
    // The initial nextPoint is the 2nd point in the boss movement, initial position plus its vertical movement.
    private Vector2 nextPoint;
    // Counter to know when the vertical movement switches from positive to negative and otherwise.
    private int nextSwitch = 0;
    // The horizontal distance between points in the same X axis.
    private const float xDistance = 1.76f;
    // The vertical distance between points in the same Y axis.
    private const float yDistance = 3.16f;
    // Shooting.
    private GameObject[] bullets = new GameObject[24];
    private int bulletIndex = 0;
    private float lastShot = 0f;
    private float fireRate = 1f;
    private float shootingSpeed = GameplayValues.GetEnemyShootSpeed();
    private static readonly Dictionary<Difficulty, float> fireRates = new Dictionary<Difficulty, float>() {
        { Difficulty.VERY_EASY, 1.4f },
        { Difficulty.EASY, 1.25f },
        { Difficulty.MEDIUM, 1f },
        { Difficulty.HARD, 0.75f },
        { Difficulty.EXTREME, 0.6f }
    };
    // Bomb planting.
    private GameObject[] bombs = new GameObject[6];
    private int bombIndex = 0;
    private int dropped = 0;
    private float nextCooldown = 0f;
    private float timeBetweenBombs = 0.25f;
    // Less time is easier because the explosions are closer so there's more space to move.
    private static readonly Dictionary<Difficulty, float> bombRates = new Dictionary<Difficulty, float>() {
        { Difficulty.VERY_EASY, 0.15f },
        { Difficulty.EASY, 0.20f },
        { Difficulty.MEDIUM, 0.25f },
        { Difficulty.HARD, 0.33f },
        { Difficulty.EXTREME, 0.5f }
    };
    // Animation.
    private Flipper flipper;

    public void Awake() {
        PrepareBulletPool();
        PrepareBombPool();
        flipper = GetComponent<Flipper>();
    }

    public void Start() {
        target = ObjectLocator.GetPlayer();
        nextPoint = new Vector2(currentPoint.x, currentPoint.y + yDistance);
    }

    public override void Activate() {
        GameState.reachedBoss = true;
        initialPosition = transform.position;
        lastShot = Time.time;
        nextCooldown = Time.time;
        timeBetweenBombs = bombRates[GameState.difficulty];
        fireRate = fireRates[GameState.difficulty];
        if (GameState.difficulty == Difficulty.VERY_EASY) {
            invulnerableTime = 0.75f;
        }
        active = true;
    }

    private void PrepareBombPool() {
        var prefab = Resources.Load<GameObject>(Hazards.DESERT_BOMB);
        for (var i = 0; i < bombs.Length; i++) {
            bombs[i] = Instantiate(prefab, new Vector2(0f, 0f), Quaternion.identity);
            bombs[i].gameObject.SetActive(false);
        }
    }

    private void PrepareBulletPool() {
        var prefab = Resources.Load<GameObject>(Hazards.FIREBALL);
        for (var i = 0; i < bullets.Length; i++) {
            bullets[i] = Instantiate(prefab, new Vector2(0f, 0f), Quaternion.identity);
            bullets[i].gameObject.SetActive(false);
        }
    }

    public void FixedUpdate() {
        if (!GameState.isGameLocked && active) {
            var xMoveSpeed = even ? 0f : (going? -speed : speed);
            var yMoveSpeed = even ? (goingUp ? speed : -speed) : 0f;
            if (nextPoint.x > currentPoint.x && transform.position.x < nextPoint.x
                || nextPoint.x < currentPoint.x && transform.position.x > nextPoint.x
                || nextPoint.y > currentPoint.y && transform.position.y < nextPoint.y
                || nextPoint.y < currentPoint.y && transform.position.y > nextPoint.y
            ) {
                transform.Translate(new Vector2(xMoveSpeed, yMoveSpeed));
            } else {
                // Calculate the next point and switch variables for the movement update.
                even = !even;
                nextSwitch++;
                if (nextSwitch == 2) {
                    nextSwitch = 0;
                    goingUp = !goingUp;
                }
                currentPoint = nextPoint;
                step++;
                if (step == 5) {
                    step = going ? 2 : 0;
                    going = !going;
                }
                var nextPointX = currentPoint.x + (even ? 0f : (going ? -xDistance : xDistance));
                if (flipper.lookingRight && nextPointX < currentPoint.x) {
                    flipper.Flip();
                }
                nextPoint = new Vector2(
                    nextPointX,
                    currentPoint.y + (even ? (goingUp ? yDistance : -yDistance) : 0f)
                );
            }

            if (Time.time - nextCooldown > timeBetweenBombs) {
                DropBomb(transform.position);
            }

            if (target != null && lastShot + fireRate < Time.time) {
                Shoot();
            }
        }
    }

    private void DropBomb(Vector2 fromWhere) {
        const float bottom = -2.8f;
        const float top = 0.31f;
        const float rightmost = 197f;
        const float leftmost = 193.2f;
        var dirs = 0 + (fromWhere.y < top ? 1 : 0) + (fromWhere.y > bottom ? 2 : 0) + (fromWhere.x < rightmost ? 4 : 0) + (fromWhere.x > leftmost ? 8 : 0);
        var dbomb = GetNextBomb();
        dbomb.GetComponent<DesertBomb>().Activate((Direction) dirs);
        dropped++;
        if (dropped == 3) {
            dropped = 0;
            nextCooldown = Time.time + timeBetweenBombs * 3;
        } else {
            nextCooldown = Time.time + timeBetweenBombs;
        }
    }

    // Get next bomb GameObject from the pool.
    private GameObject GetNextBomb() {
        var bomb = bombs[bombIndex];
        bomb.transform.position = gameObject.transform.position;
        bomb.SetActive(true);
        bombIndex++;
        if (bombIndex + 1 == bombs.Length) {
            bombIndex = 0;
        }
        return bomb;
    }

    private GameObject GetNextBullet() {
        var bullet = bullets[bulletIndex];
        bullet.transform.position = gameObject.transform.position;
        bullet.SetActive(true);
        bulletIndex++;
        if (bulletIndex + 1 == bullets.Length) {
            bulletIndex = 0;
        }
        return bullet;
    }

    private void Shoot() {
        if (target != null) {
            lastShot = Time.time;
            // TODO: Refactor vector2s
            var fireBall = GetNextBullet();
            fireBall.GetComponent<Rigidbody2D>().AddForce(Vector2.right * shootingSpeed);
            fireBall = GetNextBullet();
            fireBall.GetComponent<Rigidbody2D>().AddForce(Vector2.left * shootingSpeed);
            fireBall = GetNextBullet();
            fireBall.GetComponent<Rigidbody2D>().AddForce(Vector2.up * shootingSpeed);
            fireBall = GetNextBullet();
            fireBall.GetComponent<Rigidbody2D>().AddForce(Vector2.down * shootingSpeed);
            if (GameState.difficulty != Difficulty.VERY_EASY) {
                fireBall = GetNextBullet();
                fireBall.GetComponent<Rigidbody2D>().AddForce(new Vector2(0.66f, -0.66f) * shootingSpeed);
                fireBall = GetNextBullet();
                fireBall.GetComponent<Rigidbody2D>().AddForce(new Vector2(-0.66f, -0.66f) * shootingSpeed);
                fireBall = GetNextBullet();
                fireBall.GetComponent<Rigidbody2D>().AddForce(new Vector2(0.66f, 0.66f) * shootingSpeed);
                fireBall = GetNextBullet();
                fireBall.GetComponent<Rigidbody2D>().AddForce(new Vector2(-0.66f, 0.66f) * shootingSpeed);
            }
        }
    }

    public override void OnDeath() {
        AchievementManager.UnlockAchievement(Achievements.BOSS_DESERT_VEZ);
        if (GameState.difficulty == Difficulty.EXTREME) {
            AchievementManager.UnlockAchievement(Achievements.BOSS_DESERT_EXTREME);
        }
        if (GameState.difficulty >= Difficulty.HARD) {
            AchievementManager.UnlockAchievement(Achievements.BOSS_DESERT_HARD);
        }
        if (GameState.difficulty >= Difficulty.MEDIUM) {
            AchievementManager.UnlockAchievement(Achievements.BOSS_DESERT);
        }
        if (GameState.difficulty >= Difficulty.EASY) {
            AchievementManager.UnlockAchievement(Achievements.BOSS_DESERT_EZ);
        }
    }
}

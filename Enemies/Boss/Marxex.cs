using System;
using System.Collections.Generic;
using UnityEngine;

public class Marxex : BossEntity {

    public event Action onDeathEvent;
    private int pos = 0;
    private float minY = -6.07f;
    private float leftX = 3.54f;
    private float rightX = 9.16f;
    private Dictionary<Difficulty, float> timesBetweenBombs = new Dictionary<Difficulty, float>() {
        { Difficulty.VERY_EASY, 1.25f },
        { Difficulty.EASY, 1f },
        { Difficulty.MEDIUM, 0.75f },
        { Difficulty.HARD, 0.5f },
        { Difficulty.EXTREME, 0.4f },
    };
    private GameObject[] bombs = new GameObject[6];
    private int bombIndex = 0;
    private float lastBomb;
    private float horizontalSpeed = 0.02f;
    private float verticalSpeed = 0.017f;
    private float midWay;
    private float lastDownwardsShot = 0f;
    private Dictionary<Difficulty, float> timesBetweenDownwards = new Dictionary<Difficulty, float>() {
        { Difficulty.VERY_EASY, 1.75f },
        { Difficulty.EASY, 1.5f },
        { Difficulty.MEDIUM, 1.25f },
        { Difficulty.HARD, 1.15f },
        { Difficulty.EXTREME, 1f },
    };
    private float downwardsSpeed = 100f;
    private Flipper flipper;
    private GameObject target;

    public void Awake() {
        PrepareBombPool();
        midWay = leftX + (rightX - leftX) / 2;
        flipper = GetComponent<Flipper>();
        target = ObjectLocator.GetPlayer();
    }

    private void Start() {
        nextAct = false;
    }

    private void FixedUpdate() {
        if (!GameState.isGameLocked && active) {
            if (pos == 0) {
                var ymov = 0f;
                if (transform.position.y > minY && transform.position.x < midWay) {
                    ymov = -verticalSpeed;
                } else if (transform.position.y <= 3.2f && transform.position.x > midWay) {
                    ymov = verticalSpeed;
                }
                transform.Translate(new Vector2(horizontalSpeed, ymov));

                if (transform.position.x >= rightX) {
                    pos = 1;
                    lastDownwardsShot = Time.time;
                }
            }
            
            if (pos == 1) {
                if (transform.position.x > leftX) {
                    transform.Translate(new Vector2(-horizontalSpeed, 0f));
                    if (Time.time - lastDownwardsShot > timesBetweenDownwards[GameState.difficulty]) {
                        DropShot();
                    }
                } else {
                    pos = 2;
                }
            }

            if (pos == 2) {
                if (transform.position.x < rightX) {
                    transform.Translate(new Vector2(horizontalSpeed, 0f));
                    if (Time.time - lastDownwardsShot > timesBetweenDownwards[GameState.difficulty]) {
                        DropShot();
                    }
                } else {
                    pos = 3;
                }
            }

            if (pos == 3) {
                var ymov = 0f;
                if (transform.position.y > minY && transform.position.x > midWay) {
                    ymov = -verticalSpeed;
                } else if (transform.position.y <= 3.2f && transform.position.x < midWay) {
                    ymov = verticalSpeed;
                }
                transform.Translate(new Vector2(-horizontalSpeed, ymov));

                if (transform.position.x <= leftX) {
                    pos = 0;
                }
            }

            if (pos != 1 && pos != 2 && Time.time - lastBomb >= timesBetweenBombs[GameState.difficulty]) {
                var dbomb = GetNextBomb();
                dbomb.GetComponent<DesertBomb>().Activate(Direction.ALL);
                lastBomb = Time.time;
            }

            if (flipper.lookingRight && target.transform.position.x < transform.position.x) {
                flipper.Flip();
            }

            if (!flipper.lookingRight && target.transform.position.x > transform.position.x) {
                flipper.Flip();
            }
        }
    }

    // Refactor to use events better
    public override void OnDeath() {
        if (onDeathEvent != null) {
            onDeathEvent();
        }
    }

    // Duplicated code, fix?
    private void PrepareBombPool() {
        var prefab = Resources.Load<GameObject>(Hazards.DESERT_BOMB);
        for (var i = 0; i < bombs.Length; i++) {
            bombs[i] = Instantiate(prefab, new Vector2(0f, 0f), Quaternion.identity);
            bombs[i].gameObject.SetActive(false);
        }
    }

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

    private void DropShot() {
        var shot = GameResources.GetInstance().GetFireball();
        shot.transform.position = transform.position;
        shot.GetComponent<Rigidbody2D>().AddForce(Vector2.down * downwardsSpeed);
        shot.AddComponent<FireCircleShot>().FutureShoot(Time.time + 0.65f);
        lastDownwardsShot = Time.time;
    }
}

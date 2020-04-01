using UnityEngine;
using System;
using System.Collections.Generic;

public class PeterChurches : BossEntity {

    [HideInInspector]
    public event Action onDeath;
    [HideInInspector]
    public GameObject blackBg;
    private GameObject[] redStars = new GameObject[10];
    private Vector2[] redStarPos = new Vector2[9] {
        new Vector2(3.8f, -3.3f),
        new Vector2(4.3f, -2.6f),
        new Vector2(4.8f, -3.3f),
        new Vector2(6f, -3.3f),
        new Vector2(6.5f, -2.6f),
        new Vector2(7f, -3.3f),
        new Vector2(8.2f, -3.3f),
        new Vector2(8.7f, -2.6f),
        new Vector2(9.2f, -3.3f)
    };
    private int step = 7;
    private float speed = 0.05f;
    private bool shooting = false;
    private float lastCircle = 0f;
    private Dictionary<Difficulty, float> circleDelays = new Dictionary<Difficulty, float>() {
        { Difficulty.VERY_EASY, 10f },
        { Difficulty.EASY, 1.75f },
        { Difficulty.MEDIUM, 1.5f },
        { Difficulty.HARD, 1.25f },
        { Difficulty.EXTREME, 1f },
    };
    private List<GameObject> firstFireBalls;
    private bool secondFireBalls = false;
    private float explodeFireBalls = 0f;

    private void Awake() {
        PrepareStarPool();
        nextAct = false;
    }

    private void Start() {
        blackBg.AddComponent<FadeIn>();
    }

    public override void Activate() {
        active = true;
        if (GameState.difficulty == Difficulty.VERY_EASY) {
            invulnerableTime = 0.75f;
        }
    }

    private void FixedUpdate() {
        if (!GameState.isGameLocked && active) {
            if (secondFireBalls && Time.time >= explodeFireBalls) {
                secondFireBalls = false;
                foreach (GameObject fb in firstFireBalls) {
                    fb.AddComponent<FireCircleShot>().Shoot();
                }
            }

            if (!shooting && (step == 1 || step == 4)) {
                if (transform.localPosition.y > 0f) {
                    transform.Translate(new Vector2(0f, -speed));
                    CircleShoot();
                } else {
                    step++;
                }
            }

            if (step == 2) {
                if (transform.localPosition.x > 0f) {
                    transform.Translate(new Vector2(-speed, 0f));
                    CircleShoot();
                } else {
                    step = 3;
                }
            }

            if (step == 3 || step == 6) {
                if (transform.localPosition.y < 2f) {
                    transform.Translate(new Vector2(0f, speed));
                    CircleShoot();
                } else {
                    step++;
                    Shoot();
                }
            }

            if (step == 5) {
                if (transform.localPosition.x < 4.6f) {
                    transform.Translate(new Vector2(speed, 0f));
                    CircleShoot();
                } else {
                    step = 6;
                }
            }

            if (step == 7) {
                step = 1;
                Shoot();
            }
        }
    }

    private void Shoot() {
        if (active) {
            shooting = true;
            blackBg.GetComponent<FadeIn>().Activate();
            blackBg.GetComponent<FadeIn>().onFadeInEnd += SpawnStars;
            // Add circle cooldown to give time.
            lastCircle = Time.time;
        }
    }

    private void CircleShoot() {
        if (active && !shooting && Time.time - lastCircle >= circleDelays[GameState.difficulty]) {
            var fcs = GetComponent<FireCircleShot>();
            fcs.SetBallType(Hazards.POISON_BALL);
            firstFireBalls = fcs.Shoot();
            lastCircle = Time.time;
            secondFireBalls = true;
            explodeFireBalls = Time.time + 1f;
        }
    }

    private void SpawnStars() {
        for (var i = 0; i < 9; i++) {
            if (GameState.difficulty == Difficulty.VERY_EASY && i % 2 == 0
                || GameState.difficulty == Difficulty.EASY && i % 3 == 0) {
                continue;
            }
            ActivateStar(i);
        }
        var color = blackBg.GetComponent<SpriteRenderer>().color;
        color.a = 0;
        blackBg.GetComponent<SpriteRenderer>().color = color;
        blackBg.GetComponent<FadeIn>().ResetState();
        shooting = false;
    }

    private void ActivateStar(int i) {
        redStars[i].SetActive(true);
        redStars[i].transform.position = redStarPos[i];
        var target = ObjectLocator.GetPlayer();
        redStars[i].GetComponent<RedStar>().ShootTowards(target.transform.position);
    }

    // Refactor to use events better
    public override void OnDeath() {
        if (onDeath != null) {
            onDeath();
        }
    }

    private void PrepareStarPool() {
        var redStar = Resources.Load<GameObject>(Hazards.REDSTAR);
        for (var i = 0; i < 9; i++) {
            redStars[i] = Instantiate(redStar, Vector2.zero, Quaternion.identity).gameObject;
        }
    }
}

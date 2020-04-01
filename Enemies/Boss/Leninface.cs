using UnityEngine;
using System;
using System.Collections.Generic;

public class Leninface : BossEntity {

    private GameObject target;
    public event Action onDeathEvent;
    private Flipper flipper;
    private Dictionary<Difficulty, float> speed = new Dictionary<Difficulty, float>() {
        { Difficulty.VERY_EASY, 0.01f },
        { Difficulty.EASY, 0.015f },
        { Difficulty.MEDIUM, 0.02f },
        { Difficulty.HARD, 0.025f },
        { Difficulty.EXTREME, 0.03f }
    };

    private void Start() {
        target = ObjectLocator.GetPlayer();
        nextAct = false;
        flipper = GetComponent<Flipper>();
    }

    private void FixedUpdate() {
        if (!GameState.isGameLocked && active) {
            if (flipper.lookingRight && target.transform.position.x > transform.position.x) {
                flipper.Flip();
            }

            if (!flipper.lookingRight && target.transform.position.x < transform.position.x) {
                flipper.Flip();
            }

            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed[GameState.difficulty]);
        }
    }

    // Refactor to use events better
    public override void OnDeath() {
        if (onDeathEvent != null) {
            onDeathEvent();
        }
    }
}

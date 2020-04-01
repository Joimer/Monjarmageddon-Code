using System.Collections.Generic;
using UnityEngine;

public class ChemicalShooter : MonoBehaviour {

    private float fireRate = 2.3f;
    private float lastShot = 0f;
    private bool activating = false;
    private float activateTime = 0f;
    private bool active = false;
    private GameObject target;
    public float activeDistance = 2.3f;
    public Direction direction;
    public float shootDelay = 0.5f;
    private float shootSpeed = 100f;
    private readonly Dictionary<Difficulty, float> fireRates = new Dictionary<Difficulty, float>() {
        { Difficulty.VERY_EASY, 2.66f },
        { Difficulty.EASY, 2.5f },
        { Difficulty.MEDIUM, 2.3f },
        { Difficulty.HARD, 2f },
        { Difficulty.EXTREME, 1.5f }
    };

    private void Start() {
        target = ObjectLocator.GetPlayer();
        fireRate = fireRates[GameState.difficulty];
    }

    private void FixedUpdate() {
        if (!GameState.isGameLocked && target && target.GetComponent<Transform>() != null) {
            var distance = Vector2.Distance(transform.position, target.transform.position);

            if (active && distance > activeDistance) {
                Deactive();
            }

            if (!active && !activating && distance < activeDistance) {
                Activate();
            }

            if (activating && Time.time > activateTime + shootDelay) {
                activating = false;
                active = true;
            }

            if (active && lastShot + fireRate < Time.time) {
                Shoot();
            }
        }
    }

    private void Activate() {
        activating = true;
        activateTime = Time.time;
    }

    private void Deactive() {
        activating = false;
        activateTime = 0f;
        active = false;
    }

    private void Shoot() {
        if (Vector2.Distance(transform.position, target.transform.position) <= activeDistance) {
            lastShot = Time.time;
            var yOffset = 0f;
            var xOffset = 0f;
            var force = Vector2.up;
            if (direction == Direction.UP) {
                yOffset = 0.1f;
            }
            if (direction == Direction.DOWN) {
                force = Vector2.down;
                yOffset = -0.1f;
            }
            if (direction == Direction.LEFT) {
                force = Vector2.left;
                xOffset = -0.15f;
            }
            if (direction == Direction.RIGHT) {
                force = Vector2.right;
                xOffset = 0.15f;
            }
            var pbr = GameResources.GetInstance().GetPoisonball();
            pbr.transform.position = transform.position + new Vector3(xOffset, yOffset, 0f);
            pbr.GetComponent<EnemyProjectile>().ignoreCollisions = false;
            pbr.SetActive(true);
            pbr.GetComponent<Rigidbody2D>().AddForce(force * shootSpeed);
        }
    }
}

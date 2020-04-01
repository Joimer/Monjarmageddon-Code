using System.Collections.Generic;
using UnityEngine;

public class FireCircleShot : MonoBehaviour {

    private float shootingSpeed = 100f;
    private int mode = 0;
    private float explodeTime = 0f;
    private bool activeFuture = false;

    public void Update() {
        if (!GameState.isGameLocked && activeFuture && Time.time >= explodeTime) {
            activeFuture = false;
            Shoot();
        }
    }

    public void SetBallType(string type) {
        if (type == Hazards.POISON_BALL) {
            mode = 1;
        } else {
            mode = 0;
        }
    }

    public List<GameObject> Shoot(Vector2 pos) {
        var shotBalls = new List<GameObject>();
        // Right
        var fireBall = PrepareBall(pos);
        fireBall.GetComponent<Rigidbody2D>().AddForce(new Vector2(1f, 0f) * shootingSpeed);
        shotBalls.Add(fireBall);
        // Left
        fireBall = PrepareBall(pos);
        fireBall.GetComponent<Rigidbody2D>().AddForce(new Vector2(-1f, 0f) * shootingSpeed);
        shotBalls.Add(fireBall);
        // Down
        fireBall = PrepareBall(pos);
        fireBall.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, -1f) * shootingSpeed);
        shotBalls.Add(fireBall);
        // Up
        fireBall = PrepareBall(pos);
        fireBall.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 1f) * shootingSpeed);
        shotBalls.Add(fireBall);
        // Left + up
        fireBall = PrepareBall(pos);
        fireBall.GetComponent<Rigidbody2D>().AddForce(new Vector2(-0.66f, 0.66f) * shootingSpeed);
        shotBalls.Add(fireBall);
        // Right + up
        fireBall = PrepareBall(pos);
        fireBall.GetComponent<Rigidbody2D>().AddForce(new Vector2(0.66f, 0.66f) * shootingSpeed);
        shotBalls.Add(fireBall);
        if (GameState.difficulty != Difficulty.VERY_EASY) {
            // Right + down
            fireBall = PrepareBall(pos);
            fireBall.GetComponent<Rigidbody2D>().AddForce(new Vector2(0.66f, -0.66f) * shootingSpeed);
            shotBalls.Add(fireBall);
            // Left + down
            fireBall = PrepareBall(pos);
            fireBall.GetComponent<Rigidbody2D>().AddForce(new Vector2(-0.66f, -0.66f) * shootingSpeed);
            shotBalls.Add(fireBall);
        }

        return shotBalls;
    }

	public List<GameObject> Shoot() {
        return Shoot(transform.position);
    }

    public void FutureShoot(float time) {
        activeFuture = true;
        explodeTime = time;
    }

    private GameObject PrepareBall(Vector2 pos) {
        GameObject ball;
        if (mode == 1) {
            ball = GameResources.GetInstance().GetPoisonball();
        } else {
            ball = GameResources.GetInstance().GetFireball();
        }
        ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        ball.GetComponent<Rigidbody2D>().angularVelocity = 0f;
        ball.transform.position = pos;
        ball.SetActive(true);
        return ball;
    }
}

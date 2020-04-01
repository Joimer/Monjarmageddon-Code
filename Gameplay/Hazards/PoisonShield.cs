using UnityEngine;

public class PoisonShield : MonoBehaviour {

    private GameObject[] poisonBalls;
    private float[] angles = new float[4] { 2 * Mathf.PI, 3 * Mathf.PI / 2, 6 * Mathf.PI / 6, Mathf.PI / 2 };
    private float rotateSpeed = 2f;
    private float radius = 0.4f;

    public void Awake() {
        var pbp = Resources.Load<GameObject>(Hazards.POISON_BALL);
        poisonBalls = new GameObject[4];
        for (var i = 0; i < 4; i++) {
            poisonBalls[i] = Instantiate(pbp, transform.position, Quaternion.identity, transform);
            poisonBalls[i].transform.position = (Vector2) transform.position + new Vector2(Mathf.Sin(angles[i]), Mathf.Cos(angles[i])) * radius;
            poisonBalls[i].GetComponent<EnemyProjectile>().inactiveOutOfSight = false;
            poisonBalls[i].SetActive(true);
        }
    }

    public void Update() {
        for (var i = 0; i < 4; i++) {
            angles[i] += rotateSpeed * Time.deltaTime;
            poisonBalls[i].transform.position = (Vector2) transform.position + new Vector2(Mathf.Sin(angles[i]), Mathf.Cos(angles[i])) * radius;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.GetComponent<CrossThrow>() != null
            || collision.gameObject.GetComponent<FeetCollider>() != null
            || collision.gameObject.GetComponent<PlatformerMovement2D>() != null
        ) {
            ShootBalls();
            gameObject.SetActive(false);
        }
    }

    private void ShootBalls() {
        for (var i = 0; i < 4; i++) {
            poisonBalls[i].GetComponent<Rigidbody2D>().AddForce(new Vector2(Mathf.Sin(angles[i]), Mathf.Cos(angles[i])) * 150f);
        }
    }
}

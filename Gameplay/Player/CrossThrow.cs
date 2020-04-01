using UnityEngine;

public class CrossThrow : MonoBehaviour {

    public float shootSpeed = 0.031f;
    private float activation = 0f;
    private PlatformerMovement2D playerMovement;
    private GameObject divideAnim;

    private void Awake() {
        var nun = ObjectLocator.GetPlayer();
        playerMovement = nun.GetComponent<PlatformerMovement2D>();
        divideAnim = Instantiate(Resources.Load<GameObject>(Visuals.CROSS_BREAK), Vector2.zero, Quaternion.identity);
        divideAnim.SetActive(false);
    }

    private void FixedUpdate() {
        if (Time.time - activation > 6f) {
            Break();
        }
    }

    public void BeginShoot() {
        activation = Time.time;
        gameObject.SetActive(true);
        var xForce = playerMovement.shootingDirection == Direction.RIGHT ? 4f : -4f;
        GetComponent<Rigidbody2D>().AddForce(new Vector2(xForce + (playerMovement.GetVelocity().x / 2), 1f), ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.GetComponent<ProjectileDestroyer>() != null) {
            Break();
        }

        if (collision.gameObject.layer == 0) {
            Break();
        }
    }

    public void Break() {
        divideAnim.transform.position = transform.position;
        divideAnim.SetActive(true);
        divideAnim.GetComponent<CrossFade>().Play();
        gameObject.SetActive(false);
    }
}

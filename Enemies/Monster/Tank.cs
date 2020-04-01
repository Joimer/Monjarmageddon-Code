using UnityEngine;

public class Tank : MonoBehaviour, IEnemyEntity {

    private string uid;
    private float lastShot = 0f;
    private float shootingSpeed = GameplayValues.GetEnemyShootSpeed() + 400f;
    private GameObject target;
    private float activeDistance = 3.9f;
    private float fireRate = 1.5f;
    private bool firing = false;
    private float startFiring = 0f;
    private GameObject fireBall;
    private float fireWindUp = 0.4f;

    private void Awake() {
        uid = transform.position.ToString();
        var firePrefab = Resources.Load<GameObject>(Hazards.FIREBALL);
        fireBall = Instantiate(firePrefab, transform.position, transform.rotation);
        fireBall.transform.localScale = new Vector3(1.2f, 1.2f, fireBall.transform.localScale.z);
        fireBall.SetActive(false);
    }

    void Start() {
        target = ObjectLocator.GetPlayer();

        if (GameState.lastCheckpoint != null && GameState.lastCheckpoint.enemiesKilled.Contains(uid)) {
            Destroy(gameObject);
        }
    }

    public void FixedUpdate() {
        if (!GameState.isGameLocked) {
            if (target != null && target.transform.position.x < transform.position.x && Vector2.Distance(transform.position, target.transform.position) <= activeDistance) {
                if (!firing) {
                    firing = true;
                    startFiring = Time.time;
                } else {
                    if (Time.time - startFiring > fireWindUp && lastShot + fireRate < Time.time) {
                        AudioManager.GetInstance().PlayEffect(Sfx.CANON_SHOT, 1f);
                        Shoot();
                    } else {
                        Color color = GetComponent<SpriteRenderer>().color;
                        var speed = 0.015f;
                        color.g -= speed;
                        color.b -= speed;
                        GetComponent<SpriteRenderer>().color = color;
                    }
                }
            } else {
                firing = false;
            }
        }
    }

    private void Shoot() {
        if (!gameObject.activeSelf) {
            return;
        }
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        lastShot = Time.time;
        // Reset fireball velocity and position before re-activating and shooting.
        fireBall.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        fireBall.GetComponent<Rigidbody2D>().angularVelocity = 0f;
        fireBall.transform.position = transform.position;
        var direction = (target.transform.position - fireBall.transform.position).normalized;
        var finalDirection = new Vector2(direction.x, Mathf.Clamp(direction.y, -0.33f, 0.33f));
        fireBall.SetActive(true);
        fireBall.GetComponent<Rigidbody2D>().AddForce(finalDirection * shootingSpeed);
    }

    public string GetUid() {
        return uid;
    }

    public bool IsActive() {
        return true;
    }

    private void Flip() {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}

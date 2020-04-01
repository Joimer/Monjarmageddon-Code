using UnityEngine;

public class EnemyProjectile : MonoBehaviour {

    public bool ignoreCollisions = false;
    public bool inactiveOutOfSight = true;
    public ProjectileType type = ProjectileType.FIREBALL;
    private SpriteRenderer sr;
    private float start;
    private float minimumActiveTime = 5f;
    private float aliveTime = 30f;
    private GameObject disappearAnim;

    public enum ProjectileType {
        BLUE,
        FIREBALL,
        POISONBALL
    }

    private void Awake() {
        sr = GetComponent<SpriteRenderer>();
        string resource;
        if (type == ProjectileType.POISONBALL) {
            resource = Visuals.POISON_SPLIT;
        } else if (type == ProjectileType.BLUE) {
            resource = Visuals.BLUE_SPLIT;
        } else {
            resource = Visuals.FIREBALL_SPLIT;
        }
        disappearAnim = Instantiate(Resources.Load<GameObject>(resource), Vector2.zero, Quaternion.identity);
        disappearAnim.SetActive(false);
    }

    private void Start() {
        start = Time.time;
    }

    private void LateUpdate() {
        if (Time.time - start > aliveTime) {
            Break();
        }

        if (inactiveOutOfSight && !sr.isVisible && Time.time - start > minimumActiveTime) {
            Break();
        }
    }

    private void OnEnable() {
        start = Time.time;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag(Tags.PLAYER_HITBOX)) {
            Break();
        }

        if (!ignoreCollisions && collision.gameObject.layer == 0) {
            Break();
        }
        // Evento a player pool o...
    }

    public void Break() {
        disappearAnim.transform.position = transform.position;
        disappearAnim.SetActive(true);
        disappearAnim.GetComponent<ProjectileDisappear>().Play();
        gameObject.SetActive(false);
    }
}

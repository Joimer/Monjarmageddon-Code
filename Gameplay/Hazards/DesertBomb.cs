using UnityEngine;

public class DesertBomb : MonoBehaviour {

    private float startTime;
    private const float duration = 1f;
    private GameObject explosion;
    private bool activated = false;
    private Direction explosionDir = Direction.ALL;

    public void Awake() {
        explosion = Resources.Load<GameObject>(Hazards.EXPLOSION_EXPANDING);
    }

    public void Activate(Direction dir) {
        activated = true;
        startTime = Time.time;
        explosionDir = dir;
    }

    public void Deactivate() {
        activated = false;
    }

    public void Update() {
        if (activated && Time.time - startTime > duration) {
            var exp = Instantiate(explosion, new Vector2(gameObject.transform.position.x - 1.05f, gameObject.transform.position.y + 0.64f), gameObject.transform.rotation);
            exp.GetComponent<DesertExplosion>().Activate(explosionDir);
            gameObject.SetActive(false);
        }
    }
}

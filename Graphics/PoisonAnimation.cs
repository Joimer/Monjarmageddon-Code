using UnityEngine;

public class PoisonAnimation : MonoBehaviour {

    private float start;

    private void Start() {
        start = Time.time;
    }

    private void FixedUpdate() {
        if (Time.time - start >= 0.24f) {
            var raycastHit = Physics2D.Raycast(gameObject.transform.position, -Vector2.up, 250f);
            if (raycastHit.transform.gameObject.layer == LayerMask.NameToLayer("Default")) {
                Instantiate(Resources.Load<GameObject>("Prefabs/Hazards/ToxicFog"), new Vector2(raycastHit.point.x, raycastHit.point.y + 0.1f), transform.rotation);
            }
            Destroy(gameObject);
        }
    }
}

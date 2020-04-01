using UnityEngine;

public class ExplosionAnimation : MonoBehaviour {

    private float start;
    private GameObject player;

    private void Start() {
        start = Time.time;
        player = ObjectLocator.GetPlayer();
    }

    void FixedUpdate() {
        if (Time.time - start >= 0.39f) {
            var raycastHit = Physics2D.Raycast(gameObject.transform.position, -Vector2.up, 250f);
            if (raycastHit.transform.gameObject.layer == LayerMask.NameToLayer("Default")) {
                Instantiate(Resources.Load<GameObject>("Prefabs/FloorBlood"), raycastHit.point, transform.rotation);
            }

            if (player != null && player.transform != null) {
                var distance = Vector2.Distance(transform.position, player.transform.position);
                if (distance < 0.75f) {
                    var splatter = Instantiate(Resources.Load<GameObject>("Prefabs/BloodSplatter"), player.transform);
                    splatter.transform.localPosition = new Vector2(0f, 0f);
                }
            }
            Destroy(gameObject);
        }
    }
}

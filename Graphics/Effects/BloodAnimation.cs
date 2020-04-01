using UnityEngine;

public class BloodAnimation : MonoBehaviour {

    private float start;
    private GameObject player;
    private GameObject floorBloodResource;
    private GameObject boodSplatterResource;

    public void Awake() {
        floorBloodResource = Resources.Load<GameObject>("Prefabs/FloorBlood");
        boodSplatterResource = Resources.Load<GameObject>("Prefabs/BloodSplatter");
    }

    public void Start() {
        start = Time.time;
        player = ObjectLocator.GetPlayer();
    }

	public void FixedUpdate() {
        if (Time.time - start >= 0.24f) {
            var raycastHit = Physics2D.Raycast(gameObject.transform.position, Vector2.down, 250f);
            if (raycastHit.transform.gameObject.layer == LayerMask.NameToLayer("Default")) {
                Instantiate(floorBloodResource, raycastHit.point, transform.rotation);
            }

            // In case this happens at the same time you die.
            if (player != null && player.transform != null) {
                var distance = Vector2.Distance(transform.position, player.transform.position);
                if (distance < 0.75f) {
                    var splatter = Instantiate(boodSplatterResource, player.transform);
                    splatter.transform.localPosition = new Vector2(0f, 0f);
                }
            }
            Destroy(gameObject);
        }
    }
}

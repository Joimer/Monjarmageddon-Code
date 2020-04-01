using UnityEngine;

public class FireFlask : MonoBehaviour {

    private int movement = 0;
    private Vector2 apex;
    private Vector2 ground;
    private GameObject explosionResource;

    private void Awake() {
        explosionResource = Resources.Load<GameObject>(Hazards.EXPLOSION);
    }

    private void Start() {
        SetPositions(transform.position);
    }

    public void SetPositions(Vector2 pos) {
        apex = new Vector2(pos.x + 1.61f, pos.y + 0.93f);
        ground = new Vector2(apex.x + 1.35f, apex.y - 1.27f);
    }

    private void FixedUpdate() {
        if (isActiveAndEnabled && gameObject.activeSelf) {
            if (movement == 0) {
                if (transform.position.x < apex.x) {
                    transform.Translate(new Vector2(0.086f, 0.05f));
                } else {
                    movement = 1;
                }
            }

            if (movement == 1) {
                if (transform.position.x < ground.x) {
                    transform.Translate(new Vector2(0.053f, -0.03f));
                } else {
                    Explode();
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.GetComponent<DamageReceiver>() != null) {
            Explode();
        }
    }

    private void Explode() {
        movement = 0;
        gameObject.SetActive(false);
        // TODO: Duplicated code in PlayerHitReceiver, rework
        AudioManager.GetInstance().PlayEffect(Sfx.EXPLOSION, 1f);
        Instantiate(explosionResource, gameObject.transform.position, gameObject.transform.rotation);
    }
}

using UnityEngine;

public class Goat : MonoBehaviour {

    public bool ignoreCollisions = false;
    private GameObject dedGoat;

    private void Awake() {
        dedGoat = Instantiate(Resources.Load<GameObject>(Hazards.GOAT_DEAD), transform.position, transform.rotation);
        dedGoat.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag(Tags.PLAYER_HITBOX)) {
            Kill();
        }

        if (!ignoreCollisions && collision.gameObject.layer == 0) {
            Kill();
        }
    }

    private void Kill() {
        dedGoat.transform.position = transform.position;
        dedGoat.SetActive(true);
        dedGoat.GetComponent<GoatDead>().Play();
        gameObject.SetActive(false);
    }
}

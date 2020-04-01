using UnityEngine;

public class DirtyFeet : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag(Tags.PLAYER)) {
            collision.gameObject.GetComponent<Footprints>().SetBloodyFeet();
        }
    }
}

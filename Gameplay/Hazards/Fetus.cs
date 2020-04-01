using UnityEngine;

public class Fetus : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision) {
        var cgo = collision.gameObject;
        if ((cgo.CompareTag(Tags.PLAYER) || cgo.CompareTag(Tags.GROUND_COLLISION))) {
            Instantiate(Resources.Load<GameObject>(Visuals.BLOOD), gameObject.transform.position, gameObject.transform.rotation);
            gameObject.SetActive(false);
        }
    }
}

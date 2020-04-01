using UnityEngine;

public class Balcony : MonoBehaviour {

    private GameObject player;

    private void Start() {
        player = ObjectLocator.GetPlayer();
    }
	private void Update () {
        if (player != null && player.transform.parent && player.transform.parent == transform) {
            GetComponent<SpriteRenderer>().sortingOrder = 7;
        } else {
            GetComponent<SpriteRenderer>().sortingOrder = 3;
        }
	}
}

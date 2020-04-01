using UnityEngine;

public class NoRender : MonoBehaviour {

	private void Start () {
        var renderer = GetComponent<SpriteRenderer>();
        if (renderer != null) {
            renderer.enabled = false;
        }
    }
}

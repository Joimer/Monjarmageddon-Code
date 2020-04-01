using UnityEngine;

public class DeactivateWhenNotSeen : MonoBehaviour {

    private Renderer render;

    public void Awake() {
        render = GetComponent<Renderer>();
    }

	public void Update() {
        if (!render.isVisible) {
            gameObject.SetActive(false);
        }
    }
}

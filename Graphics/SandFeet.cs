using UnityEngine;

public class SandFeet : MonoBehaviour {

	private void Start() {
        var movement = GetComponent<Footprints>();
        if (movement != null) {
            movement.SteppingSand();
        }
	}
}

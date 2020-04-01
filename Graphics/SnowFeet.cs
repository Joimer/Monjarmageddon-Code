using UnityEngine;

public class SnowFeet : MonoBehaviour {

    private void Start() {
        var movement = GetComponent<Footprints>();
        if (movement != null) {
            movement.SteppingSnow();
        }
    }
}

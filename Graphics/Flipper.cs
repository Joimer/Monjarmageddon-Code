using UnityEngine;

public class Flipper : MonoBehaviour {

    [HideInInspector]
    public bool lookingRight = true;

	public void Flip() {
        var scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        lookingRight = !lookingRight;
    }
}

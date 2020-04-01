using UnityEngine;

public class Fadeout : MonoBehaviour {

    private void FixedUpdate() {
        Color color = GetComponent<SpriteRenderer>().color;
        if (color.a > 0) {
            color.a -= 0.01f;
            GetComponent<SpriteRenderer>().color = color;
        }
    }
}

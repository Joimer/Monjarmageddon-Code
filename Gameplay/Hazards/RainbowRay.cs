using UnityEngine;

public class RainbowRay : MonoBehaviour {

    private int step = 1;
    private SpriteRenderer sr;
    private float lastTick;
    private float switchDelay = 0.08f;

    private void Awake() {
        sr = GetComponent<SpriteRenderer>();

        // Set it to start in red and semi-transparent.
        var color = Color.red;
        color.a = 0.4423f;
        sr.color = color;
        lastTick = Time.time;
    }

    private void FixedUpdate() {
        var color = sr.color;

        // orange (255, 127, 0)
        if (step == 1 && Time.time - lastTick > switchDelay) {
            lastTick = Time.time;
            color = new Color(1, 0.49f, 0);
            step = 2;
        }

        // yellow (244, 228, 18)
        if (step == 2 && Time.time - lastTick > switchDelay) {
            lastTick = Time.time;
            color = Color.yellow;
            step = 3;
        }

        // green (12, 178, 13)
        if (step == 3 && Time.time - lastTick > switchDelay) {
            lastTick = Time.time;
            color = new Color(0.3f, 0.69f, 0.05f);
            step = 4;
        }

        // blue (41, 49, 214)
        if (step == 4 && Time.time - lastTick > switchDelay) {
            lastTick = Time.time;
            color = new Color(0.16f, 0.19f, 0.84f);
            step = 5;
        }

        // purple (126, 41, 214)
        if (step == 5 && Time.time - lastTick > switchDelay) {
            lastTick = Time.time;
            color = new Color(0.49f, 0.16f, 0.84f);
            step = 6;
        }

        // magenta? idk (255, 0, 255)
        if (step == 6 && Time.time - lastTick > switchDelay) {
            lastTick = Time.time;
            color = Color.magenta;
            step = 7;
        }

        // Goes red (255, 0, 0)
        if (step == 7 && Time.time - lastTick > switchDelay) {
            lastTick = Time.time;
            color = Color.red;
            step = 1;
        }

        color.a = 0.4423f;
        sr.color = color;
    }
}

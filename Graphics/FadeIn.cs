using UnityEngine;
using System;

public class FadeIn : MonoBehaviour {

    public event Action onFadeInEnd;
    private bool finished = false;
    private bool active = false;
    private Color color;

    private void Awake() {
        color = GetComponent<SpriteRenderer>().color;
    }

    private void FixedUpdate() {
        if (active) {
            if (color.a < 1) {
                color.a += 0.01f;
                GetComponent<SpriteRenderer>().color = color;
            } else if (!finished) {
                finished = true;
                if (onFadeInEnd != null) {
                    onFadeInEnd();
                }
            }
        }
    }

    public void ResetState() {
        finished = false;
        active = false;
        color.a = 0f;
        GetComponent<SpriteRenderer>().color = color;
    }

    public void Activate() {
        active = true;
    }
}

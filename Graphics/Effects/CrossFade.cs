using UnityEngine;

public class CrossFade : MonoBehaviour {

    private float started = 0f;

    public void Play() {
        started = Time.time;
        GetComponent<Animator>().Play("cross_fade", -1, 0f);
    }

    public void Update() {
        if (Time.time - started >= 0.21f) {
            gameObject.SetActive(false);
        }
    }
}

using UnityEngine;

public class ProjectileDisappear : MonoBehaviour {

    private float started = 0f;

    public void Play() {
        started = Time.time;
        GetComponent<Animator>().Play("split", -1, 0f);
    }

    public void Update() {
        if (Time.time - started >= 0.2f) {
            gameObject.SetActive(false);
        }
    }
}

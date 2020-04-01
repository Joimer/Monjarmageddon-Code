using UnityEngine;

public class GoatDead : MonoBehaviour {

    private float started = 0f;

    public void Play() {
        started = Time.time;
        GetComponent<Animator>().Play("goat_dead", -1, 0f);
    }

    public void Update() {
        if (Time.time - started >= 0.4f) {
            gameObject.SetActive(false);
        }
    }
}

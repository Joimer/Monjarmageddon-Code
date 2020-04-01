using UnityEngine;

public class Teleport : MonoBehaviour {

    private float started = 0f;

	public void Play() {
        started = Time.time;
        GetComponent<Animator>().Play("teleport", -1, 0f);
    }

    public void Update() {
        if (Time.time - started >= 0.4f) {
            gameObject.SetActive(false);
        }
    }
}

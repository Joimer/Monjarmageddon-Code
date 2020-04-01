using UnityEngine;

public class Deactivator : MonoBehaviour {

    public float timeToDeactivate;
    private float start;

    private void Awake() {
        start = Time.time;
    }

    private void Update() {
        if (Time.time - start >= timeToDeactivate) {
            gameObject.SetActive(false);
        }
    }
}

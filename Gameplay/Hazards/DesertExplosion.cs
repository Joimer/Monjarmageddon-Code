using UnityEngine;

public class DesertExplosion : MonoBehaviour {

    public GameObject upperExplosion;
    public GameObject lowerExplosion;
    public GameObject leftExplosion;
    public GameObject rightExplosion;

    private bool active = false;
    private float start = 0f;
    private float duration = 0.4f;

    public void Activate() {
        Activate(Direction.ALL);
    }

    public void Activate(Direction dir) {
        if (((byte)dir & (1 << 0)) == 0) {
            upperExplosion.SetActive(false);
        }
        if (((byte)dir & (1 << 1)) == 0) {
            lowerExplosion.SetActive(false);
        }
        if (((byte)dir & (1 << 2)) == 0) {
            rightExplosion.SetActive(false);
        }
        if (((byte)dir & (1 << 3)) == 0) {
            leftExplosion.SetActive(false);
        }
        active = true;
        start = Time.time;
    }

    public void Update() {
        if (active && Time.time - start > duration) {
            gameObject.SetActive(false);
        }
    }
}

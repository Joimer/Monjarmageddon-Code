using UnityEngine;

public class Portal : MonoBehaviour {

    public Vector2 exitPoint = Vector2.zero;
    private ObjectPool tps;

    public void Start() {
        tps = new ObjectPool(Resources.Load<GameObject>(Visuals.TELEPORT), 4);
    } 

    public void OnTriggerEnter2D(Collider2D collision) {
        var tp1 = tps.RetrieveNext();
        var tp2 = tps.RetrieveNext();
        PlayTeleportAnim(tp1, collision.gameObject.transform.position);
        collision.gameObject.transform.position = exitPoint;
        AudioManager.GetInstance().PlayEffect(Sfx.PORTAL);
        PlayTeleportAnim(tp2, exitPoint);
    }

    private void PlayTeleportAnim(GameObject tp, Vector2 pos) {
        tp.transform.position = pos;
        tp.SetActive(true);
        tp.GetComponent<Teleport>().Play();
    }
}

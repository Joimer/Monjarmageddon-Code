using UnityEngine;

public class CokeTable : MonoBehaviour {

    private bool used = false;

    private string uid;

    private void Awake() {
        uid = transform.position.ToString();
    }

    void Start() {
        if (GameState.lastCheckpoint != null && GameState.lastCheckpoint.objectsUsed.Contains(uid)) {
            SetUsed();
        }
    }

    public void OnTriggerEnter2D(Collider2D collision) {
        if (!used && collision.GetComponent<PlatformerMovement2D>() != null) {
            AudioManager.GetInstance().PlayEffect(Sfx.PICK_ITEM);
            SetUsed();
            collision.GetComponent<PlatformerMovement2D>().SpeedBuff();
        }
    }

    private string GetUid() {
        return GetComponent<Transform>().position.ToString();
    }

    private void SetUsed() {
        used = true;
        // TODO: Cambiar
        GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/items/used_table");
    }
}

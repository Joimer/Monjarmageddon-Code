using UnityEngine;

public class Rosary : MonoBehaviour {

    private string uid;
    private GameObject[] stars;
    private Vector2[] dirs = new Vector2[] {
        Vector2.right,
        Vector2.left,
        Vector2.up,
        Vector2.down + Vector2.right,
        Vector2.down + Vector2.left,
    };
    private float shootingSpeed = 250f;

    private void Awake() {
        uid = transform.position.ToString();
        stars = new GameObject[5];
        var prefab = Resources.Load<GameObject>("Prefabs/Props/Star");
        for (var i = 0; i < 5; i++) {
            stars[i] = Instantiate(prefab, transform.position, Quaternion.identity);
            stars[i].SetActive(false);
        }
    }

    private void Start() {
        if (GameState.lastCheckpoint != null && GameState.lastCheckpoint.objectsUsed.Contains(uid)) {
            ShootStars();
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        var nun = collision.gameObject.GetComponent<PlatformerMovement2D>();
        if (nun != null) {
            AudioManager.GetInstance().PlayEffect(Sfx.POWER_UP);
            GameState.GetInstance().objectsUsed.Add(uid);
            nun.SetHalo();
            ShootStars();
            Destroy(gameObject);
        }
    }

    private void ShootStars() {
        for (var i = 0; i < 5; i++) {
            stars[i].SetActive(true);
            stars[i].GetComponent<Rigidbody2D>().AddForce(dirs[i] * shootingSpeed);
        }
    }
}

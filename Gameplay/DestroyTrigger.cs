using UnityEngine;

public class DestroyTrigger : MonoBehaviour {

    public GameObject toDestroy;
    public float delayToDestroy = 0f;
    public LeverColours colour;
    private string uid;
    private Sprite usedSprite;
    private bool destroyed = false;

    public enum LeverColours {
        RED = 1,
        GREEN = 2,
        BLUE = 3
    }

    private void Awake() {
        uid = transform.position.ToString();
        usedSprite = Resources.Load<Sprite>("Sprites/lever_pushed");
    }

    private void Start() {
        if (GameState.lastCheckpoint != null && GameState.lastCheckpoint.enemiesKilled.Contains(uid)) {
            RemoveTarget();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (toDestroy != null && !destroyed) {
            AudioManager.GetInstance().PlayEffect(Sfx.PORTAL);
            RemoveTarget();
            GameState.GetInstance().enemiesKilled.Add(uid);
        }
    }

    private void RemoveTarget() {
        destroyed = true;
        Destroy(toDestroy, delayToDestroy);
        GetComponent<SpriteRenderer>().sprite = usedSprite;

        // Turn off the light signs.
        if (colour == LeverColours.RED) {
            var arrows = (RedRayButton[]) GameObject.FindObjectsOfType(typeof(RedRayButton));
            foreach (var arrow in arrows) {
                arrow.gameObject.SetActive(false);
            }
        } else if (colour == LeverColours.GREEN) {
            var arrows = (GreenRayButton[]) GameObject.FindObjectsOfType(typeof(GreenRayButton));
            foreach (var arrow in arrows) {
                arrow.gameObject.SetActive(false);
            }
        } else {
            var arrows = (BlueRayButton[]) GameObject.FindObjectsOfType(typeof(BlueRayButton));
            foreach (var arrow in arrows) {
                arrow.gameObject.SetActive(false);
            }
        }
    }
}

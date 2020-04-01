using UnityEngine;

public class StageActIntro : MonoBehaviour {

    public GameObject zoneName;
    public GameObject disk;
    public GameObject zoneText;
    public GameObject actText;
    // For stages with different beginning.
    public float offset = 0f;
    private Vector2 zoneNamePosition = new Vector2(-2.2f, 0.33f);
    private Vector2 diskPosition = new Vector2(-0.71f, -0.03f);    
    private Vector2 zoneTextPosition = new Vector2(-1.69f, -0.25f);
    private Vector2 actTextPosition = new Vector2(-0.86f, -0.51f);
    private int turn = 1;
    private float moveSpeed = 0.25f;
    private float introInTime = 0f;
    private float introOutTime = 0f;
    private GameObject gameInfo;
    private GameObject gameMenu;

    private void Start() {
        GameState.menuAvailable = false;
        GameState.isGameLocked = true;
        gameInfo = GameObject.Find("GameInfo");
        gameInfo.SetActive(false);
    }

	private void FixedUpdate () {
		if (turn == 1) {
            MoveZoneName();
        }
        if (turn == 2) {
            MoveDisk();
        }
        if (turn == 3) {
            MoveZoneText();
        }
        if (turn == 4) {
            MoveActText();
        }
        if (turn == 5) {
            introInTime = Time.time;
            turn = 6;
        }
        if (turn == 6 && introInTime + 1 < Time.time) {
            // Make black background transparent so we can reuse it later for level end.
            // Turn renderer off so it uses the least CPU possible.
            Color color = GetComponent<SpriteRenderer>().color;
            color.a = 0;
            GetComponent<SpriteRenderer>().color = color;
            GetComponent<Renderer>().enabled = false;
            gameInfo.SetActive(true);
            if (!GameMenu.menuActive) {
                GameState.isGameLocked = false;
            }
            introOutTime = Time.time;
            turn = 7;
        }
        if (turn == 7 && introOutTime + 0.5 < Time.time) {
            GameState.menuAvailable = true;
            MoveOut();
        }
        if (turn == 8) {
            Destroy(zoneName);
            Destroy(disk);
            Destroy(zoneText);
            Destroy(actText);
        }
    }

    private void MoveZoneName() {
        if (turn == 1 && zoneName.transform.localPosition.x < zoneNamePosition.x + offset) {
            zoneName.transform.Translate(new Vector2(moveSpeed, 0f));
        } else {
            turn = 2;
        }
    }

    private void MoveDisk() {
        if (turn == 2 && disk.transform.localPosition.x > diskPosition.x + offset) {
            disk.transform.Translate(new Vector2(-moveSpeed, 0f));
        } else {
            turn = 3;
        }
    }

    private void MoveZoneText() {
        if (turn == 3 && zoneText.transform.localPosition.x < zoneTextPosition.x + offset) {
            zoneText.transform.Translate(new Vector2(moveSpeed, 0f));
        } else {
            turn = 4;
        }
    }

    private void MoveActText() {
        if (actText != null && turn == 4 && actText.transform.localPosition.x > actTextPosition.x + offset) {
            actText.transform.Translate(new Vector2(-moveSpeed, 0f));
        } else {
            turn = 5;
        }
    }

    private void MoveOut() {
        if (turn == 7 && zoneName.GetComponent<Renderer>().isVisible && disk.GetComponent<Renderer>().isVisible
            && zoneText.GetComponent<Renderer>().isVisible
            && (actText == null || actText.GetComponent<Renderer>().isVisible)
        ) {
            zoneName.transform.Translate(new Vector2(-moveSpeed * 2, 0f));
            disk.transform.Translate(new Vector2(moveSpeed * 2, 0f));
            zoneText.transform.Translate(new Vector2(-moveSpeed * 2, 0f));
            if (actText != null) {
                actText.transform.Translate(new Vector2(moveSpeed * 2, 0f));
            }
        } else {
            turn = 8;
        }
    }
}

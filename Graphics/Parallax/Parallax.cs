using UnityEngine;

public class Parallax : MonoBehaviour {

    // Repetition first appearing to the left.
    public GameObject leftBg;
    // Repetition first appearing to the right.
    public GameObject rightBg;
    // Does it move horizontally?
    public bool horizontalMove = true;
    // Does it move vertically as well?
    public bool verticalMove = true;
    // Divisor for the total horizontal speed, the higher the slowest it is.
    public float hoirzontalSpeedDiv = 10f;
    // Divisor for the total vertical speed, the higher the slowest it is.
    public float verticalSpeedDiv = 10f;
    // Automatically move horizontally?
    // TODO: Remove?
    public bool autoHorizontal = false;
    // Array to manage which one is on the leftmost and rightmost side.
    private GameObject[] bgs;
    // Camera reference.
    private GameObject reference;
    private Vector2 lastPosition;
    // Width of the background to switch its position.
    private float warpingSize;
    // At which point to the left will the object wrap from its position to the right.
    private float leftWrappingPoint = 0f;
    // At which point to the right will the object wrap from its position to the left.
    private float rightWrappingPoint = 0f;

    public void Start() {
        bgs = new GameObject[3] { leftBg, gameObject, rightBg };
        reference = ObjectLocator.GetCamera();
        lastPosition = reference.transform.position;
        var sr = GetComponent<SpriteRenderer>().sprite;
        warpingSize = sr.rect.size.x * transform.localScale.x / sr.pixelsPerUnit;
        if (horizontalMove) {
            leftWrappingPoint = bgs[0].transform.localPosition.x - warpingSize;
            rightWrappingPoint = bgs[2].transform.localPosition.x + warpingSize;
        }
    }

    private void Update() {
        if (reference == null) {
            return;
        }

        var isCameraLocked = GameState.GetInstance().isCameraLocked;
        var xDifference = autoHorizontal ? -0.01f : (isCameraLocked || !horizontalMove) ? 0f : -(reference.transform.position.x - lastPosition.x) / (hoirzontalSpeedDiv / 2);
        var yDifference = verticalMove && !isCameraLocked ? -(reference.transform.position.y - lastPosition.y) / verticalSpeedDiv : 0f;

        foreach (GameObject bg in bgs) {
            if (bg != null) {
                // Get the difference of initial position with current
                bg.transform.Translate(new Vector2(xDifference, yDifference));
            }
        }

        if (horizontalMove && !isCameraLocked) {
            if (bgs[0].transform.localPosition.x < leftWrappingPoint) {
                var temp = bgs[0];
                temp.transform.localPosition = new Vector2(temp.transform.localPosition.x + warpingSize * 3, temp.transform.localPosition.y);
                bgs[0] = bgs[1];
                bgs[1] = bgs[2];
                bgs[2] = temp;
            }

            if (bgs[2].transform.localPosition.x > rightWrappingPoint) {
                var temp = bgs[2];
                temp.transform.localPosition = new Vector2(temp.transform.localPosition.x - warpingSize * 3, temp.transform.localPosition.y);
                bgs[2] = bgs[1];
                bgs[1] = bgs[0];
                bgs[0] = temp;
            }
        }

        lastPosition = reference.transform.position;
    }
}

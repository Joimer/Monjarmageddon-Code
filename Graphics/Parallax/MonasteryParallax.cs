using UnityEngine;

// I'm lazy and this class works for Monastery.
// Vaya chapuza xd
public class MonasteryParallax : MonoBehaviour {

    public Sprite background;
    public float speed = 10f;
    public bool verticalMove = true;

    // The camera that moves, so the items that are "far" from the camera move with it.
    private GameObject reference;
    private Vector2 lastPosition;
    private GameObject bg1;
    private GameObject bg2;
    private GameObject bg3;
    private float bgSize = 0f;
    
    public void Start() {
        reference = ObjectLocator.GetCamera();

        // Create the parallax items.
        bg1 = new GameObject();
        bg1.name = "Background 1";
        bg2 = new GameObject();
        bg2.name = "Background 2";
        bg3 = new GameObject();
        bg3.name = "Background 3";
        bg1.AddComponent<SpriteRenderer>();
        bg1.transform.parent = transform;
        bg2.transform.parent = transform;
        bg3.transform.parent = transform;
        bg2.AddComponent<SpriteRenderer>();
        bg3.AddComponent<SpriteRenderer>();
        bg1.GetComponent<SpriteRenderer>().sprite = background;
        bg2.GetComponent<SpriteRenderer>().sprite = background;
        bg3.GetComponent<SpriteRenderer>().sprite = background;
        bgSize = bg1.GetComponent<SpriteRenderer>().sprite.rect.size.x;
        bg1.GetComponent<Transform>().localPosition = new Vector2(0, 0);
        bg2.GetComponent<Transform>().localPosition = new Vector2(bgSize, 0);
        bg3.GetComponent<Transform>().localPosition = new Vector2(-bgSize, 0);
    }

    public void Update() {
        if (reference == null) {
            return;
        }

        // Get the difference of initial position with current.
        var xDifference = -(reference.transform.position.x - lastPosition.x) / speed;
        var yDifference = verticalMove ? -(reference.transform.position.y - lastPosition.y) / (speed / 2) : 0f;
        transform.Translate(new Vector2(xDifference, yDifference));
        lastPosition = reference.transform.position;
    }
}

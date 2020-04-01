using System.Collections.Generic;
using UnityEngine;

public class CircularPlatformSystem : MonoBehaviour {

    public float speed = 0.01f;
    public float height = 2.22f;
    private Vector2 initialPosition;
    private float top;
    private float bottom;
    private float right;
    private float left;
    private List<GameObject> platforms = new List<GameObject>();
    public bool darker = false;
    public Orientation orientation = Orientation.VERTICAL;
    public GameObject prefab;

    void Start() {
        GetComponent<SpriteRenderer>().enabled = false;
        initialPosition = transform.position;
        if (orientation == Orientation.HORIZONTAL) {
            left = initialPosition.x - height;
            right = initialPosition.x + height;
            top = initialPosition.y + height / 2;
            bottom = initialPosition.y - height / 2;
        } else {
            left = initialPosition.x - height / 2;
            right = initialPosition.x + height / 2;
            top = initialPosition.y + height;
            bottom = initialPosition.y - height;
        }
        var upperRight = new Vector2(right, top);
        var upperLeft = new Vector2(left, top);
        var lowerRight = new Vector2(right, bottom);
        var lowerLeft = new Vector2(left, bottom);
        var rightPos = new Vector2(right, initialPosition.y);
        var leftPos = new Vector2(left, initialPosition.y);

        // Spawn platforms
        var platform = prefab == null? Resources.Load<GameObject>("Prefabs/DesertPlatform") : prefab;
        var plat1 = Instantiate(platform, upperRight, transform.rotation, transform);
        var plat2 = Instantiate(platform, lowerRight, transform.rotation, transform);
        var plat3 = Instantiate(platform, rightPos, transform.rotation, transform);
        var plat4 = Instantiate(platform, upperLeft, transform.rotation, transform);
        var plat5 = Instantiate(platform, leftPos, transform.rotation, transform);
        var plat6 = Instantiate(platform, lowerLeft, transform.rotation, transform);
        if (darker) {
            var color = new Color(0.529f, 0.443f, 0.443f, 1f);
            plat1.GetComponent<SpriteRenderer>().color = color;
            plat2.GetComponent<SpriteRenderer>().color = color;
            plat3.GetComponent<SpriteRenderer>().color = color;
            plat4.GetComponent<SpriteRenderer>().color = color;
            plat5.GetComponent<SpriteRenderer>().color = color;
            plat6.GetComponent<SpriteRenderer>().color = color;
        }
        platforms.Add(plat1);
        platforms.Add(plat2);
        platforms.Add(plat3);
        platforms.Add(plat4);
        platforms.Add(plat5);
        platforms.Add(plat6);
    }

    void FixedUpdate() {
        if (!GameState.isGameLocked) {
            foreach (GameObject platform in platforms) {
                var x = platform.transform.position.x;
                var y = platform.transform.position.y;

                // The platform has to go down if it's to the left and over the bottom.
                if (x <= left && y > bottom) {
                    platform.transform.Translate(new Vector2(0f, -speed));
                }

                // It has to go up if it's to the right and below the top.
                if (x >= right && y < top) {
                    platform.transform.Translate(new Vector2(0f, speed));
                }

                // It has to go to the right if it's in the bottom.
                if (x <= right && y <= bottom) {
                    platform.transform.Translate(new Vector2(speed, 0f));
                }

                // And it has to go to the left if it's in the top.
                if (x >= left && y >= top) {
                    platform.transform.Translate(new Vector2(-speed, 0f));
                }
            }
        }
    }
}

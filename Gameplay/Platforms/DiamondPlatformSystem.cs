using UnityEngine;
using System;
using System.Collections.Generic;

public class DiamondPlatformSystem : MonoBehaviour {

    private Vector2 initialPosition;
    private float top;
    private float bottom;
    private float right;
    private float left;
    private List<GameObject> platforms = new List<GameObject>();
    public float speed = 0.01f;
    public float distance = 1.5f;
    public GameObject prefab;

    private void Start() {
        GetComponent<SpriteRenderer>().enabled = false;
        initialPosition = transform.position;
        left = (float)Math.Round(initialPosition.x - distance, 2);
        right = (float)Math.Round(initialPosition.x + distance, 2);
        top = (float)Math.Round(initialPosition.y + distance, 2);
        bottom = (float)Math.Round(initialPosition.y - distance, 2);

        // Decide where the platforms go.
        var upPos = new Vector2(initialPosition.x, top);
        var downPos = new Vector2(initialPosition.x, bottom);
        var rightPos = new Vector2(right, initialPosition.y);
        var leftPos = new Vector2(left, initialPosition.y);

        // Spawn platforms.
        var platform = prefab == null ? Resources.Load<GameObject>("Prefabs/LabPlatform") : prefab;
        var plat1 = Instantiate(platform, upPos, transform.rotation, transform);
        var plat2 = Instantiate(platform, downPos, transform.rotation, transform);
        var plat3 = Instantiate(platform, rightPos, transform.rotation, transform);
        var plat4 = Instantiate(platform, leftPos, transform.rotation, transform);
        platforms.Add(plat1);
        platforms.Add(plat2);
        platforms.Add(plat3);
        platforms.Add(plat4);
    }

    private void FixedUpdate() {
        if (!GameState.isGameLocked) {
            float x, y, xMove, yMove;
            foreach (GameObject platform in platforms) {
                x = platform.transform.position.x;
                y = platform.transform.position.y;
                // The platform has to go down and to the left if it has reached the top peak and central axis.
                // And it has to go down and to the right if it has reached the mid of the vertical axis and the leftmost of the horizontal axis.
                // It has to go up and right if it has reached the middle of the horizontal axis and the bottom point of the vertical axis.
                // It has to go to the left and up if it has reached top of vertical axis and the middle of the horizontal axis.
                xMove = y >= initialPosition.y ? -speed : speed;
                yMove = x < initialPosition.x ? -speed : speed;
                platform.transform.Translate(new Vector2(xMove, yMove));
            }
        }
    }
}

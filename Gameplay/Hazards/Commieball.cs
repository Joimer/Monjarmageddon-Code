using UnityEngine;
using System.Collections.Generic;

public class Commieball : MonoBehaviour {

    private Direction direction;

    private void Awake() {
        direction = Direction.DOWNRIGHT;
    }

    private void Start() {
        gameObject.AddComponent<CommieFire>();
    }

    private void FixedUpdate() {
        var x = direction == Direction.DOWNRIGHT || direction == Direction.UPRIGHT ? 0.025f : -0.025f;
        var y = direction == Direction.UPLEFT || direction == Direction.UPRIGHT ? 0.025f : -0.025f;
        var movement = new Vector2(x, y);
        transform.Translate(movement);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        var directions = new Dictionary<Direction, Direction>() {
            { Direction.DOWNRIGHT, Direction.UPLEFT },
            { Direction.UPLEFT, Direction.DOWNLEFT },
            { Direction.DOWNLEFT, Direction.UPRIGHT },
            { Direction.UPRIGHT, Direction.DOWNRIGHT }
        };
        if (directions.ContainsKey(direction)) {
            direction = directions[direction];
        }
    }
}

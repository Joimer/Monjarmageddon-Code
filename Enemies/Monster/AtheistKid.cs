using UnityEngine;

public class AtheistKid : MonoBehaviour, IEnemyEntity {

    private Vector3 initialPosition;
    private string uid;

    public void Awake() {
        uid = transform.position.ToString();
        var movement = gameObject.AddComponent<MonsterHorizontalMovement>();
        movement.speed = GameState.difficulty == Difficulty.VERY_EASY ? 0.02f : 0.03f;
        movement.range = GameState.difficulty == Difficulty.VERY_EASY ? 1.3f : 1.5f;
    }

    public void Start() {
        if (GameState.lastCheckpoint != null && GameState.lastCheckpoint.enemiesKilled.Contains(uid)) {
            Destroy(gameObject);
        }
    }

    public string GetUid() {
        return uid;
    }

    public bool IsActive() {
        return true;
    }
}

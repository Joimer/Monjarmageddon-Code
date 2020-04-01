using UnityEngine;
using System.Collections.Generic;

public class CheckpointDetector : MonoBehaviour {

    private bool used = false;
    private Sprite openBible;

    public void Awake() {
        openBible = Resources.Load<Sprite>("Sprites/Items/bible_open");
    }

    public void Start() {
        if (GameState.lastCheckpoint != null) {
            if (GameState.lastCheckpoint.objectsUsed.Contains(GetUid())) {
                SetUsed();
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision) {
        if (!used && collision.GetComponent<PlatformerMovement2D>() != null) {
            AudioManager.GetInstance().PlayEffect(Sfx.CHECKPOINT);
            SetUsed();
            var gs = GameState.GetInstance();
            GameState.lastCheckpoint = new Checkpoint(
                gs.currentSceneTime, GameState.score, gs.obleas, GameState.holyWaters, collision.GetComponent<Transform>().position, GetUid(),
                new List<string>(gs.enemiesKilled),
                new List<string>(gs.objectsUsed)
            );
        }
    }

    private string GetUid() {
        return GetComponent<Transform>().position.ToString();
    }

    private void SetUsed() {
        used = true;
        GameState.GetInstance().objectsUsed.Add(GetUid());
        GetComponent<SpriteRenderer>().sprite = openBible;
    }
}

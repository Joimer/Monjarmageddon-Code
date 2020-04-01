using UnityEngine;
using System.Collections.Generic;

// This stores a checkpoint state upon a level, where the player state is saved to be restored after death.
public class Checkpoint {

    public float timeEllapsed;
    public int score;
    public int wafers;
    public int holyWaters;
    public Vector2 position;
    public string uid;
    public List<string> enemiesKilled;
    public List<string> objectsUsed;

    public Checkpoint(float timeEllapsed, int score, int wafers, int holyWaters, Vector2 position, string uid, List<string> enemiesKilled, List<string> objectsUsed) {
        this.timeEllapsed = timeEllapsed;
        this.score = score;
        this.wafers = wafers;
        this.holyWaters = holyWaters;
        this.position = position;
        this.uid = uid;
        this.enemiesKilled = enemiesKilled;
        this.objectsUsed = objectsUsed;
    }
}

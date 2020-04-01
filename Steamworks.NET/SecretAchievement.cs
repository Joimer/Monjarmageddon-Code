using UnityEngine;

public class SecretAchievement : MonoBehaviour {

    public void OnTriggerEnter2D(Collider2D collision) {
        var player = collision.GetComponent<PlatformerMovement2D>();
        if (player != null) {
            Destroy(gameObject);
            AchievementManager.UnlockAchievement(Achievements.SECRET_FOUND);
        }
    }
}

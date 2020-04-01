using UnityEngine;

public class Gravel : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision) {
        var explosion = collision.GetComponent<ExplosionAnimation>();
        if (explosion != null) {
            Destroy(gameObject);
            AchievementManager.UnlockAchievement(Achievements.PATH_BOMB_OPEN);
        }
    }
}

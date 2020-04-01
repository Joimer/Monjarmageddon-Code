using UnityEngine;

public class BossPlatform : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.GetComponent<NightQueen>() != null) {
            AudioManager.GetInstance().PlayEffect(Sfx.RIFLE_SHOT);
            gameObject.AddComponent<FallDown>().Touch();
        }
    }
}

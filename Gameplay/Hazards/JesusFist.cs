using UnityEngine;

public class JesusFist : MonoBehaviour, IEnemyEntity {

	public string GetUid() {
        return "jesus_clenched_fist";
    }

    public bool IsActive() {
        return true;
    }
}

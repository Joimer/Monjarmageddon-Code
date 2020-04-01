using UnityEngine;

public class GameResources : MonoBehaviour {

    private static ObjectPool fireballPool;
    private static ObjectPool poisonballPool;
    public static GameResources instance;

    public static GameResources GetInstance() {
        if (instance == null) {
            instance = new GameObject().AddComponent<GameResources>();
            instance.name = "Game Resources";
        }
        return instance;
    }

    public void Awake() {
        fireballPool = new ObjectPool(Resources.Load<GameObject>(Hazards.FIREBALL), 100);
        poisonballPool = new ObjectPool(Resources.Load<GameObject>(Hazards.POISON_BALL), 32);
    }

    public GameObject GetFireball() {
        return fireballPool.RetrieveNext();
    }

    public GameObject GetPoisonball() {
        return poisonballPool.RetrieveNext();
    }
}

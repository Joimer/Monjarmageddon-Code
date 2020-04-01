using UnityEngine;

public class Enemy : MonoBehaviour {

    [HideInInspector]
    public Vector3 initialPosition { private set; get; }

    private void Start () {
        initialPosition = transform.position;
    }
}

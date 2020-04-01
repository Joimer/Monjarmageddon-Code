using UnityEngine;

public class SemiTransparent : MonoBehaviour {
	
	void Start () {
        Color color = GetComponent<SpriteRenderer>().material.color;
        color.a = 0.5f;
    }
}

using UnityEngine;

public class PartedShot : MonoBehaviour {

    private float start = 0f;
    private float partTime = 0.25f;

	private void Start () {
        start = Time.time;
    }

	private void Update () {
		if (Time.time - start > partTime) {
            var prefab = Resources.Load<GameObject>(Hazards.FIREBALL);
            var fireBall = Instantiate(prefab, transform.position, transform.rotation);
            var fireBallTwo = Instantiate(prefab, transform.position, transform.rotation);
            var fireBallThree = Instantiate(prefab, transform.position, transform.rotation);
            var velocity = GetComponent<Rigidbody2D>().velocity;
            fireBall.GetComponent<Rigidbody2D>().velocity = velocity;
            fireBallTwo.GetComponent<Rigidbody2D>().velocity = new Vector2(velocity.x, 0.25f);
            fireBallThree.GetComponent<Rigidbody2D>().velocity = new Vector2(velocity.x, -0.25f);
            Destroy(gameObject);
        }
	}
}

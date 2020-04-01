using UnityEngine;

public class CommieFire : MonoBehaviour {

    [HideInInspector]
    public GameObject centreReference;
    private GameObject commieFire;
    private float rotateSpeed = 3f;
    private float radius = 1f;
    private float angle;

    private void Start() {
        centreReference = gameObject;
        commieFire = Instantiate(Resources.Load<GameObject>(Hazards.COMMIE_FIRE), transform.position, Quaternion.identity);
        SetCentreReference(gameObject);
    }

    private void Update() {
        angle += rotateSpeed * Time.deltaTime;
        var offset = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)) * radius;
        commieFire.transform.position = (Vector2) centreReference.transform.position + offset;
    }

    public void SetCentreReference(GameObject obj) {
        centreReference = obj;
    }
}

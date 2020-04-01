using UnityEngine;

public class AlwaysRotate : MonoBehaviour {

    // Degrees it rotates per second
    public float rotationSpeed = 720f;

	// Update is called once per frame
	private void Update() {
        var rotAmount = rotationSpeed * Time.deltaTime;
        var curRot = transform.localRotation.eulerAngles.z;
        transform.localRotation = Quaternion.Euler(new Vector3(0, 0, curRot + rotAmount));
    }
}

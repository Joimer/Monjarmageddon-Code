using UnityEngine;

public class ShootTarget : MonoBehaviour {

    private GameObject target;
    private GameObject prefab;
    private float activeDistance = 2.6f;
    private float shootingSpeed = GameplayValues.GetEnemyShootSpeed();
    //private float fireRate = 1f;
    //private float lastShot = 0f;

    public void Awake() {
        // TODO FINISH ME AAAAH
        prefab = Resources.Load<GameObject>(Hazards.FIREBALL);
    }

    public void SetTarget(GameObject obj) {
        target = obj;
    }

    private void Shoot() {
        if (gameObject.activeSelf && target != null && Vector2.Distance(transform.position, target.transform.position) <= activeDistance) {
            //lastShot = Time.time;
            var bullet = Instantiate(prefab, transform.position, transform.rotation);
            Vector2 direction = (target.transform.position - bullet.transform.position).normalized;
            bullet.GetComponent<Rigidbody2D>().AddForce(direction * shootingSpeed);
        }
    }
}

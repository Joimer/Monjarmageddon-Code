using UnityEngine;

public class Maoist : MonoBehaviour, IEnemyEntity {

    // Animate AK recoil
    public GameObject ak;
    private Vector2 recoil = new Vector2(0.019f, -0.0098f);
    private float nextRecoil = 0f;
    private float recoilTime = 0.1f;
    private int recoilsLeft = 0;

    private string uid;
    // Movement
    private bool isMovingLeft = true;
    private bool lookingLeft = true;
    private float horizontalSpeed = 0.06f;
    private bool rightLocked = false;
    private bool leftLocked = false;
    // Shooting
    private float lastShot = 0f;
    private float shootingSpeed = GameplayValues.GetEnemyShootSpeed() + 150f;
    private GameObject target;
    private float activeDistance = 3f;
    private float fireRate = 1f;
    private float longFireRate = 1f;
    private float shortFireRate = 0.15f;
    private bool targetToTheLeft = true;
    private int fired = 0;
    private Animator animator;

    private void Awake() {
        uid = transform.position.ToString();
        animator = GetComponent<Animator>();
    }

    void Start() {
        target = ObjectLocator.GetPlayer();

        if (GameState.lastCheckpoint != null && GameState.lastCheckpoint.enemiesKilled.Contains(uid)) {
            Destroy(gameObject);
        }
    }

    void FixedUpdate() {
        if (!GameState.isGameLocked) {
            if (target != null) {
                var close = Vector2.Distance(transform.position, target.transform.position) <= activeDistance;
                if (close) {
                    targetToTheLeft = target.transform.position.x < transform.position.x;
                    // Nun is to the left (lesser x), moves to the right
                    if (targetToTheLeft && !rightLocked) {
                        transform.Translate(new Vector2(horizontalSpeed, 0f));
                        isMovingLeft = false;
                        leftLocked = false;
                        if (!lookingLeft) {
                            Flip();
                        }
                        animator.Play(Animator.StringToHash(Animations.MAOIST_WALKING));
                    }

                    // Nun is to the right (higher x), moves to the left.
                    if (!targetToTheLeft && !leftLocked) {
                        transform.Translate(new Vector2(-horizontalSpeed, 0f));
                        isMovingLeft = true;
                        rightLocked = false;
                        if (lookingLeft) {
                            Flip();
                        }
                        animator.Play(Animator.StringToHash(Animations.MAOIST_WALKING));
                    }

                    if (leftLocked || rightLocked) {
                        animator.Play(Animator.StringToHash(Animations.MAOIST_STILL));
                    }
                } else {
                    animator.Play(Animator.StringToHash(Animations.MAOIST_STILL));
                }

                if (close || fired > 0) {
                    if (lastShot + fireRate < Time.time) {
                        Shoot();
                        ak.transform.Translate(recoil);
                        nextRecoil = Time.time + recoilTime;
                        recoilsLeft++;
                        fired++;
                        if (fired < 3) {
                            fireRate = shortFireRate;
                        } else {
                            fired = 0;
                            fireRate = longFireRate;
                        }
                    }
                }

                if (recoilsLeft > 0 && nextRecoil <= Time.time) {
                    ak.transform.Translate(-recoil);
                    recoilsLeft--;
                }
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag(Tags.ENEMY_COLLISION)) {
            if (isMovingLeft) {
                leftLocked = true;
            } else {
                rightLocked = true;
            }
        }
    }

    private void Shoot() {
        if (!gameObject.activeSelf) {
            return;
        }
        AudioManager.GetInstance().PlayEffect(Sfx.RIFLE_SHOT, 1f);
        lastShot = Time.time;
        // TODO: Refactor
        var fireBall = Instantiate(Resources.Load<GameObject>(Hazards.GUNSHOT), transform.position, transform.rotation);
        var isTargetLeft = target.transform.position.x < fireBall.transform.position.x;
        Vector2 dir = isTargetLeft? Vector2.left : Vector2.right;
        if (fired == 1) {
            dir += new Vector2(0f, 0.33f);
        }
        if (fired == 2) {
            dir += new Vector2(0f, -0.33f);
        }
        fireBall.GetComponent<Rigidbody2D>().AddForce(dir * shootingSpeed);
    }

    public string GetUid() {
        return uid;
    }

    public bool IsActive() {
        return true;
    }

    private void Flip() {
        lookingLeft = !lookingLeft;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}

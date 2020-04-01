using Prime31;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerMovement2D : MonoBehaviour {

    // References to the managers.
    private InputManager inputManager;
    private AudioManager audioManager;

    // Movement
    private CharacterController2D controller;
    private RaycastHit2D lastControllerColliderHit;
    private Vector3 velocity;
    private float gravity = -30f;
    private float runSpeed = 3f;
    private float groundDamping = 20f;
    private float inAirDamping = 10f;
    private float jumpHeight = 1.5f;
    private bool ignoreJumpVariable = false;
    private float normalizedHorizontalSpeed = 0;
    private bool speedBuffed = false;
    private float speedBuffApplied = 0f;
    private bool crouching = false;
    private bool lookingUp = false;
    private float crouchHitboxOffset = 0.1f;
    private bool canMove = true;

    // Attacking
    private float projectileRatio = 0.33f;
    private float lastProjectile = 0f;
    [HideInInspector]
    public Direction shootingDirection; // TODO: Public because of crossthrow, uhm...
    // Abstract in another class?
    private GameObject[] crossPool = new GameObject[5];
    private int crossPoolIndex = 0;

    // Hit management
    [HideInInspector]
    public GameObject overlapping;
    private GameObject hitbox;
    private bool dying = false;
    private EnemyHitReceiver hitReceiver;
    private Invulnerability invuln;
    private GameObject invulnerableAura;
    private bool haloActive = false;
    private BoxCollider2D coll;
    private BoxCollider2D hitboxColl;
    private Vector2 colliderFullSize;
    private Vector2 colliderFullOffset;
    private Vector2 hitboxFullSize;
    private Vector2 hitboxFullOffset;

    // Animation
    private Animator animator;
    private float lastAction = 0f;
    private string currentAnimation = Animations.NUN_IDLE;
    private List<string> idleAnims = new List<string>() {
        Animations.NUN_IDLE,
        Animations.NUN_WAITING,
        Animations.NUN_BORED,
        Animations.NUN_PRAYING,
        Animations.NUN_LOOKING_UP,
        Animations.NUN_CROUCHING,
        Animations.NUN_PRAYING_BOSS
    };
    private GameObject deadNunResource;
    private Flipper flipper;

    private void Awake() {
        shootingDirection = Direction.RIGHT;
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController2D>();
        inputManager = InputManager.GetInstance();
        audioManager = AudioManager.GetInstance();
        flipper = GetComponent<Flipper>();
        dying = false;
        hitReceiver = GetComponent<EnemyHitReceiver>();
        invuln = GetComponent<Invulnerability>();
        invulnerableAura = Instantiate(Resources.Load<GameObject>("Prefabs/Halo"), transform);
        invulnerableAura.SetActive(false);

        // Controller events.
        controller.onControllerCollidedEvent += onControllerCollider;
        controller.onTriggerEnterEvent += onTriggerEnterEvent;
        controller.onTriggerExitEvent += onTriggerExitEvent;

        // Object pools.
        var projectile = Instantiate(Resources.Load<GameObject>("Prefabs/CrossProjectile"), transform);
        for (var i = 0; i < 5; i++) {
            crossPool[i] = Instantiate(projectile, Vector2.zero, Quaternion.identity).gameObject;
            crossPool[i].SetActive(false);
        }
        deadNunResource = Resources.Load<GameObject>("Prefabs/DeadNun");
    }

    private void Start() {
        // TODO: Assign in editor
        hitbox = GameObject.FindWithTag(Tags.PLAYER_HITBOX);

        // Hitbox management (crouched)
        coll = GetComponent<BoxCollider2D>();
        hitboxColl = hitbox.GetComponent<BoxCollider2D>();
        colliderFullSize = coll.size;
        colliderFullOffset = coll.offset;
        crouchHitboxOffset = (coll.size.y - coll.size.y / 2) / 2;
        hitboxFullSize = hitboxColl.size;
        hitboxFullOffset = hitboxColl.offset;
    }

    private void onControllerCollider(RaycastHit2D hit) {
        var go = hit.transform.gameObject;

        if (go.GetComponent<MovingPlatform>() != null) {
            transform.parent = hit.transform;
        }

        if (go.GetComponent<FallDown>() != null) {
            go.GetComponent<FallDown>().Touch();
        }

        if (go.GetComponent<MoveUpwards>() != null) {
            go.GetComponent<MoveUpwards>().Touch();
        }

        if (go.CompareTag(Tags.GROUND_COLLISION) && transform.parent != null) {
            transform.parent = null;
        }
    }

    public void onTriggerEnterEvent(Collider2D col) {
        if (col.CompareTag(Tags.ENEMY_HIT)) {
            overlapping = col.gameObject;
        }
        if (col.GetComponent<EnvironmentalDeath>() != null) {
            Die();
        }
    }

    public void SetHalo() {
        var time = GameplayValues.GetInvulnerableBoostTime();
        invuln.SetInvulnerableNoBlink(time);
        invulnerableAura.SetActive(true);
        AudioManager.GetInstance().PlaySecondaryMusic(Music.INVULNERABLE, time);
        haloActive = true;
    }

    private void onTriggerExitEvent(Collider2D col) {
        overlapping = null;
    }

    // For debugging the game.
    private void DebugModeHotkeys() {
        if (!GameState.debugActive) {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Q)) {
            GameState.holyWaters = 25;
        }
        if (Input.GetKeyDown(KeyCode.W)) {
            TeleportToBoss();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            GameState.difficulty = Difficulty.VERY_EASY;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            GameState.difficulty = Difficulty.EASY;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            GameState.difficulty = Difficulty.MEDIUM;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4)) {
            GameState.difficulty = Difficulty.HARD;
        }
        if (Input.GetKeyDown(KeyCode.Alpha5)) {
            GameState.difficulty = Difficulty.EXTREME;
        }
    }

    // Used in Debug mode and in Continue option.
    public void TeleportToBoss() {
        var sn = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        if (sn == Scenes.MONASTERY_ACT_2) {
            transform.position = new Vector2(108.73f, -10.348f);
        }
        if (sn == Scenes.NIGHT_BAR_ACT_2) {
            transform.position = new Vector2(194.6f, -24.856f);
        }
        if (sn == Scenes.HOSPITAL_ACT_2) {
            transform.position = new Vector2(81.08f, -12.358f);
        }
        if (sn == Scenes.DESERT_ACT_2) {
            transform.position = new Vector2(187.93f, -0.01f);
        }
        if (sn == Scenes.LAB_ACT_2) {
            transform.position = new Vector2(106.52f, -10.162f);
        }
        if (sn == Scenes.COMMIE_HQ_ACT_2) {
            transform.position = new Vector2(151.8f, -11.04f);
        }
    }

    private void Update() {
        DebugModeHotkeys();

        if (!GameState.isGameLocked) {
            // Time the character has been idle.
            lastAction += Time.deltaTime;

            var downPressed = inputManager.IsActionPressed(GameCommand.DOWN);
            var upPressed = inputManager.IsActionPressed(GameCommand.UP);
            var leftPressed = inputManager.IsActionPressed(GameCommand.LEFT);
            var rightPressed = inputManager.IsActionPressed(GameCommand.RIGHT);

            // Hit management on overlapping objects
            if (overlapping != null) {
                if (overlapping && overlapping.activeSelf && overlapping.CompareTag(Tags.ENEMY_HIT) && !invuln.IsInvulnerable()) {
                    EnemyHit();
                }
            }

            // Halo management, if active.
            if (haloActive && !invuln.IsInvulnerable()) {
                haloActive = false;
                invulnerableAura.SetActive(false);
            }

            // Crouch management.
            if (crouching && !inputManager.IsActionPressed(GameCommand.DOWN)) {
                Uncrouch();
            }

            // Looking up management.
            if (!inputManager.IsActionPressed(GameCommand.UP)) {
                lookingUp = false;
            }

            // Control grounded state and the related animations.
            if (controller.isGrounded) {
                AchievementManager.jumpKills = 0;
                velocity.y = 0;
                if (crouching) {
                    SetAnimation(Animations.NUN_CROUCHING);
                }
                if (lookingUp && !leftPressed && !rightPressed) {
                    SetAnimation(Animations.NUN_LOOKING_UP);
                }
            } else {
                // Clean parent from moving platforms if falling or jumping.
                if (transform.parent != null) {
                    transform.parent = null;
                }
            }

            // Restore one way platform being detected if not pressing crouch+jump
            if (controller.ignoringOneWayPlatforms && (!downPressed || !inputManager.IsActionPressed(GameCommand.JUMP))) {
                controller.ignoringOneWayPlatforms = false;
            }

            // Horizontal movement.
            if (canMove && rightPressed) {
                shootingDirection = Direction.RIGHT;
                lastAction = 0;
                normalizedHorizontalSpeed = 1;
                if (controller.isGrounded && !crouching) {
                    SetAnimation(Animations.NUN_WALKING);
                }
                if (!flipper.lookingRight) {
                    flipper.Flip();
                    shootingDirection = Direction.RIGHT;
                }
            } else if (canMove && leftPressed) {
                shootingDirection = Direction.LEFT;
                lastAction = 0;
                normalizedHorizontalSpeed = -1;
                if (controller.isGrounded && !crouching) {
                    SetAnimation(Animations.NUN_WALKING);
                }
                if (flipper.lookingRight) {
                    flipper.Flip();
                    shootingDirection = Direction.LEFT;
                }
            } else {
                normalizedHorizontalSpeed = 0;
                velocity.x = 0;

                // Set the standing up idle animation if there's no movement.
                if (canMove && !crouching && !lookingUp && controller.isGrounded && lastAction < 4.99f) {
                    SetAnimation(Animations.NUN_IDLE);
                }
            }

            // Shooting
            if (canMove && inputManager.IsActionPressed(GameCommand.SHOOT) && Time.time - lastProjectile >= projectileRatio) {
                AudioManager.GetInstance().PlayEffect(Sfx.SHOOT);
                lastAction = 0;
                ShootProjectile();
            }

            // Jump only while grounded.
            // If you're holding down, jump down a platform.
            if (canMove && controller.isGrounded && inputManager.IsActionPressedOnce(GameCommand.JUMP)) {
                if (downPressed) {
                    transform.parent = null;
                    controller.ignoringOneWayPlatforms = true;
                } else {
                    Jump(false);
                }
            }

            // This controls the variable jump. If the key stops being pressed, the velocity is set to 0.
            if (!ignoreJumpVariable && velocity.y > 0 && velocity.y < 6f && !inputManager.IsActionPressed(GameCommand.JUMP)) {
                velocity.y = 0;
            }

            // Crouch action.
            if (canMove && controller.isGrounded && downPressed) {
                lastAction = 0;
                Crouch();
            }

            // Look up, only possible while in an idle animation (not moving).
            if (canMove && controller.isGrounded && upPressed && idleAnims.Contains(currentAnimation)) {
                lastAction = 0;
                lookingUp = true;
            }

            // Apply horizontal speed.
            var smoothedMovementFactor = controller.isGrounded ? groundDamping : inAirDamping;
            var actualRunSpeed = speedBuffed ? runSpeed * 2 : runSpeed;
            if (crouching) {
                actualRunSpeed /= 2;
            }
            velocity.x = Mathf.Lerp(velocity.x, normalizedHorizontalSpeed * actualRunSpeed, Time.deltaTime * smoothedMovementFactor);

            // Apply gravity before moving.
            velocity.y += gravity * Time.deltaTime;
            velocity.y = Mathf.Clamp(velocity.y, -18f, 18f);

            // Jumping sprite
            if (velocity.y > 0.8f || velocity.y < -0.8f) {
                SetAnimation(Animations.NUN_JUMPING);
            }

            controller.move(velocity * Time.deltaTime);

            // Grab our current velocity to use as a base for all calculations.
            velocity = controller.velocity;

            // Speed booooooooost
            if (speedBuffed && Time.time - speedBuffApplied >= GameplayValues.GetSpeedBuffDuration()) {
                speedBuffed = false;
                audioManager.UndoFasterSong();
            }
            
            // Idle animation when no movement after 5 seconds.
            if (lastAction > 5f && lastAction <= 7.5f) {
                SetAnimation(Animations.NUN_WAITING);
            }
            if (lastAction > 7.5f && lastAction <= 16f) {
                SetAnimation(Animations.NUN_BORED);
            }
            if (lastAction > 16f) {
                SetAnimation(Animations.NUN_PRAYING);
            }
        }
    }

    public void SetCanMove(bool canMove) {
        this.canMove = canMove;
    }

    public bool CanMove() {
        return canMove;
    }

    public void BossPray() {
        SetAnimation(Animations.NUN_PRAYING_BOSS);
    }

    // Pass an animation to play to the animator.
    private void SetAnimation(string name) {
        if (currentAnimation != name) {
            currentAnimation = name;
            if (gameObject.activeSelf) {
                animator.Play(Animator.StringToHash(name));
            }
        }
    }

    private void ShootProjectile() {
        crossPool[crossPoolIndex].transform.position = transform.position;
        crossPool[crossPoolIndex].SetActive(true);
        crossPool[crossPoolIndex].GetComponent<CrossThrow>().BeginShoot();
        crossPoolIndex++;
        if (crossPoolIndex > crossPool.Length - 1) {
            crossPoolIndex = 0;
        }
        lastProjectile = Time.time;
    }

    public bool IsInvulnerable() {
        return invuln.IsInvulnerable();
    }

    public void EnemyHit() {
        hitReceiver.ReceiveHit();
        if (GameState.holyWaters < 0) {
            Die();
        }
    }

    public void Jump(bool automatic) {
        lastAction = 0;
        var actualjumpHeight = speedBuffed ? jumpHeight * 2 : jumpHeight;
        if (automatic) {
            actualjumpHeight /= 2;
            ignoreJumpVariable = true;
        } else {
            ignoreJumpVariable = false;
        }

        velocity.y = Mathf.Sqrt(2f * actualjumpHeight * -gravity);
        controller.ignoreOneWayPlatformsThisFrame = true;
        AudioManager.GetInstance().PlayEffect(Sfx.JUMP);
        SetAnimation(Animations.NUN_JUMPING);
    }

    private void Crouch() {
        if (!crouching) {
            crouching = true;
            HalveHitbox();
        }
    }

    private void Uncrouch() {
        if (crouching) {
            crouching = false;
            FullHitbox();
        }
    }

    private void HalveHitbox() {
        if (hitbox != null) {
            coll.size = new Vector2(colliderFullSize.x, colliderFullSize.y / 2);
            coll.offset = new Vector2(colliderFullOffset.x, colliderFullOffset.y - crouchHitboxOffset);
            hitboxColl.size = new Vector2(hitboxFullSize.x, hitboxFullSize.y / 2);
            hitboxColl.offset = new Vector2(hitboxFullOffset.x, hitboxFullOffset.y - crouchHitboxOffset);
            controller.recalculateDistanceBetweenRays();
        }
    }

    private void FullHitbox() {
        if (hitbox != null) {
            coll.size = new Vector2(colliderFullSize.x, colliderFullSize.y);
            coll.offset = new Vector2(colliderFullOffset.x, colliderFullOffset.y);
            hitboxColl.size = new Vector2(hitboxFullSize.x, hitboxFullSize.y);
            hitboxColl.offset = new Vector2(hitboxFullOffset.x, hitboxFullOffset.y);
            controller.recalculateDistanceBetweenRays();
        }
    }

    public bool IsCrouching() {
        return crouching;
    }

    // TODO: Refactor
    public void FeetCollided() {
        FeetCollided(null);
    }

    public void FeetCollided(GameObject go) {
        if (go != null && (velocity.y < -0.99f || velocity.y > 0.99f) && go.GetComponent<PlayerHitReceiver>() != null && go.GetComponent<IEnemyEntity>() != null) {
            invuln.SetInvulnerableNoBlink(0.2f);
            Jump(true);
            if (go != null) {
                go.GetComponent<PlayerHitReceiver>().ReceiveHit(true);
            }
        }
    }

    // Starts the process of dying before the screen is reloaded.
    private void Die() {
        if (!dying) {
            AudioManager.GetInstance().UndoFasterSong();
            dying = true;
            GameState.activatingBoss = false;
            GameState.bossActive = false;

            // Show the angel nun going to heaven.
            var ded = Instantiate(deadNunResource, transform.position, Quaternion.identity);
            ded.GetComponent<DeadNun>().Activate();
            gameObject.SetActive(false);
        }
    }

    public void SpeedBuff() {
        speedBuffed = true;
        speedBuffApplied = Time.time;
        AudioManager.GetInstance().FasterSong();
    }

    public Vector3 GetVelocity() {
        return velocity;
    }
}

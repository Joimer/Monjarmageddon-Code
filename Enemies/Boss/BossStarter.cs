using UnityEngine;

public abstract class BossStarter : MonoBehaviour {

    public static int bossStage = 0;
    public GameObject bossText;
    protected GameObject bossPrefab;
    protected GameObject boss;
    protected Vector2 initialPos;
    protected Vector2 bossColliderLeft;
    protected Vector2 bossColliderRight;
    protected GameObject bossColliderLeftObj;
    protected GameObject bossColliderRightObj;
    protected float startingPoint;
    protected float yBossPoint;
    protected float downSpeed = -0.03f;
    protected bool locksCamera = true;

    public void OnTriggerEnter2D(Collider2D collision) {
        if (bossStage == 0 && collision.CompareTag(Tags.PLAYER)) {
            AudioManager.GetInstance().PlayMusic(Music.BOSS);
            bossStage = 1;
            GameState.GetInstance().isCameraLocked = locksCamera;
            InstanceBoss();
        }
    }

    private void InstanceBoss() {
        boss = Instantiate(bossPrefab, initialPos, Quaternion.identity);
    }

    protected void FixedUpdate() {
        if (!GameState.isGameLocked) {
            if (bossStage == 1) {
                GameState.activatingBoss = true;
                var bc = Resources.Load<GameObject>(Items.BOSS_COLLIDER);
                bossColliderLeftObj = Instantiate(bc, bossColliderLeft, Quaternion.identity);
                bossColliderRightObj = Instantiate(bc, bossColliderRight, Quaternion.identity);
                bossStage = 2;
            }

            if (bossStage == 2 && boss != null) {
                if (boss.transform.position.y > yBossPoint) {
                    boss.transform.Translate(new Vector2(0f, downSpeed));
                } else {
                    bossStage = 3;
                    GameState.activatingBoss = false;
                    GameState.bossActive = true;
                    if (boss != null) {
                        boss.GetComponent<BossEntity>().Activate();
                    }
                }
            }

            if (bossStage == 4) {
                Destroy(boss);
            }
        }
    }

    // For external events to increase the boss stage.
    public void IncreaseStage() {
        bossStage += 1;
    }
}

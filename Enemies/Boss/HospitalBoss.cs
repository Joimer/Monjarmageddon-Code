using UnityEngine;

public class HospitalBoss : MonoBehaviour {

    public GameObject bossTextGO;
    private GameObject target;
    private int bossStage = 0;
    private GameObject boss;
    private Vector2 initialPos = new Vector2(96.9129f, -5f);
    private Vector2 bossColliderLeft = new Vector2(91.837f, -6.702f);
    private Vector2 bossColliderRight = new Vector2(97.74f, -6.926f);
    private GameObject bossCollider;
    private float yBossPoint = -6.43f;
    private float downSpeed = -0.07f;
    private RpgDialog dialog;
    private string bossName = "Aborting Mom";
    private float prayStart = 0f;
    private int stageWhenFinishDownwards = 3;

    private void Awake() {
        boss = Instantiate(Resources.Load<GameObject>(Bosses.HOSPITAL), initialPos, Quaternion.identity);
        bossCollider = Resources.Load<GameObject>(Items.BOSS_COLLIDER);
        bossName = TextManager.GetText("hospital_boss_name");
    }

    private void Start() {
        target = ObjectLocator.GetPlayer();
    }

    public void OnTriggerEnter2D(Collider2D collision) {
        if (bossStage == 0 && collision.CompareTag(Tags.PLAYER)) {
            bossStage = 1;
            dialog = gameObject.AddComponent<RpgDialog>();
            dialog.onFinish += FinishDialog;
            dialog.Activate(TextManager.GetText("hospital_boss_text"), bossName, bossTextGO);
            GameState.GetInstance().isCameraLocked = true;
            AudioManager.GetInstance().StopAllMusic();
            target.GetComponent<PlatformerMovement2D>().SetCanMove(false);
        }
    }

    public void FixedUpdate() {
        if (!GameState.isGameLocked) {
            if (bossStage == 1) {
                GameState.activatingBoss = true;
                Instantiate(bossCollider, bossColliderLeft, Quaternion.identity);
                Instantiate(bossCollider, bossColliderRight, Quaternion.identity);
                bossStage = 2;
            }

            if (bossStage == 2 && boss != null) {
                if (boss.transform.position.y > yBossPoint) {
                    boss.transform.Translate(new Vector2(0f, downSpeed));
                } else {
                    bossStage = stageWhenFinishDownwards;
                }
            }

            if (bossStage == 4) {
                bossStage = 5;
                target.GetComponent<PlatformerMovement2D>().BossPray();
                prayStart = Time.time;
            }

            if (bossStage == 5 && Time.time - prayStart >= 1.6f) {
                target.GetComponent<PlatformerMovement2D>().SetCanMove(true);
                AudioManager.GetInstance().PlayMusic(Music.BOSS);
                GameState.activatingBoss = false;
                GameState.bossActive = true;
                if (boss != null) {
                    boss.GetComponent<BossEntity>().Activate();
                }
                bossStage++;
            }
        }
    }

    private void FinishDialog() {
        if (bossStage == 3) {
            bossStage = 4;
        } else {
            stageWhenFinishDownwards = 4;
        }
    }
}

using UnityEngine;

public class NightBarBoss : MonoBehaviour {

    public GameObject bossTextGO;
    private GameObject target;
    private int bossStage = 0;
    private GameObject boss;
    private Vector2 initialPos = new Vector2(201.32f, -20.66f);
    private Vector2 bossColliderLeft = new Vector2(195.783f, -24.11f);
    private Vector2 bossColliderRight = new Vector2(205.9472f, -24.58f);
    private GameObject bossCollider;
    private float yBossPoint = -23.8f;
    private float downSpeed = -0.07f;
    private RpgDialog dialog;
    private string bossName = "Night Queen";
    private float prayStart = 0f;
    private int stageWhenFinishDownwards = 3;

    private void Awake() {
        boss = Instantiate(Resources.Load<GameObject>(Bosses.NIGHT_BAR), initialPos, Quaternion.identity);
        bossCollider = Resources.Load<GameObject>(Items.BOSS_COLLIDER);
        bossName = TextManager.GetText("nightbar_boss_name");
    }

    private void Start() {
        target = ObjectLocator.GetPlayer();
    }

    public void OnTriggerEnter2D(Collider2D collision) {
        if (bossStage == 0 && collision.CompareTag(Tags.PLAYER)) {
            bossStage = 1;
            dialog = gameObject.AddComponent<RpgDialog>();
            dialog.onFinish += FinishDialog;
            dialog.Activate(TextManager.GetText("nightbar_boss_text"), bossName, bossTextGO);
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

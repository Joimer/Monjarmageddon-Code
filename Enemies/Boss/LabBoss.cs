using UnityEngine;

public class LabBoss : MonoBehaviour {

    public GameObject bossTextGO;
    public GameObject boss;
    public GameObject backRay;
    private GameObject target;
    private int bossStage = 0;
    private RpgDialog dialog;
    private string bossName = "Scientist";
    private float prayStart = 0f;

    public void Start() {
        target = ObjectLocator.GetPlayer();
    }

    public void OnTriggerEnter2D(Collider2D collision) {
        if (bossStage == 0 && collision.CompareTag(Tags.PLAYER)) {
            bossStage = 1;
            dialog = gameObject.AddComponent<RpgDialog>();
            dialog.onFinish += FinishDialog;
            dialog.Activate(TextManager.GetText("lab_boss_text"), bossName, bossTextGO);
            boss.GetComponent<Scientist>().SetCounter(0, 0);
            backRay.SetActive(true);
            AudioManager.GetInstance().StopAllMusic();
            target.GetComponent<PlatformerMovement2D>().SetCanMove(false);
        }
    }

    protected void FixedUpdate() {
        if (!GameState.isGameLocked) {
            if (bossStage == 2) {
                bossStage = 3;
                target.GetComponent<PlatformerMovement2D>().BossPray();
                prayStart = Time.time;
            }

            if (bossStage == 3 && Time.time - prayStart >= 1.6f) {
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
        bossStage = 2;
        boss.GetComponent<Scientist>().SetCounter();
    }
}

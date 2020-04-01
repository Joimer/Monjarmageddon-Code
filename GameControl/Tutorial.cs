using UnityEngine;

public class Tutorial : MonoBehaviour {

    public GameObject dialogBox;
    private bool jumpStep = false;
    private bool holdJumpStep = false;
    private bool jumpDownStep = false;
    private bool shootStep = false;
    private bool lookUpStep = false;
    private bool lookDownStep = false;
    private GameObject target;
    private RpgDialog dialog;

    private void Awake() {
        target = ObjectLocator.GetPlayer();
        dialog = gameObject.AddComponent<RpgDialog>();
        dialog.onFinish += FinishDialog;
    }

    private void Start() {
        var movementText = InputManager.keyboardConfig[GameCommand.RIGHT].ToString() + ": "
            + TextManager.GetText("move right") + "\n"
            + InputManager.keyboardConfig[GameCommand.LEFT].ToString() + ": "
            + TextManager.GetText("move left");
        dialog.Activate(movementText, dialogBox);
        target.GetComponent<PlatformerMovement2D>().SetCanMove(false);
    }

    private void Update() {
		if (!jumpStep && target.transform.position.x > 0.93f) {
            jumpStep = true;
            var jumpText = InputManager.keyboardConfig[GameCommand.JUMP].ToString() + ": "
                + TextManager.GetText("jump");
            dialog.Activate(jumpText, dialogBox);
            target.GetComponent<PlatformerMovement2D>().SetCanMove(false);
        }

        if (!holdJumpStep && target.transform.position.x > 2f) {
            holdJumpStep = true;
            var jumpText = InputManager.keyboardConfig[GameCommand.JUMP].ToString() + " (" + TextManager.GetText("hold") + ").";
            dialog.Activate(jumpText, dialogBox);
            target.GetComponent<PlatformerMovement2D>().SetCanMove(false);
        }

        if (!jumpDownStep) {
            jumpDownStep = true;
            var jumpText = InputManager.keyboardConfig[GameCommand.JUMP].ToString() + " + "
                + InputManager.keyboardConfig[GameCommand.DOWN].ToString() + ": "
                + TextManager.GetText("jump down");
            dialog.Activate(jumpText, dialogBox);
            target.GetComponent<PlatformerMovement2D>().SetCanMove(false);
        }

        if (!shootStep) {
            shootStep = true;
            var jumpText = InputManager.keyboardConfig[GameCommand.SHOOT].ToString() + ": "
                + TextManager.GetText("shoot");
            dialog.Activate(jumpText, dialogBox);
            target.GetComponent<PlatformerMovement2D>().SetCanMove(false);
        }

        if (!lookUpStep) {
            lookUpStep = true;
            var jumpText = InputManager.keyboardConfig[GameCommand.JUMP].ToString() + ": "
                + TextManager.GetText("look up");
            dialog.Activate(jumpText, dialogBox);
            target.GetComponent<PlatformerMovement2D>().SetCanMove(false);
        }


        if (!lookDownStep) {
            lookDownStep = true;
            var jumpText = InputManager.keyboardConfig[GameCommand.JUMP].ToString() + ": "
                + TextManager.GetText("crouch");
            dialog.Activate(jumpText, dialogBox);
            target.GetComponent<PlatformerMovement2D>().SetCanMove(false);
        }
    }

    private void FinishDialog() {
        target.GetComponent<PlatformerMovement2D>().SetCanMove(true);
    }
}

using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class RpgDialog : MonoBehaviour {

    public event Action onFinish;
    private GameObject dialogBox;
    private Text text;
    private int characterIndex = 0;
    private float lastUpdate = 0f;
    private float textSpeed = 0.1f;
    private string content = "";
    private string title = "";
    private bool started = false;
    private bool spelling = false;
    private float timeToLeave;
    private float deactivateAt;
    private bool withTitle = true;

    public void Activate(string text, string title, GameObject box) {
        if (this.text != null) {
            this.text.text = "";
        }
        withTitle = true;
        this.title = title;
        dialogBox = box;
        Activate(text);
    }

    public void Activate(string text, GameObject box) {
        if (this.text != null) {
            this.text.text = "";
        }
        withTitle = false;
        dialogBox = box;
        Activate(text);
    }

    private void Activate(string text) {
        characterIndex = 0;
        lastUpdate = 0f;
        content = text;
        this.text = dialogBox.GetComponentInChildren<Text>();
        this.text.fontSize = GameState.lang == SystemLanguage.Japanese ? UiTexts.MENU_JAPANESE_TEXT_SIZE : UiTexts.MENU_TEXT_SIZE;
        started = true;
        dialogBox.SetActive(true);
        timeToLeave = text.Length * 0.33f;
        spelling = true;
    }

    public void Update() {
        if (started && !GameState.isGameLocked) {
            var im = InputManager.GetInstance();
            if (!spelling && started && (Time.time >= deactivateAt || im.IsActionPressedOnce(GameCommand.JUMP))) {
                CloseDialog();
            }

            if (spelling && im.IsActionPressedOnce(GameCommand.JUMP)) {
                characterIndex = content.Length;
                PrintText();
                SetForDeactivation();
                AudioManager.GetInstance().PlayEffect(Sfx.TICK);
            }

            if (spelling) {
                lastUpdate += Time.deltaTime;
                if (lastUpdate >= textSpeed) {
                    lastUpdate -= textSpeed;
                    if (characterIndex == content.Length) {
                        SetForDeactivation();
                    } else {
                        characterIndex++;
                        AudioManager.GetInstance().PlayEffect(Sfx.TICK);
                        PrintText();
                    }
                }
            }
        }
    }

    private void SetForDeactivation() {
        characterIndex = content.Length;
        spelling = false;
        deactivateAt = Time.time + timeToLeave;
    }

    private void PrintText() {
        string firstPart = "";
        for (var i = 0; i < characterIndex; i++) {
            firstPart += content[i];
        }
        var secondPart = content.Substring(characterIndex);
        var newText = new StringBuilder("");
        if (withTitle) {
            newText.Append("<b>" + title + "</b>\n");
        }
        newText.Append(firstPart + "<color=#727272>" + secondPart + "</color>");
        text.text = newText.ToString();
    }

    private void CloseDialog() {
        if (onFinish != null) {
            onFinish();
            lastUpdate = 0f;
            characterIndex = -1;
        }
        started = false;
        dialogBox.SetActive(false);
    }
}

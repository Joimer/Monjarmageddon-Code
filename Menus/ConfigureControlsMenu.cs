using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class ConfigureControlsMenu : MonoBehaviour {

    // Menu titles.
    public GameObject pressKeyPanel;
    public Text pressKeyText;
    public RectTransform nunIcon;
    public Text configureTitle;
    public Text keyboardTitle;
    public Text gamepadTitle;
    public Text jump;
    public Text shoot;
    public Text lookUp;
    public Text crouch;
    public Text moveLeft;
    public Text moveRight;
    public Text start;
    public Text back;

    // Key names.
    public Text jumpKey;
    public Text shootKey;
    public Text lookupKey;
    public Text crouchKey;
    public Text moveleftKey;
    public Text moverightKey;
    public Text startKey;

    // Pad names.
    public Text jumpPad;
    public Text shootPad;
    public Text lookupPad;
    public Text crouchPad;
    public Text moveleftPad;
    public Text moverightPad;
    public Text startPad;

    // Flow control.
    private GameState gs;
    private Font font;
    private Texture2D icon;
    private int optionIndex = 0;
    private int horizontalIndex = 0;
    private bool switchingScene = false;
    private bool assigningKey = false;
    private bool assigningPad = false;
    private const string menuSlash = "-";

    // Convenient to manage key assign and deassign.
    private readonly GameCommand[] commands = new GameCommand[] { GameCommand.JUMP, GameCommand.SHOOT, GameCommand.UP, GameCommand.DOWN, GameCommand.LEFT, GameCommand.RIGHT, GameCommand.ACCEPT };
    private Dictionary<KeyCode, GameCommand> assignedCommands;
    private Dictionary<GamepadKey, GameCommand> assignedPadCommands;
    private Dictionary<GameCommand, Text> keyToName;
    private Dictionary<GameCommand, Text> gamepadKeyToName;

    // Managers
    private InputManager inputManager;
    private AudioManager audioManager;

    private void Awake() {
        gs = GameState.GetInstance();
        inputManager = InputManager.GetInstance();
        audioManager = AudioManager.GetInstance();
        pressKeyPanel.SetActive(false);

        keyToName = new Dictionary<GameCommand, Text>() {
            { GameCommand.JUMP, jumpKey },
            { GameCommand.SHOOT, shootKey },
            { GameCommand.UP, lookupKey },
            { GameCommand.DOWN, crouchKey },
            { GameCommand.LEFT, moveleftKey },
            { GameCommand.RIGHT, moverightKey },
            { GameCommand.ACCEPT, startKey }
        };

        gamepadKeyToName = new Dictionary<GameCommand, Text>() {
            { GameCommand.JUMP, jumpPad },
            { GameCommand.SHOOT, shootPad },
            { GameCommand.UP, lookupPad },
            { GameCommand.DOWN, crouchPad },
            { GameCommand.LEFT, moveleftPad },
            { GameCommand.RIGHT, moverightPad },
            { GameCommand.ACCEPT, startPad }
        };

        FillDefaultKeys();
        UpdateAllTexts();
    }

    private void FillDefaultKeys() {
        assignedCommands = new Dictionary<KeyCode, GameCommand>();
        assignedPadCommands = new Dictionary<GamepadKey, GameCommand>();
        foreach (var keyPair in InputManager.keyboardConfig) {
            if (!assignedCommands.ContainsKey(keyPair.Value)) {
                assignedCommands.Add(keyPair.Value, keyPair.Key);
            }
        }
        foreach (var keyPair in InputManager.gamepadConfig) {
            if (!assignedPadCommands.ContainsKey(keyPair.Value)) {
                assignedPadCommands[keyPair.Value] = keyPair.Key;
            }
        }
    }

    private void UpdateAllTexts() {
        pressKeyText.text = "\n-- " + TextManager.GetText("press key") + " --";
        configureTitle.text = TextManager.GetText("configure controls");
        keyboardTitle.text = TextManager.GetText("keyboard");
        gamepadTitle.text = TextManager.GetText("gamepad");

        jump.text = TextManager.GetText("jump");
        jumpKey.text = InputManager.keyboardConfig[GameCommand.JUMP].ToString();
        jumpPad.text = InputManager.gamepadConfig[GameCommand.JUMP].ToString();

        shoot.text = TextManager.GetText("shoot");
        shootKey.text = InputManager.keyboardConfig[GameCommand.SHOOT].ToString();
        shootPad.text = InputManager.gamepadConfig[GameCommand.SHOOT].ToString();

        lookUp.text = TextManager.GetText("look up");
        lookupKey.text = InputManager.keyboardConfig[GameCommand.UP].ToString();
        lookupPad.text = InputManager.gamepadConfig[GameCommand.UP].ToString();

        crouch.text = TextManager.GetText("crouch");
        crouchKey.text = InputManager.keyboardConfig[GameCommand.DOWN].ToString();
        crouchPad.text = InputManager.gamepadConfig[GameCommand.DOWN].ToString();

        moveLeft.text = TextManager.GetText("move left");
        moveleftKey.text = InputManager.keyboardConfig[GameCommand.LEFT].ToString();
        moveleftPad.text = InputManager.gamepadConfig[GameCommand.LEFT].ToString();

        moveRight.text = TextManager.GetText("move right");
        moverightKey.text = InputManager.keyboardConfig[GameCommand.RIGHT].ToString();
        moverightPad.text = InputManager.gamepadConfig[GameCommand.RIGHT].ToString();

        start.text = "Start";
        startKey.text = InputManager.keyboardConfig[GameCommand.ACCEPT].ToString();
        startPad.text = InputManager.gamepadConfig[GameCommand.ACCEPT].ToString();

        back.text = TextManager.GetText("back");
    }

    private void Update() {
        // Manage column in which the cursor is present.
        if (!assigningKey && inputManager.IsMenuPressRight() && !switchingScene && !assigningKey) {
            audioManager.PlayEffect(Sfx.MENU_BEEP);
            horizontalIndex++;
            if (horizontalIndex == 2) {
                horizontalIndex = 0;
            }
            UpdateNunIconPosition();
        }

        if (!assigningKey && inputManager.IsMenuPressLeft() && !switchingScene && !assigningKey) {
            audioManager.PlayEffect(Sfx.MENU_BEEP);
            horizontalIndex--;
            if (horizontalIndex == -1) {
                horizontalIndex = 1;
            }
            UpdateNunIconPosition();
        }

        // Going up and down in the control you want to update.
        if (!assigningKey && inputManager.IsMenuPressUp() && !switchingScene && !assigningKey) {
            audioManager.PlayEffect(Sfx.MENU_BEEP);
            optionIndex--;
            if (optionIndex == -1) {
                optionIndex = 7;
            }
            UpdateNunIconPosition();
        }

        if (!assigningKey && inputManager.IsMenuPressDown() && !switchingScene && !assigningKey) {
            audioManager.PlayEffect(Sfx.MENU_BEEP);
            optionIndex++;
            if (optionIndex == 8) {
                optionIndex = 0;
            }
            UpdateNunIconPosition();
        }

        // Going back to main menu.
        if (optionIndex == 7 && inputManager.IsMenuStart() && !switchingScene) {
            audioManager.PlayEffect(Sfx.MENU_ACCEPT);
            switchingScene = true;
            GameConfig gc = SaveManager.LoadGameConfig();
            gc.keyboardConfig = InputManager.keyboardConfig;
            gc.padConfig = InputManager.gamepadConfig;
            SaveManager.SaveGameConfig(gc);
            gs.LoadScene(Scenes.OPTIONS, 0.5f);
        }

        // Assigning a key.
        if (assigningKey) {
            foreach (KeyCode key in Enum.GetValues(typeof(KeyCode))) {
                // Keyboard configuration.
                if (Input.GetKeyDown(key)) {
                    // Escape is a special key to exit the assignment menu.
                    if (key == KeyCode.Escape) {
                        DeactivateAssignment();
                        return;
                    }
                    // If they key is assignable to this action, update the configuration.
                    if (!IgnoreKey(key)) {
                        UpdateGameKey(commands[optionIndex], key);
                        UpdateControlText(key.ToString());
                        DeactivateAssignment();
                        return;
                    }
                }
            }
        }

        // Assigning a game pad button or axis.
        if (assigningPad) {
            foreach (KeyCode key in Enum.GetValues(typeof(KeyCode))) {
                // Keyboard configuration.
                if (Input.GetKeyDown(key)) {
                    // Escape is a special key to exit the assignment menu.
                    if (key == KeyCode.Escape) {
                        DeactivateAssignment();
                        return;
                    }
                    if (!IgnoreKey(key)) {
                        UpdateGamePadKey(commands[optionIndex], new GamepadKey(key));
                        UpdateControlText(key.ToString());
                        DeactivateAssignment();
                        return;
                    }
                }
            }

            // Axis configuration.
            // One menu tick to check if an axis is active before the player presses it so we know to nullify it.
            var update = false;
            string axis = "";
            bool positive = true;
            var treshold = inputManager.GetGamepadTreshold();

            if (Input.GetAxis(GamepadAxis.HORIZONTAL) != 0) {
                axis = GamepadAxis.HORIZONTAL;
                positive = Input.GetAxis(GamepadAxis.HORIZONTAL) > treshold;
                update = true;
            }
            if (Input.GetAxis(GamepadAxis.VERTICAL) != 0) {
                axis = GamepadAxis.VERTICAL;
                positive = Input.GetAxis(GamepadAxis.VERTICAL) > treshold;
                update = true;
            }
            if (Input.GetAxis(GamepadAxis.HORIZONTAL2) != 0) {
                axis = GamepadAxis.HORIZONTAL2;
                positive = Input.GetAxis(GamepadAxis.HORIZONTAL2) > treshold;
                update = true;
            }
            if (Input.GetAxis(GamepadAxis.VERTICAL2) != 0) {
                axis = GamepadAxis.VERTICAL2;
                positive = Input.GetAxis(GamepadAxis.VERTICAL2) > treshold;
                update = true;
            }
            if (Input.GetAxis(GamepadAxis.HORIZONTAL3) != 0) {
                axis = GamepadAxis.HORIZONTAL3;
                positive = Input.GetAxis(GamepadAxis.HORIZONTAL3) > treshold;
                update = true;
            }
            if (Input.GetAxis(GamepadAxis.VERTICAL3) != 0) {
                axis = GamepadAxis.VERTICAL3;
                positive = Input.GetAxis(GamepadAxis.VERTICAL3) > treshold;
                update = true;
            }

            // If any axis was active during detection, it is assigned.
            if (update) {
                var gpkey = new GamepadKey(axis, positive);
                UpdateGamePadKey(commands[optionIndex], gpkey);
                UpdateControlText(gpkey.ToString());
                DeactivateAssignment();
                return;
            }
        }

        // Activating the "assign key" option.
        if (!assigningKey && inputManager.IsMenuStart() && !switchingScene) {
            ActivateAssignment();
        }

        // Fade out.
        if (switchingScene) {
            var color = GetComponent<SpriteRenderer>().color;
            color.a += 0.03f;
            GetComponent<SpriteRenderer>().color = color;
        }
    }

    private void UpdateGameKey(GameCommand cmd, KeyCode key) {
        inputManager.SetCommand(cmd, key);
        // Remove the old entry.
        KeyCode toRemove = KeyCode.None;
        foreach (KeyValuePair<KeyCode, GameCommand> entry in assignedCommands) {
            if (cmd == entry.Value) {
                toRemove = entry.Key;
            }
        }
        if (toRemove != KeyCode.None) {
            assignedCommands.Remove(toRemove);
        }
        // If the new key is being used elsewhere, remove that entry too.
        if (assignedCommands.ContainsKey(key)) {
            var command = assignedCommands[key];
            var text = keyToName[command];
            text.text = "-";
            assignedCommands.Remove(key);
        }
        // And add the new entry.
        assignedCommands.Add(key, cmd);
    }

    private void UpdateGamePadKey(GameCommand cmd, GamepadKey key) {
        inputManager.SetGamepadCommand(cmd, key);
        // First, remove the actual saved gamepad key for command.
        GamepadKey toRemove = null;
        foreach (KeyValuePair<GamepadKey, GameCommand> entry in assignedPadCommands) {
            if (cmd == entry.Value) {
                toRemove = entry.Key;
            }
        }
        if (toRemove != null) {
            RemoveFormerPadKey(toRemove);
        }
        // Remove pad key if it was used elsewhere.
        toRemove = null;
        foreach (var cmdKey in assignedPadCommands.Keys) {
            if (cmdKey.ToString() == key.ToString()) {
                toRemove = cmdKey;
                break;
            }
        }
        if (toRemove != null) {
            RemoveFormerPadKey(toRemove);
        }
        assignedPadCommands.Add(key, cmd);
    }

    private void RemoveFormerPadKey(GamepadKey gpkey) {
        var command = assignedPadCommands[gpkey];
        var text = gamepadKeyToName[command];
        text.text = menuSlash;
        // TODO: Shouldn't be necessary, seek when does this state happen.
        if (assignedPadCommands.ContainsKey(gpkey)) {
            assignedPadCommands.Remove(gpkey);
        }
    }

    // Ignore gamepad when configuring keyboard and otherwise.
    private bool IgnoreKey(KeyCode key) {
        var strKey = key.ToString();
        // TODO: Move to a dict
        const string joystick = "Joystick";
        const string vertical = "Vertical";
        const string horizontal = "Horizontal";
        return 
            (horizontalIndex == 0
            && (
                (strKey.Length > 7 && strKey.Substring(0, 8) == joystick) 
                || strKey.Contains(vertical)
                || strKey.Contains(horizontal)
            ))
            || horizontalIndex == 1 && (strKey.Length < 8 || strKey.Substring(0, 8) != joystick);
    }

    private void UpdateControlText(string newText) {
        Text text = GetTextToUpdate();
        text.text = newText;
    }

    // Get the text object to display they key/button name.
    private Text GetTextToUpdate() {
        var indexToText = new Text[] {
            jumpKey, shootKey, lookupKey, crouchKey, moveleftKey, moverightKey, startKey
        };
        if (horizontalIndex == 1) {
            indexToText = new Text[] {
                jumpPad, shootPad, lookupPad, crouchPad, moveleftPad, moverightPad, startPad
            };
        }
        return indexToText[optionIndex];
    }

    private void ActivateAssignment() {
        if (horizontalIndex == 1) {
            assigningPad = true;
        } else {
            assigningKey = true;
        }
        inputManager.SetIgnoreMenuTick(true);
        pressKeyPanel.SetActive(true);
    }

    // Hide the "press key" popup.
    private void DeactivateAssignment() {
        inputManager.MenuKeyTick();
        assigningKey = false;
        assigningPad = false;
        pressKeyPanel.SetActive(false);
        inputManager.SetIgnoreMenuTick(false);
    }

    // Move around the nun icon to show what option is being changed.
    private void UpdateNunIconPosition() {
        var x = (horizontalIndex == 0) ? -122.6f : 91f;
        float y;
        if (optionIndex == 7) {
            y = -155f;
            x = -48f;
        } else {
            y = 125.5f - optionIndex * 32.5f;
        }
        nunIcon.anchoredPosition = new Vector2(x, y);
    }
}

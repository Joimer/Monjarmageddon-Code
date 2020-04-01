using UnityEngine;
using System;
using System.Collections.Generic;

public class InputManager : MonoBehaviour {

    public static InputManager instance;

    // Keyboard configuration for the game.
    public static Dictionary<GameCommand, KeyCode> keyboardConfig = GameplayValues.defaultKeyboardConfig;

    // Gamepad button configuration.
    public static Dictionary<GameCommand, GamepadKey> gamepadConfig = GameplayValues.defaultPadConfig;

    // Gamepad treshold for sensitivity.
    private float gamePadTreshold = 0.5f;

    // Gamepad delay between axis reading in menus.
    private float gamepadTickDelay = 0.15f;
    private float lastMenuKeyTick = 0f;
    public bool gamePadConfigured = false;
    private bool ignoreMenuTick = false;

    // Unity singleton stuff :(
    public static InputManager GetInstance() {
        if (instance == null) {
            instance = new GameObject("Input Manager").AddComponent<InputManager>();
            instance.SetDefault();
        }
        return instance;
    }

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
        DontDestroyOnLoad(instance);
    }

    // Default keybindings for both keyboard and gamepad.
    public void SetDefault() {
        keyboardConfig = new Dictionary<GameCommand, KeyCode>() {
            { GameCommand.JUMP, KeyCode.Z },
            { GameCommand.SHOOT, KeyCode.X },
            { GameCommand.UP, KeyCode.UpArrow },
            { GameCommand.DOWN, KeyCode.DownArrow },
            { GameCommand.LEFT, KeyCode.LeftArrow },
            { GameCommand.RIGHT, KeyCode.RightArrow },
            { GameCommand.ACCEPT, KeyCode.Return }
        };
        gamepadConfig = GetGenericPadConfig();
        gamePadConfigured = false;
    }

    // Set a keyboard key.
    public void SetCommand(GameCommand command, KeyCode key) {
        keyboardConfig[command] = key;
    }

    // Set a gamepad button (can be a button or an axis press).
    public void SetGamepadCommand(GameCommand command, GamepadKey key) {
        gamepadConfig[command] = key;
        gamePadConfigured = true;
    }

    public void SetGamepadCommand(GameCommand command, string axis, bool positive) {
        gamepadConfig[command] = new GamepadKey(axis, positive);
        gamePadConfigured = true;
    }

    public float GetGamepadTreshold() {
        return gamePadTreshold;
    }

    public void SetGamepadTreshold(float tr) {
        gamePadTreshold = tr;
    }

    // Update all keybinds at once.
    public void UpdateKeybinds(Dictionary<GameCommand, KeyCode> keyboardConfig) {
        InputManager.keyboardConfig = keyboardConfig;
    }

    // Update all gamepad bindings at once.
    public void UpdateKeybinds(Dictionary<GameCommand, GamepadKey> gamepadConfig) {
        InputManager.gamepadConfig = gamepadConfig;
        gamePadConfigured = false;
    }

    // Retuns which KeyCode triggers the parameter command.
    private KeyCode GetKey(GameCommand command) {
        if (keyboardConfig.ContainsKey(command)) {
            return keyboardConfig[command];
        }
        return KeyCode.None;
    }

    // Returns which gamepad KeyCode button triggers the parameter command.
    private GamepadKey GetButton(GameCommand command) {
        if (gamepadConfig.ContainsKey(command)) {
            return gamepadConfig[command];
        }
        return new GamepadKey(KeyCode.None);
    }

    // Checks if the passed action is being pressed, continuously.
    public bool IsActionPressed(GameCommand action) {
        var pressed = Input.GetKey(GetKey(action));
        pressed |= IsGamePadPress(GetButton(action));

        return pressed;
    }

    // Checks if the passed action is pressed once (won't re-trigger).
    public bool IsActionPressedOnce(GameCommand action) {
        var pressed = Input.GetKeyDown(GetKey(action));
        pressed |= IsGamePadPressOnce(GetButton(action));

        return pressed;
    }

    // Checks if a gamepad axis is being pressed for the specified action.
    private bool IsGamePadPress(GamepadKey action) {
        var pressed = false;
        if (action.key != KeyCode.None) {
            pressed |= Input.GetKey(action.key);
        } else {
            pressed |= action.positive
                ? Input.GetAxisRaw(action.axis) > gamePadTreshold
                : Input.GetAxisRaw(action.axis) < -gamePadTreshold;
        }

        return pressed;
    }

    private bool IsGamePadPressOnce(GamepadKey action) {
        var pressed = false;
        if (action.key != KeyCode.None) {
            pressed |= Input.GetKeyDown(action.key);
        } else {
            pressed |= Time.realtimeSinceStartup - lastMenuKeyTick > gamepadTickDelay 
                && (action.positive
                ? Input.GetAxisRaw(action.axis) > gamePadTreshold
                : Input.GetAxisRaw(action.axis) < -gamePadTreshold);
        }

        return pressed;
    }

    public void MenuKeyTick() {
        lastMenuKeyTick = Time.realtimeSinceStartup;
    }

    public void SetIgnoreMenuTick(bool dotick) {
        ignoreMenuTick = dotick;
    }

    // Checks if anything is being pressed.
    public bool AnyActionPressed() {
        return AnyActionPressed(new List<GameCommand>());
    }

    // Checks if any game action is being pressed unless it's one of the specified command exceptions.
    public bool AnyActionPressed(List<GameCommand> exceptions) {
        var pressed = false;
        foreach (GameCommand command in Enum.GetValues(typeof(GameCommand))) {
            if (exceptions.Contains(command)) {
                continue;
            }
            pressed |= IsActionPressed(command);
        }

        return pressed;
    }
    
    public bool IsOnlyActionPressed(GameCommand action) {
        if (!IsActionPressed(action)) {
            return false;
        }
        var pressed = true;
        foreach (GameCommand command in Enum.GetValues(typeof(GameCommand))) {
            if (command == action) {
                continue;
            }
            if (IsActionPressed(command)) {
                pressed = false;
                break;
            }
        }

        return pressed;
    }

    // In menus, arrow keys must keep working.
    // This is both intuitive and allows the player to fix fucked up key configurations by mistake.
    public bool IsMenuPressUp() {
        if (!ignoreMenuTick && Time.realtimeSinceStartup - lastMenuKeyTick >= gamepadTickDelay) {
            if (Input.GetKeyDown(KeyCode.UpArrow)
                || IsActionPressedOnce(GameCommand.UP)
                || Input.GetAxisRaw(GamepadAxis.VERTICAL) > gamePadTreshold
                || Input.GetAxisRaw(GamepadAxis.VERTICAL2) > gamePadTreshold
                || Input.GetAxisRaw(GamepadAxis.VERTICAL3) > gamePadTreshold
            ) {
                MenuKeyTick();
                return true;
            }
        }
        return false;
    }

    public bool IsMenuPressDown() {
        if (!ignoreMenuTick && Time.realtimeSinceStartup - lastMenuKeyTick >= gamepadTickDelay) {
            if (Input.GetKeyDown(KeyCode.DownArrow) 
                || IsActionPressedOnce(GameCommand.DOWN)
                || Input.GetAxisRaw(GamepadAxis.VERTICAL) < -gamePadTreshold
                || Input.GetAxisRaw(GamepadAxis.VERTICAL2) < -gamePadTreshold
                || Input.GetAxisRaw(GamepadAxis.VERTICAL3) < -gamePadTreshold
            ) {
                MenuKeyTick();
                return true;
            }
        }
        return false;
    }

    public bool IsMenuPressLeft() {
        if (!ignoreMenuTick && Time.realtimeSinceStartup - lastMenuKeyTick >= gamepadTickDelay) {
            if (Input.GetKeyDown(KeyCode.LeftArrow) 
                || IsActionPressedOnce(GameCommand.LEFT)
                || Input.GetAxisRaw(GamepadAxis.HORIZONTAL) < -gamePadTreshold
                || Input.GetAxisRaw(GamepadAxis.HORIZONTAL2) < -gamePadTreshold
                || Input.GetAxisRaw(GamepadAxis.HORIZONTAL3) < -gamePadTreshold
            ) {
                MenuKeyTick();

                return true;
            }
        }
        return false;
    }

    public bool IsMenuPressRight() {
        if (!ignoreMenuTick && Time.realtimeSinceStartup - lastMenuKeyTick >= gamepadTickDelay) {
            if (Input.GetKeyDown(KeyCode.RightArrow)
                || IsActionPressedOnce(GameCommand.RIGHT) 
                || Input.GetAxisRaw(GamepadAxis.HORIZONTAL) > gamePadTreshold
                || Input.GetAxisRaw(GamepadAxis.HORIZONTAL2) > gamePadTreshold
                || Input.GetAxisRaw(GamepadAxis.HORIZONTAL3) > gamePadTreshold
            ) {
                MenuKeyTick();
                return true;
            }
        }
        return false;
    }

    public bool IsMenuStart() {
        return Input.GetKeyDown(KeyCode.Return) || IsActionPressedOnce(GameCommand.ACCEPT) || IsActionPressedOnce(GameCommand.JUMP);
    }

    // Basically used for select level screen.
    public bool AnyActionPressedOnce(List<GameCommand> exceptions) {
        foreach (GameCommand command in Enum.GetValues(typeof(GameCommand))) {
            if (exceptions.Contains(command)) {
                continue;
            }
            if (IsActionPressedOnce(command)) {
                return true;
            }
        }

        return false;
    }

    // Configure.
    public void AutoloadGamepadConfiguration() {
        // Only autoload if there is no configuration set prior.
        if (gamePadConfigured) {
            return;
        }

        // We automatically configure the last connected gamepad.
        string[] gamepads = Input.GetJoystickNames();
        if (gamepads.Length > 0) {
            Dictionary<GameCommand, GamepadKey> binds;
            var name = gamepads[gamepads.Length - 1].ToLower();
            if (name.IndexOf("xbox") > -1) {
                var os = SystemInfo.operatingSystem;
                if (os.IndexOf("mac") > -1) {
                    binds = GetXboxPadMacConfig();
                } else if (os.IndexOf("nux") > -1) {
                    binds = GetXboxPadLinuxConfig();
                } else {
                    binds = GetXboxPadWindowsConfig();
                }
            } else {
                binds = GetGenericPadConfig();
            }
            UpdateKeybinds(binds);
        }
    }

    private Dictionary<GameCommand, GamepadKey> GetGenericPadConfig() {
        return new Dictionary<GameCommand, GamepadKey>() {
            { GameCommand.JUMP, new GamepadKey(KeyCode.JoystickButton1) },
            { GameCommand.SHOOT, new GamepadKey(KeyCode.JoystickButton2) },
            { GameCommand.UP, new GamepadKey(GamepadAxis.VERTICAL3, true) },
            { GameCommand.DOWN, new GamepadKey(GamepadAxis.VERTICAL3, false) },
            { GameCommand.LEFT, new GamepadKey(GamepadAxis.HORIZONTAL3, false) },
            { GameCommand.RIGHT, new GamepadKey(GamepadAxis.HORIZONTAL3, true) },
            { GameCommand.ACCEPT, new GamepadKey(KeyCode.JoystickButton9) }
        };
    }

    private Dictionary<GameCommand, GamepadKey> GetXboxPadWindowsConfig() {
        return new Dictionary<GameCommand, GamepadKey>() {
            { GameCommand.JUMP, new GamepadKey(KeyCode.JoystickButton0) },
            { GameCommand.SHOOT, new GamepadKey(KeyCode.JoystickButton1) },
            { GameCommand.UP, new GamepadKey(GamepadAxis.VERTICAL, true) },
            { GameCommand.DOWN, new GamepadKey(GamepadAxis.VERTICAL, false) },
            { GameCommand.LEFT, new GamepadKey(GamepadAxis.HORIZONTAL, false) },
            { GameCommand.RIGHT, new GamepadKey(GamepadAxis.HORIZONTAL, true) },
            { GameCommand.ACCEPT, new GamepadKey(KeyCode.JoystickButton7) }
        };
    }

    private Dictionary<GameCommand, GamepadKey> GetXboxPadLinuxConfig() {
        return new Dictionary<GameCommand, GamepadKey>() {
            { GameCommand.JUMP, new GamepadKey(KeyCode.JoystickButton0) },
            { GameCommand.SHOOT, new GamepadKey(KeyCode.JoystickButton1) },
            { GameCommand.UP, new GamepadKey(GamepadAxis.VERTICAL, true) },
            { GameCommand.DOWN, new GamepadKey(GamepadAxis.VERTICAL, false) },
            { GameCommand.LEFT, new GamepadKey(GamepadAxis.HORIZONTAL, false) },
            { GameCommand.RIGHT, new GamepadKey(GamepadAxis.HORIZONTAL, true) },
            { GameCommand.ACCEPT, new GamepadKey(KeyCode.JoystickButton7) }
        };
    }

    private Dictionary<GameCommand, GamepadKey> GetXboxPadMacConfig() {
        return new Dictionary<GameCommand, GamepadKey>() {
            { GameCommand.JUMP, new GamepadKey(KeyCode.JoystickButton16) },
            { GameCommand.SHOOT, new GamepadKey(KeyCode.JoystickButton17) },
            { GameCommand.UP, new GamepadKey(GamepadAxis.VERTICAL, true) },
            { GameCommand.DOWN, new GamepadKey(GamepadAxis.VERTICAL, false) },
            { GameCommand.LEFT, new GamepadKey(GamepadAxis.HORIZONTAL, false) },
            { GameCommand.RIGHT, new GamepadKey(GamepadAxis.HORIZONTAL, true) },
            { GameCommand.ACCEPT, new GamepadKey(KeyCode.JoystickButton9) }
        };
    }
}

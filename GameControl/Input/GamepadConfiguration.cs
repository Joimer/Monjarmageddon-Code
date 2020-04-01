using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class GamepadConfiguration {

    public Dictionary<GameCommand, KeyCode> commandToKey = new Dictionary<GameCommand, KeyCode>() {
        // Button A
        // Accept, Jump
        { GameCommand.JUMP, KeyCode.JoystickButton0 },
        // Button B
        // Cancel/Back, Shoot
        { GameCommand.SHOOT, KeyCode.JoystickButton1 },
        // Button Up
        // Up in menus, Look up
        { GameCommand.UP, KeyCode.UpArrow },
        // Button Down
        // Down in menus, Crouch
        { GameCommand.DOWN, KeyCode.DownArrow },
        // Button Left
        // Left in menus, Move left
        { GameCommand.LEFT, KeyCode.LeftArrow },
        // Button Right
        // Right in menus, Move right
        { GameCommand.RIGHT, KeyCode.RightArrow },
        // Button Start
        // Pause game, pass intros, choose menu option
        { GameCommand.ACCEPT, KeyCode.JoystickButton5 }
    };

    // Dead zone in the vertical axis that may change the minimal value for "no press".
    public float deadzoneVertical;

    // Dead zone in the horizontal axis that may change the minimal value for "no press".
    public float deadzoneHorizontal;

    public GamepadConfiguration() {
        SetDefault();
    }

    public void SetDefault() {
        commandToKey[GameCommand.JUMP] = KeyCode.JoystickButton0;
        commandToKey[GameCommand.SHOOT] = KeyCode.JoystickButton1;
        commandToKey[GameCommand.ACCEPT] = KeyCode.JoystickButton5;
        deadzoneVertical = 0f;
        deadzoneHorizontal = 0f;
    }

    public KeyCode GetKey(GameCommand command) {
        if (commandToKey.ContainsKey(command)) {
            return commandToKey[command];
        }
        return KeyCode.None;
    }
}

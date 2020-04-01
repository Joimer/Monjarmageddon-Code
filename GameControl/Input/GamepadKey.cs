using UnityEngine;
using System;

// Class that represents a Gamepad key press, be it a button press or an axis change.
[Serializable]
public class GamepadKey {
    public KeyCode key = KeyCode.None;
    public string axis = GamepadAxis.NONE;
    public bool positive = true;

    public override string ToString() {
        return key == KeyCode.None ? axis + " " + (positive? "+" : "-") : key.ToString();
    }

    public GamepadKey(KeyCode key) {
        this.key = key;
    }

    public GamepadKey(string axis, bool positive) {
        this.axis = axis;
        this.positive = positive;
    }
}

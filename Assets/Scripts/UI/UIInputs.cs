using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInputs : MonoBehaviour
{
    [Header("UI Schemes")]
    public GameObject Keyboard;
    public GameObject Gamepad;

    [Header("Keyboard Assets")]
    public Image Escape;
    public Image Up;
    public Image Down;
    public Image Left;
    public Image Right;
    public Image Sprint;
    public Image Jump;
    public Image LeftMouse;

    [Header("Gamepad Assets")]
    public Image Options;
    public Image LeftAnalog;
    public Image RightAnalog;
    public Image L2;
    public Image Cross;
    public Image R2;

    public Color ButtonUnpressed;
    public Color ButtonPressed;

    public InputManager InputManager;

    private Vector2 _leftAnalogDefaultPosition;
    private Vector2 _rightAnalogDefaultPosition;

    private void Awake()
    {
        _leftAnalogDefaultPosition = LeftAnalog.transform.localPosition;
        _rightAnalogDefaultPosition = RightAnalog.transform.localPosition;
    }

    private void Update()
    {
        Keyboard.SetActive(!InputManager.IsUsingGamePad);
        Gamepad.SetActive(InputManager.IsUsingGamePad);

        ManageKeyboardScheme(!InputManager.IsUsingGamePad);
        ManageGamepadScheme(InputManager.IsUsingGamePad);
    }

    private void ManageKeyboardScheme(bool isUsingKeybard)
    {
        if (!isUsingKeybard)
            return;

        ManageKeyboardButtonState(InputManager.OnPause(), Escape);
        ManageKeyboardButtonState(InputManager.OnMoveVertical() > 0f, Up);
        ManageKeyboardButtonState(InputManager.OnMoveVertical() < 0f, Down);
        ManageKeyboardButtonState(InputManager.OnMoveHorizontal() < 0f, Left);
        ManageKeyboardButtonState(InputManager.OnMoveHorizontal() > 0f, Right);
        ManageKeyboardButtonState(InputManager.OnSprint(), Sprint);
        ManageKeyboardButtonState(InputManager.OnJump(), Jump);
        ManageKeyboardButtonState(InputManager.OnFire(), LeftMouse);
    }

    private void ManageGamepadScheme(bool isUsingGamepad)
    {
        if (!isUsingGamepad)
            return;

        ManagePadButtonState(InputManager.OnPause(), Options);
        ManagePadButtonState(InputManager.OnSprint(), L2);
        ManagePadButtonState(InputManager.OnJump(), Cross);
        ManagePadButtonState(InputManager.OnFire(), R2);

        ManageLeftAnalog(new Vector2(InputManager.OnMoveHorizontal(), InputManager.OnMoveVertical()));
        ManageRightAnalog(new Vector2(InputManager.OnLookX(), InputManager.OnLookY()));
    }

    private void ManageKeyboardButtonState(bool InputPressed, Image button)
    {
        if (InputPressed)
            button.color = ButtonPressed;
        else if (!InputPressed)
            button.color = ButtonUnpressed;
    }

    private void ManagePadButtonState(bool InputPressed, Image button)
    {
        button.enabled = InputPressed;
    }

    private void ManageLeftAnalog(Vector2 position)
    {
        LeftAnalog.transform.localPosition = _leftAnalogDefaultPosition + position * 2.2f;
    }

    private void ManageRightAnalog(Vector2 position)
    {
        RightAnalog.transform.localPosition = _rightAnalogDefaultPosition + position * 5f;
    }
}

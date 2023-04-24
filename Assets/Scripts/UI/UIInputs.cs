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
    public Image DpadUp;
    public Image DpadDown;

    public Color ButtonUnpressed;
    public Color ButtonPressed;

    private InputManager _inputManager;

    private Vector2 _leftAnalogDefaultPosition;
    private Vector2 _rightAnalogDefaultPosition;

    private void Awake()
    {
        _leftAnalogDefaultPosition = LeftAnalog.transform.localPosition;
        _rightAnalogDefaultPosition = RightAnalog.transform.localPosition;
        _inputManager = GetComponentInParent<GameManager>().InputManager;
    }

    private void Update()
    {
        Keyboard.SetActive(!_inputManager.IsUsingGamePad);
        Gamepad.SetActive(_inputManager.IsUsingGamePad);

        ManageKeyboardScheme(!_inputManager.IsUsingGamePad);
        ManageGamepadScheme(_inputManager.IsUsingGamePad);
    }

    private void ManageKeyboardScheme(bool isUsingKeybard)
    {
        if (!isUsingKeybard)
            return;

        ManageKeyboardButtonState(_inputManager.OnPause(), Escape);
        ManageKeyboardButtonState(_inputManager.OnMoveVertical() > 0f, Up);
        ManageKeyboardButtonState(_inputManager.OnMoveVertical() < 0f, Down);
        ManageKeyboardButtonState(_inputManager.OnMoveHorizontal() < 0f, Left);
        ManageKeyboardButtonState(_inputManager.OnMoveHorizontal() > 0f, Right);
        ManageKeyboardButtonState(_inputManager.OnSprint(), Sprint);
        ManageKeyboardButtonState(_inputManager.OnJump(), Jump);
        ManageKeyboardButtonState(_inputManager.OnFire(), LeftMouse);
    }

    private void ManageGamepadScheme(bool isUsingGamepad)
    {
        if (!isUsingGamepad)
            return;

        ManagePadButtonState(_inputManager.OnPause(), Options);
        ManagePadButtonState(_inputManager.OnSprint(), L2);
        ManagePadButtonState(_inputManager.OnJump(), Cross);
        ManagePadButtonState(_inputManager.OnFire(), R2);
        ManagePadButtonState(_inputManager.OnMouseScrolling() < 0f, DpadDown);
        ManagePadButtonState(_inputManager.OnMouseScrolling() > 0f, DpadUp);

        ManageLeftAnalog(new Vector2(_inputManager.OnMoveHorizontal(), _inputManager.OnMoveVertical()));
        ManageRightAnalog(new Vector2(_inputManager.OnLookX(), _inputManager.OnLookY()));
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
        LeftAnalog.transform.localPosition = _leftAnalogDefaultPosition + position * 2.5f;
    }

    private void ManageRightAnalog(Vector2 position)
    {
        RightAnalog.transform.localPosition = _rightAnalogDefaultPosition + position * 6f;
    }
}

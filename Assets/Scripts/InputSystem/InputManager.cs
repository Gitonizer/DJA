using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;

public class InputManager : MonoBehaviour
{
    public bool IsUsingNewInputSystem = false;
    public bool IsGamePaused { get; set; }

    public NewInput InputActions;

    public bool IsUsingGamePad;

    private bool _isSprinting;
    private bool _isFiring;
    private bool _isJumping;

    private void OnEnable()
    {
        InputActions = new NewInput();
        InputActions.Enable();

        InputActions.Gameplay.LookX.started += (context) => DetectDeviceEvent(context);
        InputActions.Gameplay.LookY.started += (context) => DetectDeviceEvent(context);
        InputActions.Gameplay.Scroll.started += (context) => DetectDeviceEvent(context);
        InputActions.Gameplay.Movement.started += (context) => DetectDeviceEvent(context);
        InputActions.UI.Pause.started += (context) => DetectDeviceEvent(context);

        SubscribeEvent(InputActions.Gameplay.Jump, (value) => _isJumping = value);
        SubscribeEvent(InputActions.Gameplay.Fire, (value) => _isFiring = value);
        SubscribeEvent(InputActions.Gameplay.Sprint, (value) => _isSprinting = value);
    }

    private void OnDisable()
    {
        InputActions.Disable();
    }

    public float OnLookX() => IsUsingNewInputSystem ? InputActions.Gameplay.LookX.ReadValue<float>() : Input.GetAxis("Mouse X");

    public float OnLookY() => IsUsingNewInputSystem ? InputActions.Gameplay.LookY.ReadValue<float>() : Input.GetAxis("Mouse Y");

    public float OnMoveHorizontal() => IsUsingNewInputSystem ? InputActions.Gameplay.Movement.ReadValue<Vector2>().x : Input.GetAxis("Horizontal");

    public float OnMoveVertical() => IsUsingNewInputSystem ? InputActions.Gameplay.Movement.ReadValue<Vector2>().y : Input.GetAxis("Vertical");

    public bool OnJump() => IsUsingNewInputSystem ? _isJumping : Input.GetButton("Jump");

    public bool OnFire() => IsUsingNewInputSystem ? _isFiring : Input.GetButton("Fire1");

    public bool OnSprint() => IsUsingNewInputSystem ? _isSprinting : Input.GetButton("Sprint");

    public float OnMouseScrolling() => IsUsingNewInputSystem ? InputActions.Gameplay.Scroll.ReadValue<float>() : Input.GetAxis("Mouse ScrollWheel");

    // Device Checks
    public void DetectDeviceEvent(InputAction.CallbackContext context) => IsUsingGamePad = IsGamepad(context);

    public bool OnPause()
    {
        if (IsGamePaused) InputActions.Gameplay.Disable();
        else InputActions.Gameplay.Enable();

        return IsUsingNewInputSystem ? InputActions.UI.Pause.WasPressedThisFrame() : Input.GetButtonDown("Pause");
    }

    private void SubscribeEvent(InputAction gameplayAction, Action<bool> isAction)
    {
        gameplayAction.started += (ctx) =>
        {
            isAction(true);
            IsUsingGamePad = IsGamepad(ctx);
        };
        gameplayAction.canceled += (ctx) => isAction(false);
    }

    private bool IsGamepad(InputAction.CallbackContext context)
    {
        return context.control.device.name.Contains("Gamepad");
    }
}
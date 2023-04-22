using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private bool _isNewInputSystem = false;

    public float LookX() => Input.GetAxis("Mouse X");

    public float LookY() => Input.GetAxis("Mouse Y");

    public float MoveHorizontal() => Input.GetAxis("Horizontal");

    public float MoveVertical() => Input.GetAxis("Vertical");

    public bool Jump() => Input.GetButton("Jump");

    public bool Fire() => Input.GetButton("Fire1");

    public bool IsSprinting() => Input.GetKey(KeyCode.LeftShift);

    public float IsScrolling() => Input.GetAxis("Mouse ScrollWheel");

    public bool IsPausing() => Input.GetKeyDown(KeyCode.Escape);

    public bool IsNewInputSystem
    {
        get { return _isNewInputSystem; }
    }
}
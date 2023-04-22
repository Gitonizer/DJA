using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inputs : MonoBehaviour
{
    [Header("UI Assets")]
    public Image Escape;
    public Image Up;
    public Image Down;
    public Image Left;
    public Image Right;
    public Image Sprint;
    public Image Jump;
    public Image LeftMouse;

    public Color ButtonUnpressed;
    public Color ButtonPressed;

    private void Update()
    {
        ManageButtonState(KeyCode.Escape, Escape);
        ManageButtonState(KeyCode.W, Up);
        ManageButtonState(KeyCode.S, Down);
        ManageButtonState(KeyCode.A, Left);
        ManageButtonState(KeyCode.D, Right);
        ManageButtonState(KeyCode.LeftShift, Sprint);
        ManageButtonState(KeyCode.Space, Jump);
        ManageMouseState(LeftMouse);
    }

    public void ManageButtonState(KeyCode keyCode, Image button)
    {
        if (Input.GetKeyDown(keyCode))
            button.color = ButtonPressed;
        else if (Input.GetKeyUp(keyCode))
            button.color = ButtonUnpressed;
    }
    public void ManageMouseState(Image button)
    {
        if (Input.GetMouseButtonDown(0))
            button.color = ButtonPressed;
        else if (Input.GetMouseButtonUp(0))
            button.color = ButtonUnpressed;
    }
}

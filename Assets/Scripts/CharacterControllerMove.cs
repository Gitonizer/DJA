using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerMove : MonoBehaviour
{
    private CharacterController _characterController;

    public float Speed;

    private const float GRAVITY = 9.8f;

    private Vector3 _turn;
    public float Sensitivity;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _turn = new Vector3();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        Look();
        Move();
    }

    private void Move()
    {
        float axisForward = Input.GetAxis("Vertical");
        float axisStrafe = Input.GetAxis("Horizontal");

        Vector3 applyGravity = Vector3.up * -GRAVITY;

        Vector3 movement = ((transform.forward * axisForward + transform.right * axisStrafe).normalized + applyGravity) * Speed * Time.deltaTime;

        _characterController.Move(movement);
    }

    private void Look()
    {
        _turn.x += Input.GetAxis("Mouse X") * Sensitivity;
        _turn.y += Input.GetAxis("Mouse Y") * Sensitivity;

        transform.localRotation = Quaternion.Euler(-_turn.y, _turn.x, 0);
    }
}

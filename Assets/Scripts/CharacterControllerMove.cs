using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerMove : MonoBehaviour
{
    private Rigidbody _rigidbody;

    public float Speed;

    private const float GRAVITY = 9.8f;

    private Vector3 _turn;
    public float Sensitivity;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _turn = new Vector3();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        Look();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Look()
    {
        _turn.x += Input.GetAxis("Mouse X") * Sensitivity;
        _turn.y += Input.GetAxis("Mouse Y") * Sensitivity;

        transform.localRotation = Quaternion.Euler(-_turn.y, _turn.x, 0);
    }

    private void Move()
    {
        float axisForward = Input.GetAxis("Vertical");
        float axisStrafe = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump"))
        {
            print("Jumping");
            _rigidbody.AddForce(Vector3.up * 10f, ForceMode.Impulse);
        }

        Vector3 movement = _rigidbody.position + Speed * Time.fixedDeltaTime * ((transform.forward * axisForward + transform.right * axisStrafe).normalized);

        _rigidbody.MovePosition(movement);
    }
}

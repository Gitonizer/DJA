using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float Speed;

    private Animator _animator;
    private float _axisForward;
    private float _axisStrafe;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Move();

        _animator.SetFloat("Speed", _axisForward != 0 ? 6 : 0);
    }

    private void Move()
    {
        _axisForward = Input.GetAxis("Vertical");
        _axisStrafe = Input.GetAxis("Horizontal");

        transform.position += (transform.forward * _axisForward + transform.right * _axisStrafe).normalized * Speed * Time.deltaTime;
    }
}

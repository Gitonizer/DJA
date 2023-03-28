using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    public Transform Player;
    public bool isFirstPerson;

    Camera _camera;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }
    private void Update()
    {
        if (isFirstPerson)
            return;

        _camera.transform.LookAt(Player);
    }
}

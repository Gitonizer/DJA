using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightCenas : MonoBehaviour
{
    private Transform _trans;

    private void Awake()
    {
        _trans = GetComponent<Transform>();
    }

    private void Update()
    {
        _trans.Rotate(new Vector3(0, 0.1f, 0));
    }
}

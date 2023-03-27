using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateCubes : MonoBehaviour
{
    public GameObject Cube;

    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Start()
    {

        
    }

    private void Update()
    {
        if (_camera.transform.childCount > 5)
        {
            foreach (Transform cube in _camera.transform)
            {
                Destroy(cube.gameObject);
            }
        }

        GameObject createdCube = Instantiate(Cube, _camera.transform, false);

        createdCube.transform.localPosition =
            new Vector3(Random.Range(-2, 2), Random.Range(-2, 2), Random.Range(0, 10));
    }
}

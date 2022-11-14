using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysLootAtCamera : MonoBehaviour
{
    [SerializeField]
    Camera _camera;

    private void Update()
    {

        if (_camera == null)
            _camera = Camera.main;
        transform.forward = _camera.transform.forward;        

    }
}

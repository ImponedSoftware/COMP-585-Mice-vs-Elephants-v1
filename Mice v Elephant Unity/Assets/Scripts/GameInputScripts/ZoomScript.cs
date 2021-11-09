using System;
using System.Collections.Generic;
using UnityEngine;
using System.Windows.Input;

public class ZoomScript : MonoBehaviour
{

    private Camera camera;

    private float orthographicSizeMin = 2f;

    private float orthographicSizeMax = 20f;

    private float zoomSpeed = 2f;

    private void Start()
    {
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    private void Update()
    {
        if (camera.orthographic)
        {
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                camera.orthographicSize += zoomSpeed;
            }
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                camera.orthographicSize -= zoomSpeed;
            }
            camera.orthographicSize = Mathf.Clamp(camera.orthographicSize, orthographicSizeMin, orthographicSizeMax);
        }
    }

}
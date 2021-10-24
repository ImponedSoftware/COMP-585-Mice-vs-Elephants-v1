using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAroundGridScript : MonoBehaviour
{
    private Camera camera;
    private float moveSpeed = 0.01f;

    private void Start()
    {
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    // Currently, the screen will move as fast as your computer is able to compute
    // If a slow computer runs this script, moving around the screen will be slower than a faster computer
    private void Update()
    {
        if (camera.orthographic)
        {
            if (Input.GetKey(KeyCode.W))
            {
                float y = camera.transform.position.y + moveSpeed;
                camera.transform.position = new Vector3(camera.transform.position.x, y, camera.transform.position.z);
            }
            if (Input.GetKey(KeyCode.A))
            {
                float x = camera.transform.position.x - moveSpeed;
                camera.transform.position = new Vector3(x, camera.transform.position.y, camera.transform.position.z);
            }
            if (Input.GetKey(KeyCode.S))
            {
                float y = camera.transform.position.y - moveSpeed;
                camera.transform.position = new Vector3(camera.transform.position.x, y, camera.transform.position.z);
            } 
            if (Input.GetKey(KeyCode.D))
            {
                float x = camera.transform.position.x + moveSpeed;
                camera.transform.position = new Vector3(x, camera.transform.position.y, camera.transform.position.z);
            }
            
        }
    }
}

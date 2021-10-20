using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Vector3 _cameraFollowPosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Setup(Vector3 cameraFollowPosition) { 
        _cameraFollowPosition = cameraFollowPosition;
    }

    // Update is called once per frame
    void Update()
    {
        _cameraFollowPosition.z = transform.position.z;
        transform.position = _cameraFollowPosition;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public CameraFollow cameraFollow;

    // Start is called before the first frame update
    void Start()
    {
        cameraFollow.Setup(new Vector3(0, -1000));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

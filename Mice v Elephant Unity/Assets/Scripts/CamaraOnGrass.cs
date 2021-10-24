using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class CamaraOnGrass : MonoBehaviour
{
    // Start is called before the first frame update

    public Camera camera;
    void Start()
    {
        var x = 20;
        var y = 20;
        //GameObject obj = Instantiate(test);
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();

        camera.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width - 20, Screen.height - 20, 10));
        
        for(int i = 0; i < 10; i++)
        {

            camera.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width - x, Screen.height - y, 10));
            x += 10;
            y += 10;

            Thread.Sleep(2000);

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

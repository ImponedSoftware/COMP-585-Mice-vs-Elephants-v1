using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraOnGrass : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject test;
    void Start()
    {
        GameObject obj = Instantiate(test);

        obj.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width - 20, Screen.height - 20, 10));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

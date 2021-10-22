using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private int row;

    private int coloum;

    private float tileSpace = 1.1f;

    
    // Create a new script call Zoom or something for modualtions.
    private Camera camera;

    private float orthographicSizeMin = 2f;

    private float orthographicSizeMax = 20f;

    private float zoomSpeed = 1f;

    void Start()
    {
        row = InputInGameData.row;
        coloum = InputInGameData.coloum;
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();

        Debug.Log(camera.orthographic);

        initGrid();

        camera.transform.position = new Vector3(-2, -1, camera.transform.position.z);
        camera.orthographicSize = 9;
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

    private void initGrid()
    {

        GameObject refTile = (GameObject)Instantiate(Resources.Load("Grass"));

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < coloum; j++)
            { 

                GameObject tile = (GameObject)Instantiate(refTile, transform);

                float posX = j * tileSpace;
                float posY = i * -tileSpace;

                tile.transform.position = new Vector2(posX, posY);
              
            }
        }
        Destroy(refTile);

        float gridW = coloum * tileSpace;
        float gridH = row * tileSpace;

        // Spaceing from each grid tile.
        transform.position = new Vector2(-gridW / 2 + tileSpace / 2, gridH / 2 - tileSpace / 2);

        Debug.Log("Finsih");

        Debug.Log(camera.transform.position);
    }
}

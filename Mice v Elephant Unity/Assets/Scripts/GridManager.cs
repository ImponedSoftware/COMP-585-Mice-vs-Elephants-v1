using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GridManager : MonoBehaviour
{
    private int _row;

    private int _coloum;

    private float tileSpace = 1.1f;
  
    // Create a new script call Zoom or something for modualtions.
    private Camera camera;
    
    void Start()
    {
        _row = InputInGameData.row;
        _coloum = InputInGameData.coloum;
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();

        // Where camera is within scene
        camera.transform.position = new Vector3(_row/2, -_coloum/2, camera.transform.position.z);

        // How far out camera will zoom
        camera.orthographicSize = 10;

        initGrid(_row, _coloum);
    }

    private void Update()
    {
        
    }

    private void initGrid(int row, int col)
    {

        GameObject refTile = (GameObject)Instantiate(Resources.Load("Grass"));

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            { 
                GameObject tile = (GameObject)Instantiate(refTile, transform);

                float posX = j * tileSpace;
                float posY = i * -tileSpace;
                tile.transform.position = new Vector3(posX, posY, 1);

                Debug.Log(tile.transform.position);
            }
        }
        
        Destroy(refTile);

        float gridW = col * tileSpace;
        float gridH = row * tileSpace;

        // Spacing from each grid tile. Renders empty tile/space.
       // transform.position = new Vector2(-gridW / 2 + tileSpace / 2, gridH / 2 - tileSpace / 2);

        Debug.Log("Finsih");
        Debug.Log(camera.transform.position);

    }

    public void updateGridSize() 
    {
        initGrid(4, 4);
    }

}

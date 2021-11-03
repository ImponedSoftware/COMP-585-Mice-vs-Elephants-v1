using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GridManager : MonoBehaviour
{
    private int _row;

    private int _coloum;
    private int _numOfElephants;
    private int _numOfMice;

    [SerializeField]
    private float tileSpace = 1.1f;

    public InputField XInput, YInput;
    public int tempX, tempY;
    // Create a new script call Zoom or something for modualtions.
    private Camera camera;
    private List<GameObject> elephants = new List<GameObject>();
    private List<GameObject> mice = new List<GameObject>();


    void Start()
    {
        _row = InputInGameData.row;
        _coloum = InputInGameData.coloum;
        _numOfElephants = InputInGameData.numberOfElephants;
        _numOfMice = InputInGameData.numberOfMice;
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();

        // Where camera is within scene
        camera.transform.position = new Vector3(_row/2, -_coloum/2, camera.transform.position.z);

        // How far out camera will zoom
        camera.orthographicSize = 10;

        // put grid
        initGrid(_row, _coloum);

        Debug.Log("After Init Grid");


        // put elephants on screen
        moveElephantsRandomly();
        moveMiceRandomly();

        Debug.Log("After Move Elephant");

    }

    private void Update()
    {
        
    }

    private void moveElephantsRandomly() 
    {
        System.Random random = new System.Random();

        GameObject refTile = (GameObject)Instantiate(Resources.Load("Elephant 1"));

        Debug.Log("Num of Elephant: " + _numOfElephants);
        
        for(int i = 0; i < _numOfElephants; i++)
        {
            
            // GameObject elephantTile = (GameObject)Instantiate(refTile, transform);

            // elephants.Add(elephantTile);

            int randomX = random.Next(0, _row);
            int randomY = random.Next(0, _coloum);
            Debug.Log($"X: {randomX} --- Y: {randomY}");

            GameObject elephantTile = (GameObject)Instantiate(refTile, transform);
            elephants.Add(elephantTile);
            elephants[i].transform.position = new Vector3(randomX * tileSpace, randomY * -tileSpace, 1);

        }
        Destroy(refTile);

    }

    private void moveMiceRandomly() 
    {
        System.Random random = new System.Random();

        GameObject refTile = (GameObject)Instantiate(Resources.Load("mouseart"));

        
        for(int i = 0; i < _numOfMice; i++)
        {
            
            // GameObject elephantTile = (GameObject)Instantiate(refTile, transform);

            // elephants.Add(elephantTile);

            int randomX = random.Next(0, _row);
            int randomY = random.Next(0, _coloum);
            Debug.Log($"X: {randomX} --- Y: {randomY}");

            GameObject miceTile = (GameObject)Instantiate(refTile, transform);
            mice.Add(miceTile);
            mice[i].transform.position = new Vector3(randomX * tileSpace, randomY * -tileSpace, 1);

        }
        Destroy(refTile);

    }

    private void initGrid(int row, int col)
    {

        // test
        GameObject refTile = (GameObject)Instantiate(Resources.Load("Grass"));

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            { 
                GameObject tile = (GameObject)Instantiate(refTile, transform);

                float posX = j * tileSpace;
                float posY = i * -tileSpace;
                tile.transform.position = new Vector3(posX, posY, 1);

                //Debug.Log(tile.transform.position);
            }
        }
        Destroy(refTile);
    }

}

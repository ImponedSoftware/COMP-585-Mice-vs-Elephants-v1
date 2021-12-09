using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class GridManager : MonoBehaviour
{
    private int _row;

    private int _coloum;
    private int _numOfElephants;
    private int _numOfMice;

    [SerializeField]
    public static float tileSpace = 1.1f;

    public InputField XInput, YInput;
    public int tempX, tempY;
    // Create a new script call Zoom or something for modualtions.
    private Camera camera;
    public static List<GameObject> elephants;
    public static List<GameObject> mice;

    public static Vector3[,] gameObjRef;

    public static TextMeshProUGUI simRound;

    void Start()
    {
        _row = InputInGameData.row;
        _coloum = InputInGameData.coloum;
        _numOfElephants = InputInGameData.numberOfElephants;
        _numOfMice = InputInGameData.numberOfMice;
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();

        // Where camera is within scene
        camera.transform.position = new Vector3(_row/2, -_coloum/2, camera.transform.position.z);

        simRound = GameObject.Find("RoundText").GetComponentInChildren<TextMeshProUGUI>();

        // How far out camera will zoom
        camera.orthographicSize = 10;

        // put grid
        initGrid(_row, _coloum);

        Debug.Log("After Init Grid");

        elephants = new List<GameObject>();
        mice = new List<GameObject>();
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

        gameObjRef = new Vector3[row,col];

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

                gameObjRef[j, i] = tile.transform.position;
            }
        }
        Destroy(refTile);
    }

    public static void SyncCurrentPosToScenePos(int pointX, int pointY, int currentElephantIndex)
    {
       // Debug.Log(GridManager.elephants[currentElephantIndex].transform.position);
      // GridManager.elephants[currentElephantIndex].transform.position = new Vector3(pointX * GridManager.tileSpace, pointY * -GridManager.tileSpace, 1);
        
        Vector3 pos = GridManager.elephants[currentElephantIndex].transform.position; // Array out of bounds array
                 if (pointX >= GridManager.gameObjRef.GetLength(0) - 1)
                {
                    pointX = GridManager.gameObjRef.GetLength(0) - 1;
                }

                if (pointY >= GridManager.gameObjRef.GetLength(1) - 1)
                {
                    pointY = GridManager.gameObjRef.GetLength(1) - 1;
                }


        
            Vector3 fe = (gameObjRef[pointX, pointY]);

            GridManager.elephants[currentElephantIndex].transform.position = new Vector3(fe.x, fe.y, fe.z);

              
    }

    public static void SyncCurrentPosToScenePosMice(int pointX, int pointY, int currentElephantIndex)
    {
        /*    Debug.Log(GridManager.mice[currentElephantIndex].transform.position);
            GridManager.mice[currentElephantIndex].transform.position = new Vector3(pointX * GridManager.tileSpace, pointY * -GridManager.tileSpace, 1);*/

        Vector3 pos = mice[currentElephantIndex].transform.position;
        if (pointX >= gameObjRef.GetLength(0) - 1)
        {
            pointX = gameObjRef.GetLength(0) - 1;
        }

        if (pointY >= gameObjRef.GetLength(1) - 1)
        {
            pointY = gameObjRef.GetLength(1) - 1;
        }



        Vector3 fe = (gameObjRef[pointX, pointY]);

        mice[currentElephantIndex].transform.position = new Vector3(fe.x, fe.y, fe.z);
    }



    public static void SetSimulationRoundText(int round)
    {
      
        simRound.text = "Simulation Round: " + round;
    }

}

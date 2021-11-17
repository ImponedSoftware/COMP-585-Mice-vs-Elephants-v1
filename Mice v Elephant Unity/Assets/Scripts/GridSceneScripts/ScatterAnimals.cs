using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScatterAnimals : MonoBehaviour
{
    private int _row;
    private int _coloum;
    private float _tileSpace = GridManager.tileSpace;
    void Start() 
    {
        _row = InputInGameData.row;
        _coloum = InputInGameData.coloum; 
    }
    public void scatterAnimals()
    {
        Debug.Log("LOL");

        System.Random random = new System.Random();

        // Count is the same thing as elephants.size() in Java
        for(int i = 0; i < GridManager.elephants.Count; i++)
        {
            int randomX = random.Next(0, _row);
            int randomY = random.Next(0, _coloum);
            GridManager.elephants[i].transform.position = new Vector3(randomX * _tileSpace, randomY * -_tileSpace, 1);
        }

        for(int i = 0; i < GridManager.mice.Count; i++)
        {
            int randomX = random.Next(0, _row);
            int randomY = random.Next(0, _coloum);
            GridManager.mice[i].transform.position = new Vector3(randomX * _tileSpace, randomY * -_tileSpace, 1);
        }
    }
}

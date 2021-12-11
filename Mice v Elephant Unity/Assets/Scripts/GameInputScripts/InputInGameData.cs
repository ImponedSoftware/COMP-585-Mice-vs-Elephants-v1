using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputInGameData : MonoBehaviour
{
    public Button playButton;
    public InputField ColumnInput, RowInput, NumberOfMiceInput, NumberOfElephantInput, StrikeDistanceInput;

    public static int row, coloum, numberOfElephants, numberOfMice, strikeDistance;
    public bool GridSizeChangeRequested = false;
    // Start is called before the first frame update
    void Start()
    {
        playButton.onClick.AddListener(initGameData);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void initGameData()
    {
        //int numberOfMice, numberOfElephants, strikeDistance;

        Debug.Log($"{RowInput.text} {ColumnInput.text} {NumberOfMiceInput.text} {NumberOfElephantInput.text} {StrikeDistanceInput.text}");


        int.TryParse(RowInput.text, out row);
        int.TryParse(ColumnInput.text, out coloum);
  

        numberOfElephants = int.Parse(NumberOfElephantInput.text);
        numberOfMice = int.Parse(NumberOfMiceInput.text);
        strikeDistance = int.Parse(StrikeDistanceInput.text);

        Debug.Log($"{row} {coloum} {numberOfMice} {numberOfElephants} {strikeDistance}");
    }
    
}

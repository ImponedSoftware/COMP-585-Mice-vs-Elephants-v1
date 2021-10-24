using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
* This is connected to the "Update Grid" button
* Within the game, the two input fields will update based on the input for NewColumnInput and NewRowInput
* These string values are then parsed into NewRow and NewColumn as integer values
* When the button is pressed, GridManager and UpdateGridSize will be called
* The functions that will run are "parseGridStrings()" and "updateGridSize()"
* The new values will be passed into "initGrid()", which will initialize a new grid onto the screen with the new row/col values
*/


public class UpdateGridSize : MonoBehaviour
{
    public Button button;
    private bool GridSizeChangeRequested = false;
    public InputField NewColumnInput, NewRowInput;
    public static int NewRow, NewColumn;

    void Start()
    {
        button.onClick.AddListener(parseGridStrings);
    }
    public void parseGridStrings()
    {
        int.TryParse(NewColumnInput.text, out NewRow);
        int.TryParse(NewRowInput.text, out NewColumn);
    }
}

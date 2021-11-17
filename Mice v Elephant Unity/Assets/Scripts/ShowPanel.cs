using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShowPanel : MonoBehaviour
{
    public GameObject panel;
    /*private GameObject openSidePanelButton = GameObject.Find("OpenPanelButton");*/
    public TextMeshProUGUI openSidePanelButton;

    public void Start()
    {
        openSidePanelButton = GameObject.Find("OpenPanelButton").GetComponentInChildren<TextMeshProUGUI>();
    }

    public void ShowSidePanel()
    {
        if(panel.activeInHierarchy == false)
        {
            panel.SetActive(true);
           openSidePanelButton.text = "Close Panel";
        }
        else
        {
            panel.SetActive(false);
            openSidePanelButton.text = "Open Panel";
        }
    }
}

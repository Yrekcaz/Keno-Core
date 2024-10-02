using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class MenuSwap : MonoBehaviour {

    public GameObject oldMenu; public GameObject newMenu;
    public GameObject Page1; public GameObject Page2;
     public TextMeshProUGUI PageNumberDisplay;

    public void MenuSwitch()
    {
        oldMenu.SetActive(false);
        newMenu.SetActive(true);
    }
    public void nextPreviousPage(GameObject newPage, GameObject oldPage, String PageNumber)
    {
        oldPage.SetActive(false);
        newPage.SetActive(true);
        PageNumberDisplay.text = "Page: " + PageNumber + "/2";
    }
    public void ShowPage1()
    {
        nextPreviousPage(Page1, Page2, "1");
    }
    public void ShowPage2()
    {
        nextPreviousPage(Page2, Page1, "2");
    }

}

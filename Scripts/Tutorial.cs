using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    [Tooltip("Add each page of tutorial here")]
    public GameObject[] pages; 
    [Tooltip("Total pages of tutorial, remember binary; Page 1 = 0")]
    public int totalPages; 
    public Button GameSelectButton;
    public static int currentPage = 0; 
    public static bool isTutorial = false; 
    public static bool tutorialAlreadyStarted; 
    // Start is called before the first frame update
    void Start()
    {
        if(!tutorialAlreadyStarted) {currentPage = 0;}
        foreach (GameObject page in pages)
        {
            if (page != null)
            {
                page.SetActive(false);
            }
            //else { StartCoroutine(GameEND()); }
        }
        pages[currentPage].SetActive(true);
        tutorialAlreadyStarted = true;
        isTutorial = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentPage == 1)
        GameSelectButton.interactable = true;
        else
        GameSelectButton.interactable = false;
    }

    public void NextPage()
    {
        pages[currentPage].SetActive(false);
        pages[currentPage + 1].SetActive(true);
        currentPage++;
        //if(currentPage == totalPages) {tutorialAlreadyStarted = false;}
    }
    public void PreviousPage()
    {
        pages[currentPage].SetActive(false);
        pages[currentPage - 1].SetActive(true);
        currentPage--;
        //if(currentPage <= 0) {tutorialAlreadyStarted = false;}
    }
    public void GameSelectClicked()
    {
        NextPage();
    }
    public void SetTutorialValuesFalse()
    {
        isTutorial = false;
        tutorialAlreadyStarted = false;
    }
}

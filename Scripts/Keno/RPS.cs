using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

// This script holds Rock Paper Scissors (RPS = Rock Paper Scissors)

public class RPS : MonoBehaviour
{
    public BEHAVIOUR Bh;

    [Tooltip("Set this to the game object holding all the Rock Paper Scissors buttons")] public GameObject RPSAssets;
    public GameObject RockButton; public GameObject PaperButton; public GameObject ScissorsButton;

    public IEnumerator RockPaperScissors() //The rock paper scissors game logic
    {
        Bh.interupt();   
        Bh.ProceedTrue = false;
        Bh.Entertained = true;
        //Enable Buttons
        RPSAssets.SetActive(true);
        RockButton.SetActive(true);
        PaperButton.SetActive(true);
        ScissorsButton.SetActive(true);
        yield return new WaitUntil(() => Bh.ProceedTrue);
        Debug.Log("Choice: " + Bh.choice);
        Bh.ProceedTrue = false;
        if(Bh.choice == "Rock")
        {
            PaperButton.SetActive(false);
            ScissorsButton.SetActive(false);
        }
        else if(Bh.choice == "Paper")
        {
            RockButton.SetActive(false);
            ScissorsButton.SetActive(false);
        }
        else if(Bh.choice == "Scissors")
        {
            RockButton.SetActive(false);
            PaperButton.SetActive(false);
        }
        //Keno's turn to choose
        Bh.variant = Random.Range(1, 4);
        Debug.Log("KENO Chose:" + Bh.variant);
        if (Bh.variant == 1){
            Bh.PlayVideo(Bh.Rock);
        }
        else if (Bh.variant == 2){
            Bh.PlayVideo(Bh.Paper);
        }
        else if (Bh.variant == 3){
            Bh.PlayVideo(Bh.Scissors);
        }
        yield return new WaitForSeconds(3f);
        //Determine the winner and react
        if (Bh.variant == 1 && Bh.choice == "Scissors" || Bh.variant == 2 && Bh.choice == "Rock" || Bh.variant == 3 && Bh.choice == "Paper"){ //WON
            Bh.variant = Random.Range(1, 4);
            Debug.Log("Variant: " + Bh.variant);
            Bh.PlayVideo(Bh.variant == 1 ? Bh.Joy : Bh.variant == 2 ? Bh.Joy2 : Bh.CoolGlasses);
            Bh.attitude = 1;
            if (Bh.currentAttitudeReset != null) { StopCoroutine(Bh.currentAttitudeReset);  Debug.Log("Stopped Existing AR"); }
            Bh.currentAttitudeReset = StartCoroutine(Bh.attitudeReset(Random.Range(4.5f, 15) * 60));
        }
        else if (Bh.variant == 2 && Bh.choice == "Scissors" || Bh.variant == 3 && Bh.choice == "Rock" || Bh.variant == 1 && Bh.choice == "Paper"){ //LOST
            Bh.variant = Random.Range(1, 4);
            Debug.Log("Variant: " + Bh.variant);
            Bh.PlayVideo(Bh.variant == 1 ? Bh.Mad : Bh.Sad);
            Debug.Log("Keno Lost");
            Bh.attitude = 2;
            if (Bh.currentAttitudeReset != null) { StopCoroutine(Bh.currentAttitudeReset); Debug.Log("Stopped Existing AR"); }
            Bh.currentAttitudeReset = StartCoroutine(Bh.attitudeReset(Random.Range(4.5f, 15) * 60));
        }
        else if (Bh.variant == 3 && Bh.choice == "Scissors" || Bh.variant == 1 && Bh.choice == "Rock" || Bh.variant == 2 && Bh.choice == "Paper"){ //TIE
            Bh.PlayVideo(Bh.Confused); 
            Debug.Log("Tie");
            Bh.attitude = 0;

        }
        yield return new WaitForSeconds(3f);
        //End rock paper scissors
        RPSAssets.SetActive(false);
        Bh.Entertained = false;
        Bh.ProceedTrue = false;
        Bh.uninterupt();
        Bh.PR.SetActive(true);
        Bh.GameRequests.GameButton.interactable = true;
        StartCoroutine(Bh.GameRequests.BoredTrigger(Bh.GameRequests.waitForBoredom * 60));
    }
}

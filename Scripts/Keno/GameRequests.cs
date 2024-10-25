using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameRequests : MonoBehaviour
{
    public BEHAVIOUR Bh;
    public Coroutine CurrentBoredTrigger;
    [Tooltip("This is the reference to the `RPS` script, which should be on a GameObject")] public RPS RPS;
    
    [Header("Values")]
    [Tooltip("This value defines how long it takes for Keno to be bored")] public float waitForBoredom;
    public TextMeshProUGUI gameName;
    public GameObject gameRequest; public GameObject gameSelectGUI;  public GameObject gameSelectGUI2; public Button GameButton;

    public bool isBored = false;
    // Start is called before the first frame update
    void Start()
    {
        gameRequest.SetActive(false);
        gameSelectGUI.SetActive(false);
        gameSelectGUI2.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if((!Bh.interupted) && Bh.GameReqOn && CurrentBoredTrigger == null)
        {
            CurrentBoredTrigger = StartCoroutine(BoredTrigger(waitForBoredom * 60));
        }
    }

    // Coroutine to handle bored state and the `Keno wants to play` dialogue
    public IEnumerator HandleBoredState()
    {
        Bh.interupt();
        Bh.PR.SetActive(false);
        isBored = false;
        Bh.ProceedTrue = false;
        Bh.variant = Random.Range(1, 3); // variant
        Debug.Log("Bored");
        Debug.Log("Variant: " + Bh.variant);
        Bh.PlayVideo(Bh.variant == 1 ? Bh.Bored1 : Bh.Bored2);
        yield return new WaitForSeconds(3);
        AskGame();
        gameName.text = "KENO would like to play Rock Paper Scissors";
        yield return new WaitUntil(() => Bh.ProceedTrue);
        Debug.Log("Choice: " + Bh.choice);
        gameRequest.SetActive(false);

        if (Bh.choice == "Yes")
        {
            StartCoroutine(RPS.RockPaperScissors());
        }
        else
        {
            Bh.PR.SetActive(true);
            Bh.attitude = 2;
            if (Bh.currentAttitudeReset != null) { StopCoroutine(Bh.currentAttitudeReset);
            Debug.Log("Stopped Existing AR"); }
            Bh.currentAttitudeReset = StartCoroutine(Bh.attitudeReset(Random.Range(2, 11) * 60));
            Bh.uninterupt();
            GameButton.interactable = true;
        }
    }

    public IEnumerator BoredTrigger(float waitForBoredAgain)
    {
        Debug.Log("BoredTrigger Started");
        float elapsedTime = 0f;
        while (elapsedTime < waitForBoredAgain)
        {
            if (Bh.interupted)
            {
                Debug.Log("Bored Trigger Cancled");
                CurrentBoredTrigger = null;
                yield break; // Exit the coroutine
            }
            else if(!Bh.GameReqOn)
            {
                Debug.Log("Game Requests Are Disabled");
                CurrentBoredTrigger = null;
                yield break;
            }
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }
        isBored = true;
        CurrentBoredTrigger = null;
    }
    
    // Events
    public void AskGame()
    {
        Bh.PR.SetActive(false);
        gameRequest.SetActive(true);
        GameButton.interactable = false;
    }
    public void ChooseGame()
    {
        gameSelectGUI.SetActive(true);
        GameButton.interactable = false;
    }
    public void userGameRequest()
    {
        StartCoroutine(UserRequestedGame());
    }

    public IEnumerator UserRequestedGame()
    {
        Bh.PR.SetActive(false);
        Bh.interupt();
        Bh.ProceedTrue = false;
        isBored = false;
        ChooseGame();
        yield return new WaitUntil(() => Bh.ProceedTrue);
        Debug.Log("Choice: " + Bh.choice);
        gameSelectGUI.SetActive(false);
        gameSelectGUI2.SetActive(false);
        if (Bh.choice == "RPS")
        {
            StartCoroutine(RPS.RockPaperScissors());
        }
        else if (Bh.choice == "Modes") { }
        else
        {
            Bh.PR.SetActive(true);
            Bh.uninterupt();
            GameButton.interactable = true;
        }
    }

    public void ModeEnter(string Mode)
    {
        StartCoroutine(Bh.Modes.ModeHandle(Mode));
        Debug.Log("Mode Handle Initiated");
    }

}

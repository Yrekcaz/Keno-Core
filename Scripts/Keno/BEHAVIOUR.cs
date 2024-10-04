using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;

//MADE BY YREKCAZ. GIVE PROPER CREDITS BEFORE USING

//Please feel free to rework/clean up/optimize, this is currently a trainwreck
//Please check out the README

//This is the main script, which is the main backbone of Keno

public class BEHAVIOUR : MonoBehaviour
{
    public Coroutine currentAttitudeReset;
    private Coroutine currentPause;
    [Tooltip("This is the refernce to the `Modes` script, which should be on a GameObject")] public Modes Modes;
    [Tooltip("This is the refernce to the `RPS` script, which should be on a GameObject")] public RPS RPS;

    [Header("Behaviour values")]
    public float waitForBoredom;
    public string choice; 
    public bool longWait = false;
    public bool ProceedTrue = false;
    public bool doingsomething = false;
    public bool isBored = false;
    public bool Entertained = false;
    public bool interupted = false;
    public bool ResetAttitudeTimer = false;
    private bool asleepThisSession = false;
    public bool GameReqOn;
    public int whatToDo = 0;
    public float variant;
    public int attitude = 0; // 0 = normal, 1 = happy/cool, 2 = sad/angry
    [Header("Game related stuff and buttons")]
    public TextMeshProUGUI gameName;
    public GameObject gameRequest; public GameObject gameSelectGUI;  public GameObject gameSelectGUI2; public Button GameButton;
    public GameObject RPSAssets;
    public GameObject RockButton; public GameObject PaperButton; public GameObject ScissorsButton;
    public GameObject StopActivityButton; public GameObject ModeSelectGUI;
    [Tooltip("Set this to the Poke Region button")] public GameObject PR;
    [Header("Animations")]
    public VideoPlayer Rock;
    public VideoPlayer Paper;
    public VideoPlayer Scissors;
    public VideoPlayer CustomExpression;
    public VideoPlayer HappyCustomExpression;
    public VideoPlayer SadCustomExpression;
    public VideoPlayer NeutralCustomExpression;
    public VideoPlayer Blink;
    public VideoPlayer Joy;
    public VideoPlayer Joy2;
    public VideoPlayer Sad;
    public VideoPlayer Mad;
    public VideoPlayer Bored1;
    public VideoPlayer Bored2;
    public VideoPlayer Confused;
    public VideoPlayer CoolGlasses;
    public VideoPlayer LookTopLeft;
    public VideoPlayer LookTopRight;
    public VideoPlayer LookBottomLeft;
    public VideoPlayer LookBottomRight;
    [Tooltip("This is the short variant of the animation")] public VideoPlayer LookRightS;
    [Tooltip("This is the long variant of the animation")] public VideoPlayer LookRightL;
    [Header("Mode Animations")]//Modes
    public VideoPlayer QuestionInvoke;
    public VideoPlayer SleepStart;
    public VideoPlayer SleepEnd;
    public VideoPlayer ReadStart;
    public VideoPlayer ReadLoop;
    public VideoPlayer ReadEnd;
    public VideoPlayer ReadStart2;
    public VideoPlayer ReadLoop2;
    public VideoPlayer ReadEnd2;
    public VideoPlayer MovieStart;
    public VideoPlayer MovieLoop;
    public VideoPlayer MovieEnd;
    [Header("Interaction Animations")]
    public VideoPlayer Poked;
    public VideoPlayer Alarm;

    void Start()
    {
        Tutorial.isTutorial = false; //Ensures that we are not in the tutorial
        StopAllVideos(); //Ensure all VideoPlayers are initially stopped and disabled
        GameReqOn = PlayerPrefs.GetInt("Game Requests", 1) == 1 ? true : false;
        StartCoroutine(BoredTrigger(waitForBoredom * 60));
        gameRequest.SetActive(false);
        gameSelectGUI.SetActive(false);
        gameSelectGUI2.SetActive(false);
        RPSAssets.SetActive(false);
        StopActivityButton.SetActive(false);
        ModeSelectGUI.SetActive(false);
        if (PlayerPrefs.GetInt("Custom Expression Enabled", 0) == 1)
        {
            if(PlayerPrefs.GetInt("Constant", 1) == 1)
            {
                string videoPath = PlayerPrefs.GetString("SelectedVideoPath", "");
            if (!string.IsNullOrEmpty(videoPath)) { CustomExpression.url = videoPath; Debug.Log("Custom Expression Loaded"); }
            else { Debug.LogWarning("No video file selected or path not found."); }
            } else {
                string happyPath = PlayerPrefs.GetString("HappyVideoPath", ""); if (!string.IsNullOrEmpty(happyPath)) { HappyCustomExpression.url = happyPath; Debug.Log("HAPPY Custom Expression Loaded"); }
                else { Debug.LogWarning("No HAPPY video file selected or path not found."); }
                string sadPath = PlayerPrefs.GetString("SadVideoPath", ""); if (!string.IsNullOrEmpty(sadPath)) { SadCustomExpression.url = sadPath; Debug.Log("SAD Custom Expression Loaded"); }
                else { Debug.LogWarning("No SAD video file selected or path not found."); }
                string neutralPath = PlayerPrefs.GetString("HappyVideoPath", ""); if (!string.IsNullOrEmpty(neutralPath)) { NeutralCustomExpression.url = neutralPath; Debug.Log("NEUTRAL Custom Expression Loaded"); }
                else { Debug.LogWarning("No NEUTRAL video file selected or path not found."); }
            }
        }
        doingsomething = true;
        PlayVideo(SleepEnd);
        StartCoroutine(Pause(2.5f));
    }

    void Update()
    {
        System.DateTime currentTime = System.DateTime.Now;
        System.DateTime start = System.DateTime.Today.AddHours(20); // 8 PM today
        System.DateTime end = System.DateTime.Today.AddHours(7); // 7 AM today

        if (!doingsomething & !interupted)
        {
                if (!asleepThisSession && PlayerPrefs.GetInt("Bedtime Enabled", 1) == 1 && ((currentTime >= start) || (currentTime <= end)) && !Tutorial.isTutorial)
                {
                    Debug.Log("Bedtime");
                    ModeEnter("Sleep");
                    attitude = 0;
                    asleepThisSession = true;
                }
                else if(isBored)
                {
                    StartCoroutine(HandleBoredState());
                }
                else { 
                doingsomething = true;
                whatToDo = Random.Range(PlayerPrefs.GetInt("Custom Expression Enabled", 0) == 1 ? 1 : 2, 11); // Go one number higher than the highest number you want
                Debug.Log(whatToDo);
                if (whatToDo == 0) //Custom Expression 2 (CURRENTLY UNUSED; PLACEHOLDER)
                {
                    if (PlayerPrefs.GetInt("Constant", 1) == 1) { PlayVideo(CustomExpression); }
                    else {  
                        if (attitude == 1) { PlayVideo(HappyCustomExpression); }
                        else if (attitude == 2) { PlayVideo(SadCustomExpression); }
                        else if (attitude == 0) { PlayVideo(NeutralCustomExpression); }
                    }
                }
                if (whatToDo == 1) //Custom Expression 1
                {
                    if (PlayerPrefs.GetInt("Constant", 1) == 1) { PlayVideo(CustomExpression); }
                    else {  
                        if (attitude == 1) { PlayVideo(HappyCustomExpression); }
                        else if (attitude == 2) { PlayVideo(SadCustomExpression); }
                        else if (attitude == 0) { PlayVideo(NeutralCustomExpression); }
                    }
                }
                else if (whatToDo == 2 || whatToDo == 3 || whatToDo == 4) // Blink
                {
                    PlayVideo(Blink);
                }
                else if (whatToDo == 5){} // Nothing
                else if (whatToDo == 6) // Look (Corner)
                {
                    variant = Random.Range(1, 5);
                    Debug.Log("Variant: " + variant);
                    PlayVideo(variant == 1 ? LookTopLeft : variant == 2 ? LookTopRight : variant == 3 ? LookBottomLeft : LookBottomRight);
                }
                else if (whatToDo == 7) // Look (Side, Short variant)
                {
                    variant = Random.Range(1, 3);
                    Debug.Log("Variant: " + variant);
                    PlayVideo(variant == 1 ? LookRightS : LookRightS); //Placeholder; planning on adding a left variant
                }
                else if (whatToDo == 8) // attitude dependent 1
                {
                    if (attitude == 1) { PlayVideo(Joy); }
                    else if (attitude == 2) { PlayVideo(Sad); }
                    else if (attitude == 0) { variant = Random.Range(1,3); PlayVideo(variant == 1 ? LookTopLeft : LookTopRight); }
                }
                else if (whatToDo == 9) // attitude dependent 2
                {
                    if (attitude == 1) { PlayVideo(Joy2); }
                    else if (attitude == 2) { PlayVideo(Mad); }
                    else if (attitude == 0) { variant = Random.Range(1,3); PlayVideo(variant == 1 ? LookBottomLeft : LookBottomRight); }
                }
                else if (whatToDo == 10) // attitude dependent 3
                {
                    if (attitude == 1) { PlayVideo(CoolGlasses); }
                    else if (attitude == 2) { PlayVideo(Mad); }
                    else if (attitude == 0) { variant = Random.Range(1,3); PlayVideo(variant == 1 ? LookRightL : LookRightL); longWait = true;} //Placeholder; planning on adding a left variant
                }

                if (currentPause != null) { StopCoroutine(currentPause);  Debug.Log("Stopped Existing Pause"); }
                currentPause = StartCoroutine(Pause(longWait ? 6f : Random.Range(3.7f, 4f)));
            }
        }
    }

    // Coroutine to handle bored state and the `Keno wants to play` dialogue
    IEnumerator HandleBoredState()
    {
        interupt();
        PR.SetActive(false);
        isBored = false;
        ProceedTrue = false;
        Entertained = true;
        variant = Random.Range(1, 3); // variant
        Debug.Log("Bored");
        Debug.Log("Variant: " + variant);
        PlayVideo(variant == 1 ? Bored1 : Bored2);
        yield return new WaitForSeconds(3);
        AskGame();
        gameName.text = "KENO would like to play Rock Paper Scissors";
        yield return new WaitUntil(() => ProceedTrue);
        Debug.Log("Choice: " + choice);
        Entertained = false;
        gameRequest.SetActive(false);

        if (choice == "Yes")
        {
            StartCoroutine(RPS.RockPaperScissors());
        }
        else
        {
            PR.SetActive(true);
            attitude = 2;
            if (currentAttitudeReset != null) { StopCoroutine(currentAttitudeReset);
            Debug.Log("Stopped Existing AR"); }
            currentAttitudeReset = StartCoroutine(attitudeReset(Random.Range(2, 11) * 60));
            uninterupt();
            GameButton.interactable = true;
            StartCoroutine(BoredTrigger(waitForBoredom * 60));
        }
    }

    // Waits
    IEnumerator Pause(float duration)
    {
        float timePassed = 0f;
        if (interupted)
            {
                Debug.Log("Pause stopped");
                yield break; // Exit the coroutine
            }
        while (timePassed < duration)
            {
                timePassed += Time.deltaTime;
                yield return null; // Wait for the next frame
            }
        
        doingsomething = false;
        if (longWait) {
            longWait = false;
        }
        currentPause = null;
    }   
    
    public void interupt() //Make Keno stop what he is doing
    {
        interupted = true;
        doingsomething = true;
        StopAllVideos();
    }
    public void uninterupt()
    {
        StopAllVideos();
        interupted = false;
        doingsomething = false;
    }

    public IEnumerator attitudeReset(float duration)
    {
        Debug.Log("Attitude Reset Timer Started");
        float timePassed = 0f;

        while (timePassed < duration)
        {
            timePassed += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        attitude = 0;
        Debug.Log("Attitude Reset");
        currentAttitudeReset = null; // Clear the reference when done
    }

    public IEnumerator BoredTrigger(float waitForBoredAgain)
    {
        float elapsedTime = 0f;
        while (elapsedTime < waitForBoredAgain)
        {
            if (Entertained)
            {
                Entertained = false;
                Debug.Log("Bored Trigger Cancled");
                yield break; // Exit the coroutine
            }
            else if(GameReqOn == false)
            {
                Debug.Log("Game Requests Are Disabled");
                yield break;
            }
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }
        isBored = true;
    }

    public void SetProceedTrue(string optionID)
    {
        ProceedTrue = true;
        choice = optionID;
        Debug.Log(choice);
    }

    // Events
    public void AskGame()
    {
        PR.SetActive(false);
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
        PR.SetActive(false);
        interupt();
        ProceedTrue = false;
        isBored = false;
        Entertained = true;
        ChooseGame();
        yield return new WaitUntil(() => ProceedTrue);
        Debug.Log("Choice: " + choice);
        gameSelectGUI.SetActive(false);
        gameSelectGUI2.SetActive(false);
        Entertained = false;
        if (choice == "RPS")
        {
            StartCoroutine(RPS.RockPaperScissors());
        }
        else if (choice == "Modes") { }
        else
        {
            PR.SetActive(true);
            uninterupt();
            Entertained = false;
            GameButton.interactable = true;
        }
    }

    public void ModeEnter(string Mode)
    {
        StartCoroutine(Modes.ModeHandle(Mode));
        Debug.Log("Mode Handle Initiated");
    }

    //stops all videos
    public void StopAllVideos()
    {
        VideoPlayer[] videos = { Rock, Paper, Scissors, CustomExpression, HappyCustomExpression, SadCustomExpression, NeutralCustomExpression, Blink, Joy, Joy2, Sad, Mad, Bored1, Bored2, Confused, CoolGlasses, 
        LookTopLeft, LookTopRight, LookBottomLeft, LookBottomRight, LookRightS, LookRightL, SleepStart, SleepEnd, ReadStart, ReadLoop, ReadEnd, ReadStart2, ReadLoop2, ReadEnd2, MovieStart, MovieLoop, MovieEnd,
        Poked, Alarm };
        foreach (VideoPlayer video in videos)
        {
            video.Stop();
            video.gameObject.SetActive(false);
        }
    }

    // Play Video
    public void PlayVideo(VideoPlayer videoPlayer)
    {
        StopAllVideos();
        videoPlayer.gameObject.SetActive(true);
        videoPlayer.Play();
    }
}
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
    [Tooltip("This is the reference to the eyes, which should be a GameObject with a SpriteRenderer")] public SpriteRenderer Eyes;
    [Tooltip("This is the reference to the `Modes` script, which should be on a GameObject")] public Modes Modes;
    [Tooltip("This is the reference to the `RPS` script, which should be on a GameObject")] public RPS RPS;
    [Tooltip("This is the reference to the `GameRequests ` script, which should be on a GameObject")] public GameRequests GameRequests;

    [Header("Behaviour values")]
    public string choice; 
    public bool longWait = false;
    public bool ProceedTrue = false;
    public bool doingsomething = false;
    public bool Entertained = false;
    public bool interupted = false;
    public bool ResetAttitudeTimer = false;
    private bool asleepThisSession = false;
    public bool GameReqOn;
    public int whatToDo = 0;
    public float variant;
    public int attitude = 0; // 0 = normal, 1 = happy/cool, 2 = sad/angry
    [Header("Game related stuff and buttons")]
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
    [Tooltip("This is the short variant of the animation")] public VideoPlayer LookLeftS;
    [Tooltip("This is the long variant of the animation")] public VideoPlayer LookRightL;
    [Tooltip("This is the long variant of the animation")] public VideoPlayer LookLeftL;
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
    public VideoPlayer Alarm;
    public Sprite Poked;
    [Header("Sprites for static eyes")]
    [Tooltip("This should be what the eye gameobject image is by default")]public Sprite Normal;

    void Start()
    {
        Tutorial.isTutorial = false; //Ensures that we are not in the tutorial
        StopAllVideos(); //Ensure all VideoPlayers are initially stopped and disabled
        GameReqOn = PlayerPrefs.GetInt("Game Requests", 1) == 1 ? true : false;
        RPS.RPSAssets.SetActive(false);
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
                    GameRequests.ModeEnter("Sleep");
                    attitude = 0;
                    asleepThisSession = true;
                }
                else if(GameRequests.isBored)
                {
                    StartCoroutine(GameRequests.HandleBoredState());
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
                    PlayVideo(variant == 1 ? LookRightS : LookLeftS);
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
                    else if (attitude == 0) { variant = Random.Range(1,3); PlayVideo(variant == 1 ? LookRightL : LookLeftL); longWait = true;}
                }

                if (currentPause != null) { StopCoroutine(currentPause);  Debug.Log("Stopped Existing Pause"); }
                currentPause = StartCoroutine(Pause(longWait ? 6f : Random.Range(3.7f, 4f)));
            }
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

    public void SetProceedTrue(string optionID)
    {
        ProceedTrue = true;
        choice = optionID;
        Debug.Log(choice);
    }

    //stops all videos
    public void StopAllVideos()
    {
        VideoPlayer[] videos = { Rock, Paper, Scissors, CustomExpression, HappyCustomExpression, SadCustomExpression, NeutralCustomExpression, Blink, Joy, Joy2, Sad, Mad, Bored1, Bored2, Confused, CoolGlasses, 
        LookTopLeft, LookTopRight, LookBottomLeft, LookBottomRight, LookRightS, LookLeftS, LookRightL, LookLeftL, SleepStart, SleepEnd, ReadStart, ReadLoop, ReadEnd, ReadStart2, ReadLoop2, ReadEnd2, MovieStart, MovieLoop, MovieEnd,
        Alarm };
        foreach (VideoPlayer video in videos)
        {
            if(video != null) {
                video.Stop();
                video.gameObject.SetActive(false);
            }
        }
    }

    // Play Video
    public void PlayVideo(VideoPlayer videoPlayer)
    {
        if(videoPlayer != null) {
            StopAllVideos();
            videoPlayer.gameObject.SetActive(true);
            videoPlayer.Play();
        }
    }
}
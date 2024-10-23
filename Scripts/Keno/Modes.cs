using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

// This script holds Keno's modes

public class Modes : MonoBehaviour
{
    public BEHAVIOUR Bh;

    public IEnumerator ModeHandle(string Mode)
    {
        Debug.Log("Mode Handle Entered");
        Bh.PR.SetActive(false);
        Bh.interupt();
        Bh.GameRequests.GameButton.interactable = false;
        Bh.ProceedTrue = false;
        Bh.Entertained = true;
        Bh.StopActivityButton.SetActive(true);
        Bh.ModeSelectGUI.SetActive(false);
        switch (Mode)
        {
            case "Movie": // Movie mode has not been used yet, as i haven't gotten around to making the animations or extra functions for it
                Bh.PlayVideo(Bh.MovieStart);
                yield return new WaitUntil(() => Bh.ProceedTrue);
                if (Bh.choice == "Stop Activity")
                {
                    StartCoroutine(endMode(Bh.MovieEnd, 3.5f));
                }
                break;
            case "Reading":
                Bh.variant = Random.Range(1,3);
                VideoPlayer ReadStartVar; VideoPlayer ReadLoopVar; VideoPlayer ReadEndVar;
                if (Bh.variant == 1) {ReadStartVar = Bh.ReadStart; ReadLoopVar = Bh.ReadLoop; ReadEndVar = Bh.ReadEnd;}
                else /*if (variant == 2)*/ {ReadStartVar = Bh.ReadStart2; ReadLoopVar = Bh.ReadLoop2; ReadEndVar = Bh.ReadEnd2;}
                Bh.PlayVideo(ReadStartVar);
                Bh.attitude = 0; //Set his attitude (mood) to neutral
                yield return new WaitForSeconds(1.3f);
                Bh.PlayVideo(ReadLoopVar);
                yield return new WaitUntil(() => Bh.ProceedTrue);
                if (Bh.choice == "Stop Activity")
                {
                    StartCoroutine(endMode(ReadEndVar, 1.5f));
                }
                break;
            case "Sleep": // I would really love to add a little snore thing some day (similar to that of an Anki Vector robot)
                Bh.PlayVideo(Bh.SleepStart);
                Bh.attitude = 0; //Set his attitude (mood) to neutral
                yield return new WaitUntil(() => Bh.ProceedTrue);
                if (Bh.choice == "Stop Activity")
                {
                    StartCoroutine(endMode(Bh.SleepEnd, 2));
                }
                break;

            default:
                Debug.LogWarning("Unknown Mode: " + Mode);
                break;
        }
    }

    public IEnumerator endMode(VideoPlayer End, float waitTime) // Stop whatever mode is active.
    {
        Bh.ProceedTrue = false;
        Bh.StopActivityButton.SetActive(false);
        Bh.StopAllVideos();
        yield return new WaitForEndOfFrame();
        Bh.PlayVideo(End);
        yield return new WaitForSeconds(waitTime); //`waitTime` is the length of the wait after the given animation; should be around the length of the End animation
        StartCoroutine(Bh.GameRequests.BoredTrigger(Bh.GameRequests.waitForBoredom * 60));
        Bh.uninterupt();
        Bh.PR.SetActive(true);
        Bh.GameRequests.GameButton.interactable = true;
        Debug.Log("Activity Stopped");
        yield break;
    }
}

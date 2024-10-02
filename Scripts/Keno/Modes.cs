using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Modes : MonoBehaviour
{
    public BEHAVIOUR Bh;

    public IEnumerator ModeHandle(string Mode)
    {
        Debug.Log("Mode Handle Entered");
        Bh.interupt();
        Bh.GameButton.interactable = false;
        Bh.ProceedTrue = false;
        Bh.Entertained = true;
        Bh.StopActivityButton.SetActive(true);
        Bh.ModeSelectGUI.SetActive(false);
        switch (Mode)
        {
            case "Movie": // Movie mode has not been used yet, as i haven't gotten around to making the animations or extra functions for it
                Bh.PlayVideo(Bh.MovieStart);
                yield return new WaitUntil(() => Bh.ProceedTrue);
                if (Bh.choice == "Stop Activity" || Bh.choice == "Stop Activity A")
                {
                    Bh.ProceedTrue = false;
                    Bh.StopActivityButton.SetActive(false);
                    Bh.MovieLoop.Stop();
                    Bh.PlayVideo(Bh.MovieEnd);
                    yield return new WaitForSeconds(3.5f);
                    Bh.GameButton.interactable = true;
                    Bh.doingsomething = false;
                    Debug.Log("Activity Stopped");
                    if(Bh.choice != "Stop Activity A") {Bh.uninterupt();}
                    yield break;
                }
                break;
            case "Reading":
                    Bh.variant = Random.Range(1,3);
                    VideoPlayer ReadStartVar; VideoPlayer ReadLoopVar; VideoPlayer ReadEndVar;
                    if (Bh.variant == 1) {ReadStartVar = Bh.ReadStart; ReadLoopVar = Bh.ReadLoop; ReadEndVar = Bh.ReadEnd;}
                    else /*if (variant == 2)*/ {ReadStartVar = Bh.ReadStart2; ReadLoopVar = Bh.ReadLoop2; ReadEndVar = Bh.ReadEnd2;}
                    Bh.PlayVideo(ReadStartVar);
                    Bh.attitude = 0;
                    yield return new WaitForSeconds(1.3f);
                    Bh.PlayVideo(ReadLoopVar);
                    yield return new WaitUntil(() => Bh.ProceedTrue);
                    if (Bh.choice == "Stop Activity" || Bh.choice == "Stop Activity A")
                    {
                        Bh.ProceedTrue = false;
                        Bh.StopActivityButton.SetActive(false);
                        Bh.StopAllVideos();
                        yield return new WaitForSeconds(0.1f);
                        Bh.PlayVideo(ReadEndVar);
                        yield return new WaitForSeconds(1.5f);
                        Bh.GameButton.interactable = true;
                        StartCoroutine(Bh.BoredTrigger(25 * 60));
                        Debug.Log("Activity Stopped");
                        if(Bh.choice != "Stop Activity A") {Bh.uninterupt();}
                        yield break;
                    }
                    break;
            case "Sleep": // I would really love to add a little snore thing some day (similar to that of an Anki Vector robot)
                Bh.PlayVideo(Bh.SleepStart);
                Bh.attitude = 0;
                yield return new WaitUntil(() => Bh.ProceedTrue);
                if (Bh.choice == "Stop Activity" || Bh.choice == "Stop Activity A")
                {
                    Bh.ProceedTrue = false;
                    Bh.StopActivityButton.SetActive(false);
                    Bh.StopAllVideos();
                    Bh.PlayVideo(Bh.SleepEnd);
                    yield return new WaitForSeconds(2f);
                    Bh.GameButton.interactable = true;
                    StartCoroutine(Bh.BoredTrigger(30 * 60));
                    Debug.Log("Activity Stopped");
                    if(Bh.choice != "Stop Activity A") {Bh.uninterupt();}
                    yield break;
                }
                break;

            default:
                Debug.LogWarning("Unknown Mode: " + Mode);
                break;
        }
    }
}

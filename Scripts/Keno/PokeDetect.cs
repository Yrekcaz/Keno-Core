using UnityEngine;
using UnityEngine.EventSystems;

// This script handles Keno's eyes being pressed/poked via an invisible button on top of his eyes

public class PokeDetect : MonoBehaviour
{
    public BEHAVIOUR Bh;
    private bool isHolding = false;
    private float holdStartTime;
    public float holdThreshold = 0.3f;

    // This will be linked to the EventTrigger's Pointer Down event
    public void OnPointerDown(BaseEventData eventData)
    {
        isHolding = true;
        holdStartTime = Time.time;
        Debug.Log("Button Pressed - Start the hold");
        StartVideo();
    }

    // This will be linked to the EventTrigger's Pointer Up event
    public void OnPointerUp(BaseEventData eventData)
    {
        isHolding = false;
        Debug.Log("Button Released - End the hold");
        StopVideo();
    }

    void Update()
    {
        if (isHolding)
        {
            float holdDuration = Time.time - holdStartTime;
            if (holdDuration >= holdThreshold)
            {
                Debug.Log("Button held for: " + holdDuration + " seconds");
            }
        }
    }

    private void StartVideo()
    {
        Debug.Log("Starting video...");
        Bh.interupt(); //Stop Keno's thought process
        Bh.PlayVideo(Bh.Poked); //Play the `Poked` animation
    }

    private void StopVideo()
    {
        Debug.Log("Stopping video...");
        Bh.uninterupt(); //Allow Keno to go back to the main loop
    }
}

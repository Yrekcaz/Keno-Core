using UnityEngine;
using UnityEngine.EventSystems;

// Poke Detection is in the midst of being made, just dealing with probably unrelated issues in Rock Paper Scissors

public class PokeDetect : MonoBehaviour
{
    public BEHAVIOUR Bh;
    private bool isHolding = false;
    private float holdStartTime;
    public float holdThreshold = 0.5f;

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
        Bh.interupt();
        //Bh.PlayVideo(Bh.Poked);
    }

    private void StopVideo()
    {
        Debug.Log("Stopping video...");
        Bh.uninterupt();
    }
}

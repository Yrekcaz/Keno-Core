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
        StartPoke();
    }

    // This will be linked to the EventTrigger's Pointer Up event
    public void OnPointerUp(BaseEventData eventData)
    {
        isHolding = false;
        Debug.Log("Button Released - End the hold");
        StopPoke();
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

    private void StartPoke()
    {
        Debug.Log("Started");
        Bh.interupt(); //Stop Keno's thought process
        Bh.Eyes.sprite = Bh.Poked; //Play the `Poked` animation
    }

    private void StopPoke()
    {
        Debug.Log("Stopped");
        Bh.Eyes.sprite = Bh.Normal;
        Bh.uninterupt(); //Allow Keno to go back to the main loop
    }
}

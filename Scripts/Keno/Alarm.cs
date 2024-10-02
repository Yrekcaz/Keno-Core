using System;
using UnityEngine;

// This script is currently pretty much unused until i get to making the settings page for this

public class Alarm : MonoBehaviour
{
    public int alarmHour = 7;
    public int alarmMinute = 0;
    public bool alarmEnabled = false;

    public bool isDebugging = false;

    public BEHAVIOUR Bh; //reference to behaviour script

    public AudioSource AlarmSound;

    public GameObject StopAlarm;
    public GameObject StopActivityButton;

    void Start()
    {
        alarmEnabled = PlayerPrefs.GetInt("alarmEnabled", 0) == 1 ? true : false;
        if(!isDebugging) { 
            alarmHour = PlayerPrefs.GetInt("alarmHour", 7); //default to 7 if null
            alarmMinute = PlayerPrefs.GetInt("alarmMinute", 0); //default to 00 (aka 'o clock) if null
        }
        if(alarmEnabled)
            StartCoroutine(SetDailyAlarm());
    }

    private System.Collections.IEnumerator SetDailyAlarm()
    {
        if(!alarmEnabled) { yield break;}
        while (true)
        {
            DateTime currentTime = DateTime.Now;
            DateTime nextAlarmTime = GetNextAlarmTime(currentTime);
            TimeSpan timeUntilNextAlarm = nextAlarmTime - currentTime;

            Debug.Log($"Next alarm set for: {nextAlarmTime}");
            yield return new WaitForSeconds((float)timeUntilNextAlarm.TotalSeconds);

            TriggerAlarm();
        }
    }

    private DateTime GetNextAlarmTime(DateTime currentTime)
    {
        DateTime todayAlarm = currentTime.Date.AddHours(alarmHour).AddMinutes(alarmMinute);

        // If it's past the alarm time today, set it for tomorrow
        if (currentTime >= todayAlarm)
        {
            return todayAlarm.AddDays(1);
        }
        return todayAlarm;
    }

    private void TriggerAlarm()
    {
        if(alarmEnabled) {
            Debug.Log("Alarm is going off!!");
            StopActivityButton.SetActive(false);
            Bh.interupt();
            Bh.PlayVideo(Bh.Alarm);
            AlarmSound.Play();
            StopAlarm.SetActive(true);
        }
    }
    
    public void dismiss()
    {
        Bh.uninterupt();
        AlarmSound.Stop();
        StopAlarm.SetActive(false);
    }

    public void PlayAlarmAudio()
    {
        if (AlarmSound != null)
        {
            AlarmSound.Play();
        }
    }

}

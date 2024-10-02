using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DateDisplay : MonoBehaviour
{
    public TextMeshProUGUI ageText;
    public TextMeshProUGUI TWUK; //Times Woken Up Keno

    void Start()
    {
        string savedDateStr = PlayerPrefs.GetString("BirthDate", null);
        int TWUKint = PlayerPrefs.GetInt("TWUK", 0);
        if (!string.IsNullOrEmpty(savedDateStr))
        {
            System.DateTime savedDate = System.DateTime.Parse(savedDateStr);
            System.TimeSpan difference = System.DateTime.Now - savedDate;
            DisplayDateDifference(difference);
        }
        else
        {
            ageText.text = "Age: Being set, check back later";
            SetBirthDate();
            System.DateTime savedDate = System.DateTime.Parse(savedDateStr);
            System.TimeSpan difference = System.DateTime.Now - savedDate;
            DisplayDateDifference(difference);
        }
        TWUK.text = "Times Woken Up: " + TWUKint;
    }

    public void SetBirthDate()
    {
        System.DateTime currentDate = System.DateTime.Now;
        PlayerPrefs.SetString("BirthDate", currentDate.ToString());
        PlayerPrefs.Save();
        Debug.Log("Birthday Set To: " + currentDate.ToString());
    }

    void DisplayDateDifference(System.TimeSpan difference)
    {
        int days = difference.Days;
        int hours = difference.Hours;
        int minutes = difference.Minutes;
        //int seconds = difference.Seconds;

        ageText.text = $"Age: {days} days, {hours} hours, and {minutes} minutes"; // {seconds} seconds
    }
}

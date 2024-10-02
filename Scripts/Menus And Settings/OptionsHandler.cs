using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionsHandler : MonoBehaviour
{
    public string eyeHEX;
    public Image eyePreview; // Reference for the eye color preview image
    public Renderer eyeBase; // Reference for the renderer to apply eye color
    public bool isMainMenu; 
    public bool gameRequestsOn;
    public bool BedtimeEnabled;
    public Toggle GameReqToggle; // Reference to the game request toggle
    public Toggle AutoBootToggle;
    public Toggle BedtimeToggle;
    public bool autoBoot = false;
    public GameObject TutorialPromt;
    private static bool hasInitialLoadOccurred = false; // Static variable to track initial load

    void Start()
    {
        // Load the saved preferences
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        gameRequestsOn = PlayerPrefs.GetInt("Game Requests", 1) == 1;
        BedtimeEnabled = PlayerPrefs.GetInt("Bedtime Enabled", 1) == 1;
        eyeHEX = PlayerPrefs.GetString("Eye Color", "#FFFFFF");
        if (isMainMenu) 
        {
            TutorialPromt.SetActive(false);
            if(PlayerPrefs.GetInt("FirstBoot", 0) == 0)
            {
                PromtTutorial();
            }
            autoBoot = PlayerPrefs.GetInt("Auto Boot", 0) == 0 ? false : true;
            if (!hasInitialLoadOccurred)
            {
                if (autoBoot == true)
                {
                    SceneManager.LoadScene("KENO");
                }
            }
            hasInitialLoadOccurred = true;
            UpdatePreviewEyeColor(eyeHEX);
            GameReqToggle.isOn = gameRequestsOn;
            AutoBootToggle.isOn = autoBoot;
            BedtimeToggle.isOn = BedtimeEnabled;
            GameReqToggle.onValueChanged.AddListener(delegate {
                gameReqHandle();
            });
            AutoBootToggle.onValueChanged.AddListener(delegate {
                autoBootHandle();
            });
            BedtimeToggle.onValueChanged.AddListener(delegate {
                BedtimeToggleHandle();
            });
        }
        else 
        {
            PlayerPrefs.SetInt("TWUK", PlayerPrefs.GetInt("TWUK", 0) + 1);
            ApplyEyeColor(eyeHEX);
        }
    }   

    // This method updates the eye color preview in the main menu
    private void UpdatePreviewEyeColor(string hexColor)
    {
        Color newColor;
        ColorUtility.TryParseHtmlString(hexColor, out newColor);
        eyePreview.color = newColor;
    }

    // This method applies the eye color to the in-game object
    private void ApplyEyeColor(string hexColor)
    {
        Color newColor;
        ColorUtility.TryParseHtmlString(hexColor, out newColor);
        eyeBase.material.color = newColor;
        Debug.Log("Eye Color Applied: " + hexColor);
    }

    // This method sets and saves the eye color preference
    public void setEyeColor(string HEX)
    {
        PlayerPrefs.SetString("Eye Color", HEX);
        PlayerPrefs.Save(); // Save the changes immediately
        Debug.Log("Eye Color set to: " + HEX);

        if (isMainMenu) 
        {
            UpdatePreviewEyeColor(HEX);
        } 
        else 
        {
            ApplyEyeColor(HEX);
        }
    }

    // This method toggles the game request preference
    public void gameReqHandle()
    {
        gameRequestsOn = !gameRequestsOn;
        PlayerPrefs.SetInt("Game Requests", gameRequestsOn ? 1 : 0);
        PlayerPrefs.Save();
        Debug.Log("Game Requests Toggled: " + gameRequestsOn);
    }
    public void autoBootHandle()
    {
        autoBoot = !autoBoot;
        PlayerPrefs.SetInt("Auto Boot", autoBoot ? 1 : 0);
        PlayerPrefs.Save();
        Debug.Log("Auto Boot Into Keno: " + autoBoot);
    }
    public void BedtimeToggleHandle()
    {
        BedtimeEnabled = !BedtimeEnabled;
        PlayerPrefs.SetInt("Bedtime Enabled", BedtimeEnabled ? 1 : 0);
        PlayerPrefs.Save();
        Debug.Log("Bedtime Toggled: " + BedtimeEnabled);
    }
    public void StartTutorial()
    {
        Debug.Log("Sent to tutorial");
        SceneManager.LoadScene("Tutorial");
    }
    public void PromtTutorial()
    {
        TutorialPromt.SetActive(true);
    }
    public void TutorialAccept()
    {
        PlayerPrefs.SetInt("FirstBoot", 1);
        PlayerPrefs.Save();
        SceneManager.LoadScene("Tutorial");
    }
    public void TutorialDeny()
    {
        PlayerPrefs.SetInt("FirstBoot", 1);
        PlayerPrefs.Save();
        TutorialPromt.SetActive(false);
    }
}

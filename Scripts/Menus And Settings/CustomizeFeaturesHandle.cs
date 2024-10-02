using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;
using TMPro;
using Unity.Burst.CompilerServices;

public class CustomizeFeaturesHandle : MonoBehaviour
{
    public bool CustomExpressionOn;
    public bool ConstantOn;
    public Toggle CustomExpressionToggle;
    public Toggle ConstantToggle;
    public Button ImportMP4Button;
    public Button ImportHappyButton;
    public Button ImportSadButton;
    public Button ImportNeutralButton;
    public GameObject NotConstantGameObject;
    public TextMeshProUGUI ImportMP4Text;
    public TextMeshProUGUI ImportHappyText;
    public TextMeshProUGUI ImportSadText;
    public TextMeshProUGUI ImportNeutralText;

    void Start()
    {
        CustomExpressionOn = PlayerPrefs.GetInt("Custom Expression Enabled", 0) == 1 ? true : false;
        ConstantOn = PlayerPrefs.GetInt("Constant", 1) == 1 ? true : false;
        CustomExpressionToggle.isOn = CustomExpressionOn;
        ConstantToggle.isOn = ConstantOn;
        ImportMP4Button.interactable = CustomExpressionOn;
        ImportHappyButton.interactable = CustomExpressionOn;
        ImportSadButton.interactable = CustomExpressionOn;
        ImportNeutralButton.interactable = CustomExpressionOn;
        ConstantToggle.interactable = CustomExpressionOn;
        NotConstantGameObject.SetActive(!ConstantOn);
        ImportMP4Button.gameObject.SetActive(ConstantOn);
        CustomExpressionToggle.onValueChanged.AddListener(delegate {
            ExpressionEnableToggle();
        });
        ConstantToggle.onValueChanged.AddListener(delegate {
            ConstantEnableToggle();
        });
        if(Application.platform == RuntimePlatform.WindowsPlayer)
        {
            CustomExpressionToggle.interactable = false;
            CustomExpressionToggle.isOn = false;
            CustomExpressionOn = false;
        }
        /*if (CustomExpressionToggle.isOn == true && CustomExpressionOn == true)
        {
            //START OF ROW 2 CODE
        }*/
    }

    void Update()
    {
        
    }

    public void ExpressionEnableToggle()
    {
        CustomExpressionOn = !CustomExpressionOn;
        ImportMP4Button.interactable = CustomExpressionOn;
        ImportHappyButton.interactable = CustomExpressionOn;
        ImportSadButton.interactable = CustomExpressionOn;
        ImportNeutralButton.interactable = CustomExpressionOn;
        ConstantToggle.interactable = CustomExpressionOn;
        PlayerPrefs.SetInt("Custom Expression Enabled", CustomExpressionOn == true ? 1 : 0);
        PlayerPrefs.Save();
        Debug.Log("Custom Expression Toggled To: " + CustomExpressionOn);
    }
    public void ConstantEnableToggle()
    {
        ConstantOn = !ConstantOn;
        ImportMP4Button.interactable = ConstantOn;
        ImportMP4Button.gameObject.SetActive(ConstantOn);
        ImportHappyButton.interactable = !ConstantOn;
        ImportSadButton.interactable = !ConstantOn;
        ImportNeutralButton.interactable = !ConstantOn;
        NotConstantGameObject.SetActive(!ConstantOn);
        PlayerPrefs.SetInt("Constant", ConstantOn == true ? 1 : 0);
        PlayerPrefs.Save();
        Debug.Log("Constant Toggled To: " + ConstantOn);
    }
    public void SelectMP4Pressed(string type)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead))
            {
                Permission.RequestUserPermission(Permission.ExternalStorageRead);
            }
        }
        StartCoroutine(PickVideo(type, type == "SelectedVideoPath" ? ImportMP4Text : type == "HappyVideoPath" ? ImportHappyText : type == "SadVideoPath" ? ImportSadText : ImportNeutralText ));
    }
    private IEnumerator PickVideo(string type, TextMeshProUGUI textName)
    {
        yield return new WaitForSeconds(0.2f);

        NativeGallery.Permission permission = NativeGallery.GetVideoFromGallery((path) =>
        {
            if (path != null)
            {
                // User selected a video file
                PlayerPrefs.SetString(type, path);
                PlayerPrefs.Save();
                //textName.text = path;
            }
        }, "Select a Video");

        if (permission == NativeGallery.Permission.Denied || permission == NativeGallery.Permission.ShouldAsk)
        {
            // Permission denied, show a message to the user
            Debug.Log("Permission to access the gallery was denied.");
        }
    }
    public void OpenDriveLink()
    {
        Application.OpenURL("https://drive.google.com/drive/folders/1g2JWixMCNBfmGVO0Fj2afdTYAIZWnxnO");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour {
	public bool KenoRefresh;
	public static bool LFR = false; //loaded from refresh?
	void Start()
	{
		if(KenoRefresh == true)
		{
			LFR = true;
			if(Tutorial.isTutorial == true)	{
				SceneManager.LoadScene("Tutorial");
			}
			else {
				SceneManager.LoadScene("KENO");
			}
		} 
	}
	public void LoadScene(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
	}
}

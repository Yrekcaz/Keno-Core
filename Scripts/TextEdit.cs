using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextEdit : MonoBehaviour {

	public Text textToEdit;

	void Update() {
	
	}
	
	public void TextChange(string text)
	{
		textToEdit.text = text;
	}
}

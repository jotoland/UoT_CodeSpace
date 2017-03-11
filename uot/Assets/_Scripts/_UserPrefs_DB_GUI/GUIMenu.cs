using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/***
 *John G. Toland 
 * This script is for the Pause/Naviagiton Menu GUI during gameplay
 * From here the user can navigate out of the app
 * back to ship selection and level selection.
 * The user can also resume gameplay
*/
public class GUIMenu : MonoBehaviour {

	//variables used to manipulate the GUI
	private string currentGUI;
	private GUIStyle mGUIStyleText;
	private GUIStyle mGUIStyleBox;
	private GUIStyle mButtonStyle;
	//private GUIStyle mGUIStyleInput;
	public GUIStyle mGUIStyleMessage;


	// Use this for initialization
	void Start () {
		//this is the pause button
		currentGUI = "Button";
	}

	// checks the current GUI
	void OnGUI(){
		if (currentGUI == "Button") {
			ButtonGUI ();
		} else if (currentGUI == "NavigationMenu") {
			NavigationMenu ();
		}
	}

	/// function that controls the pause/navigation menu
	void ButtonGUI(){
		mButtonStyle = new GUIStyle(GUI.skin.button);
		mButtonStyle.normal.textColor = Color.white;
		mButtonStyle.hover.textColor = Color.green;
		mButtonStyle.fontSize = 30;

		if (GUI.Button (new Rect (Screen.width/1-40,1,30,30), "||" , mButtonStyle)) {
			currentGUI = "NavigationMenu";
			Time.timeScale = 0;
			AudioListener.pause = true;
		}
	}


	/// <summary>
	/// /?????
	/// </summary>
	void NavigationMenu (){
		//GUI Box
		mGUIStyleBox = new GUIStyle (GUI.skin.box);
		//mGUIStyleInput = new GUIStyle (GUI.skin.textField);
		mGUIStyleText = new GUIStyle ();
		mGUIStyleMessage = new GUIStyle ();
		mGUIStyleBox.fontSize = 60;

		///GUI Text
		mGUIStyleText.normal.textColor = Color.white;
		mGUIStyleMessage.normal.textColor = Color.red;
		mGUIStyleMessage.fontSize = 25;
		mGUIStyleText.fontSize = 60;

		///GUI Buttons
		mButtonStyle = new GUIStyle(GUI.skin.button);
		mButtonStyle.normal.textColor = Color.white;
		mButtonStyle.hover.textColor = Color.green;
		mButtonStyle.fontSize = 30;
		GUI.Box (new Rect (Screen.width / 8, Screen.height / 16, (Screen.width / 4) + 300, (Screen.height / 4) + 500), "", mGUIStyleBox);
		GUI.Box (new Rect (Screen.width / 8, Screen.height / 16, (Screen.width / 4) + 300, (Screen.height / 4) + 500), "", mGUIStyleBox);
		GUI.Box (new Rect (Screen.width / 8, Screen.height / 16, (Screen.width / 4) + 300, (Screen.height / 4) + 500), "", mGUIStyleBox);
		GUI.Box (new Rect (Screen.width/8, Screen.height/16, (Screen.width/4)+300, (Screen.height/4)+500), "Menu:", mGUIStyleBox);
		if (GUI.Button (new Rect (Screen.width/4+25, Screen.height/2,240,70), "Resume", mButtonStyle)) {
			currentGUI = "Button";
			Time.timeScale = 1;
			AudioListener.pause = false;
		}else if (GUI.Button (new Rect (Screen.width/4+25, Screen.height/2+80,240,70), "Exit", mButtonStyle)) {
			Application.Quit ();
		}
	}
}
//finito

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// User account.
/// John G. Toland  2/5/17
/// This is the login and create account GUI script
/// This script calls the COROUTINES that login and create accounts with the DB
/// The username is stored in player prefs for the first load.
/// After that all data is passed and stored in the DB
/// </summary>
public class GUIAccount : MonoBehaviour {

	//instance of the CoRoutines class
	private CoRoutines CoRo;
	//custome guistyle variables
	private GUIStyle mGUIStyleText;
	private GUIStyle mGUIStyleBox;
	private GUIStyle mButtonStyle;
	private GUIStyle mGUIStyleInput;
	public GUIStyle mGUIStyleMessage;

	//variables for feedback to the user during login/create account
	public string outPutString;
	public bool showMessage = false;
	public bool showMessageGood = false;

	//variables for user with existing account
	private static string userName = "";
	private static string password = "";
	public string currentMenu = "Login";

	//variables for creating an account
	private string confirmPass = "";
	private string confirmUserName = "";
	private string cUserName = "";
	private string cPassword = "";
	//used to check the connection
	private int connection;

	//variable for masking password
	private char maskChar = '*';
	public static Color backgroundColor;

	void Start () {
		//make sure the audio is not paused
		AudioListener.pause = false;

		GameObject CoRoObject = GameObject.FindGameObjectWithTag ("CoRoutines");
		//make sure we can access our instance of CoRo
		if (CoRoObject != null) {
			CoRo = CoRoObject.GetComponent <CoRoutines> ();
		}
		///checking the connection
		CoRo.CheckConnection ();
	}

	/// Raises the GU event.
	void OnGUI(){
		GUI.backgroundColor = Color.blue;
		if (currentMenu == "Login") {
			LoginGUI ();
		} else if (currentMenu == "CreateAccount") {
			CreateAccountGUI ();
		}
	}

	#regionCustomFunctions
	//Login GUI first on scene
	void LoginGUI(){

		////Allocating GUI objects
		mGUIStyleBox = new GUIStyle (GUI.skin.box);
		mGUIStyleInput = new GUIStyle (GUI.skin.textField);
		mGUIStyleText = new GUIStyle ();
		mGUIStyleMessage = new GUIStyle ();
		mButtonStyle = new GUIStyle(GUI.skin.button);

		///GUI Text
		mGUIStyleText.normal.textColor = Color.white;
		mGUIStyleMessage.normal.textColor = Color.red;
		mGUIStyleMessage.fontSize = 25;
		mGUIStyleText.fontSize = 30;

		///GUI Buttons
		mButtonStyle.normal.textColor = Color.white;
		mButtonStyle.hover.textColor = Color.green;
		mButtonStyle.fontSize = 30;

		///GUI Text box and input text size
		mGUIStyleBox.fontSize = 35;
		mGUIStyleInput.fontSize = 35;
		///getting connection from PlayerPrefs incase of bad connection
		/// this is grabbing the connection from player prefs (coroutines stored it there when checking the connection)
		connection = PlayerPrefs.GetInt ("mConnection");
		GUI.Box (new Rect (Screen.width/8, Screen.height/16, (Screen.width/4)+300, (Screen.height/4)+375), "Login Menu\n Welcome", mGUIStyleBox);
		if (GUI.Button (new Rect (Screen.width/4+25, Screen.height/2+40,240,70), "Log In", mButtonStyle)) {
			//the one time player preferences is used (local storage in the unity engine)
			PlayerPrefs.SetString ("mUserName", userName);
			showMessage = false;
			if (connection == 1) {
				//connection is good, us DB
				CoRo.LoginAccount (userName, password);
			} else {
				//connection is bad or none at all, us PlayerPrefs (!!!!!Does not Save state!!!!!!!)
				LoadPlayerSelectionScene ();
			}
		}
		//if the connection is good the allow the user to create an account on DB
		//if connection is bad then the user can only login with an arbitrary user name
		if (connection == 1) {
			if (GUI.Button (new Rect (Screen.width/4+25,Screen.height/2+125,240,70), "Create Account", mButtonStyle)) {
				showMessage = false;
				currentMenu = "CreateAccount";
			}
			GUI.Label (new Rect (Screen.width/4-40, Screen.height/4+60 , 200, 23), "Password:", mGUIStyleText);
			password = GUI.PasswordField (new Rect (Screen.width/4, Screen.height/4+100, 300, 50), password,maskChar ,mGUIStyleInput);
		} else {
			//message to user off bad connection and offline play beginning
			GUI.Label (new Rect (Screen.width/8+5, Screen.height/3, 200, 23), "[ello]:\nBad Connection\n[off line play initializing...]\n[ello]:\nProceed with Login", mGUIStyleMessage);
		}
		GUI.Label (new Rect (Screen.width/4-40, Screen.height/6+20, 200, 23), "User Name:", mGUIStyleText);
		userName = GUI.TextField (new Rect (Screen.width/4, Screen.height/6+60, 300, 50), userName, mGUIStyleInput);
		if (showMessage) {
			GUI.Label (new Rect (Screen.width/8+5, Screen.height/3+80, 200, 23), "[ello]:\n" + outPutString, mGUIStyleMessage);
		} else if (showMessageGood) {
			mGUIStyleMessage.normal.textColor = Color.green;
			GUI.Label (new Rect (Screen.width/8+5, Screen.height/3+80, 200, 23), "[ello]:\n" + outPutString, mGUIStyleMessage);
		}
	}

	//CreatingAccount GUI
	void CreateAccountGUI(){
		GUI.Box (new Rect (Screen.width/8, Screen.height/16-10, (Screen.width/4)+300, (Screen.height/4)+450), "Create Account", mGUIStyleBox);
		GUI.Label (new Rect (Screen.width/5, Screen.height/6-40, 200, 23), "User Name:", mGUIStyleText);
		cUserName = GUI.TextField (new Rect (Screen.width/4, Screen.height/6, 300, 50), cUserName, mGUIStyleInput);
		GUI.Label (new Rect (Screen.width/5, Screen.height/6+60, 200, 23), "Confirm User Name:", mGUIStyleText);
		confirmUserName = GUI.TextField (new Rect (Screen.width/4, Screen.height/6+100, 300, 50), confirmUserName, mGUIStyleInput);
		GUI.Label (new Rect (Screen.width/5, Screen.height/6+160, 200, 23), "Password:", mGUIStyleText);
		cPassword = GUI.PasswordField (new Rect (Screen.width/4, Screen.height/6+200, 300, 50), cPassword,maskChar, mGUIStyleInput);
		GUI.Label (new Rect (Screen.width/5, Screen.height/6+260, 200, 23), "Confirm Password:", mGUIStyleText);
		confirmPass = GUI.PasswordField (new Rect (Screen.width/4, Screen.height/6+300, 300, 50), confirmPass ,maskChar, mGUIStyleInput);

		if (GUI.Button (new Rect (Screen.width/4+150,Screen.height/6+400,140,70), "Submit", mButtonStyle)) {
			///the user is only able to create an account if the connection is good to the DB and internet
			if (confirmPass == cPassword && confirmUserName == cUserName) {
				CoRo.CreateAccount (cUserName, cPassword);
				//need to clear the the text feilds but save the values for sending to PHP
				//confirmPass = "";
			} else if (confirmPass != cPassword) {
				outPutString = "Mis-Match on Password";
				showMessage = true;
			} else {
				outPutString =  "Mis-Match on User Name"; 
				showMessage = true;
			}
		}
		if (GUI.Button (new Rect (Screen.width/4+15, Screen.height/6+400, 100 ,70), "Back", mButtonStyle)) {
			showMessage = false;
			showMessageGood = false;
			currentMenu = "Login"; 
		}
		if (showMessage) {
			mGUIStyleMessage.normal.textColor = Color.red;
			GUI.Label (new Rect (Screen.width/8+5, Screen.height/6+360, 200, 23), "[ello]:" + outPutString, mGUIStyleMessage);
		}
	}
	#endregion
	/// <summary>
	/// Loads the player selection scene.
	/// this is used when there is no internet or connection to the DB is bad
	/// </summary>
	public void LoadPlayerSelectionScene(){
		SceneManager.LoadScene ("PlayerSelection");
	}
}
///finito

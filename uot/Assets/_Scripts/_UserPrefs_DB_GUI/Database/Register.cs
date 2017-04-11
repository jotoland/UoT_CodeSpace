using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text.RegularExpressions;
using UnityStandardAssets.CrossPlatformInput;
/* John G. Toland 4/10/17 Regsiter Script
 * Handles registration to the mySQL database
 * Communicates wtih CoRoutines script.
 * */
public class Register : MonoBehaviour {
	private int connection;
	private CoRoutines CoRo;
	public Text ERm;
	public GameObject username;
	public GameObject cUsername;
	public GameObject password;
	public GameObject cPassword;
	public GameObject Login;

	private string USERNAME;
	private string C_USERNAME;
	private string PASSWORD;
	private string C_PASSWORD;
	private string[] specialChars = { "!", "@", "#", "$", "%", "&" };
	private string outPutString;

	// Use this for initialization
	void Start () {
		gameObject.SetActive (false);
		//make sure the audio is not paused
		AudioListener.pause = false;
		GameObject CoRoObject = GameObject.FindGameObjectWithTag ("CoRoutines");
		//make sure we can access our instance of CoRo
		if (CoRoObject != null) {
			CoRo = CoRoObject.GetComponent <CoRoutines> ();
		}
		ERm.text = "";
	}

	public void SubmitBtn(){
		if (USERNAME != "" && C_USERNAME != "" && PASSWORD != "" && C_PASSWORD != "") {
			if (USERNAME == C_USERNAME && PASSWORD == C_PASSWORD) {
				if (checkPassword (C_PASSWORD) && checkUserName (C_USERNAME)) {
					print ("HEllo WORLD");
					CoRo.CreateAccount (C_USERNAME, C_PASSWORD);
					gameObject.SetActive (false);
					Login.SetActive (true);
				}
			} else if (USERNAME != C_USERNAME) {
				outPutString = "Mis-Match on User Name"; 
				setERm (outPutString, true);
			} else if (PASSWORD != C_PASSWORD) {
				outPutString = "Mis-Match on Password";
				setERm (outPutString, true);
			}
		} else {
			setERm ("Empty Input fields", true);
		}
	}

	public bool checkPassword(String password){
		bool pass = false;
		bool tooShort = false;
		bool tooLong = false;
		if (password.Length > 7 && password.Length < 17) {
			for (int i = 0; i < specialChars.Length; i++) {
				if (password.Contains (specialChars [i])) {
					pass = true;
					break;
				}
			}
		} else if (!(password.Length > 7)) {
			tooShort = true;
		} else {
			tooLong = true;
		}
		if (!pass && (tooShort || tooLong)) {
			setERm ("Password must be at least 8 characters and no more than 16.", true);
		} else if (!pass && !tooShort) {
			setERm ("Password must contain at least one: \n \"!\", \"@\", \"#\", \"$\", \"%\", \"&\"", true);
		}
		return pass;
	}

	public bool checkUserName(String username){
		bool pass = false;
		if (username.Length > 5 && username.Length < 17) {
			pass = true;
		} else {
			setERm ("Username must be at least 6 characters and no more than 16", true);
		}
		return pass;
	}

	public void BackBtn(){
		gameObject.SetActive (false);
		Login.SetActive (true);
	}

	// Update is called once per frame
	void Update () {
		//USER CROSS PLATFORM INPUT MANAGER HERE
		if (Input.GetKeyDown (KeyCode.Tab)) {
			if (username.GetComponent<InputField> ().isFocused) {
				cUsername.GetComponent<InputField> ().Select ();
			}
			if (cUsername.GetComponent<InputField> ().isFocused) {
				password.GetComponent<InputField> ().Select ();
			}
			if (password.GetComponent<InputField> ().isFocused) {
				cPassword.GetComponent<InputField> ().Select ();
			}
		}
		USERNAME = username.GetComponent<InputField> ().text;
		C_USERNAME = cUsername.GetComponent<InputField> ().text;
		PASSWORD = password.GetComponent<InputField> ().text;
		C_PASSWORD = cPassword.GetComponent<InputField> ().text;

		//USER CROSS PLATFORM INPUT MANAGER HERE
		if (CrossPlatformInputManager.GetButton ("Submit")) {
			if(PASSWORD != "" && C_PASSWORD != "" 
				&& USERNAME != "" && C_USERNAME != ""){
				SubmitBtn ();
			}
		}
	}

	public void setERm(string message, bool bad){
		if (!bad) {
			//do something to set the color to green
			ERm.color = Color.green;
		} else {
			ERm.color = Color.red;
		}
		ERm.text = message;
	}

	public String getUserName(){
		return USERNAME;
	}
		
}
//finito
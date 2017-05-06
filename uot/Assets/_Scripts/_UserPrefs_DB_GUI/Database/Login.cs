using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;
/* John G. Toland 4/10/17 Login GUI Handler Script
 * Communicates with CoRoutines and SceneLoaderHandler
 * */
public class Login : MonoBehaviour {
	private int connection;
	private CoRoutines CoRo;
	private SceneLoaderHandler SLH;
	public Text ELm;
	public GameObject username;
	public GameObject Register;
	public GameObject CreateAccountBtn;
	public GameObject passwordInputField;
	public Text muteAudioBtn;
	private bool AUDIO_MUTE = false;

	private string USERNAME;
	private string PASSWORD;
	private string outPutString;

	// Use this for initialization
	void Start () {
		ELm.text = "";
		//make sure the audio is not paused
		AudioListener.pause = false;
		GameObject CoRoObject = GameObject.FindGameObjectWithTag ("CoRoutines");
		if (CoRoObject != null) {
			CoRo = CoRoObject.GetComponent <CoRoutines> ();
		}
		CoRo.CheckConnection ();

		connection = PlayerPrefs.GetInt ("mConnection");
		print ("connection = " + connection);

		if (connection == 0) {
			CreateAccountBtn.SetActive (false);
			passwordInputField.SetActive (false);
		}

		GameObject SLHo = GameObject.Find ("JOHNS_GUIbox");
		SLH = SLHo.GetComponent<SceneLoaderHandler> ();
	}

	public void LoginBtn(){
		switch (connection){
		//connection is bad or none at all, us PlayerPrefs (!!!!!Does not Save state!!!!!!!)
		case 0:
			if (USERNAME != "") {
				PlayerPrefs.SetString ("mUserName", USERNAME);
				LoadPlayerSelectionScene ();
			}
			break;
		//connection is good, us DB
		case 1:
			if (USERNAME != "" && PASSWORD != "") {
				PlayerPrefs.SetString ("mUserName", USERNAME);
				CoRo.LoginAccount (USERNAME, PASSWORD);
			} else if (USERNAME == "") {
				setELm ("Empty username", true);
			} else {
				setELm ("Empty password", false);
			}
			break;
		}
	}

	public void ExitBtn(){
		Application.Quit ();
		StopEditorPlayback ();
	}

	private void StopEditorPlayback(){
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#endif
	}

	public void RegisterBtn(){
		gameObject.SetActive (false);
		Register.SetActive (true);
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Tab)) {
			if(username.GetComponent <InputField>().isFocused){
				passwordInputField.GetComponent<InputField>().Select ();
			}
		}
		if (CrossPlatformInputManager.GetButton ("Submit")) {
			if(PASSWORD != "" && USERNAME != "" ){
				LoginBtn ();
			}
		}
		USERNAME = username.GetComponent<InputField> ().text.Trim ();
		PASSWORD = passwordInputField.GetComponent<InputField> ().text.Trim ();
		if (connection == 0) {
			setELm ("Offline play Initializing...", false);
		}
	}

	public void setELm(string message, bool bad){
		if (!bad) {
			//do something to set the color to green
			ELm.color = Color.green;
		} else {
			ELm.color = Color.red;
		}
		ELm.text = message;
	}

	public void LoadPlayerSelectionScene(){
		SLH.LoadNewSceneString ("PlayerSelection");
	}

	public String getUserName(){
		return USERNAME;
	}

	public void muteAudio(){
		if (AUDIO_MUTE) {
			AudioListener.pause = false;
			muteAudioBtn.GetComponent<Text> ().text = "Mute Audio";
			AUDIO_MUTE = false;
		} else {
			AUDIO_MUTE = true;
			AudioListener.pause = true;
			muteAudioBtn.GetComponent<Text> ().text = "UnMute";
		}
	}
}
//finito

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/* John G. Toland 4/10/17 Level Selection Handler
 * This script toggles the level image arrays or level video arrays
 * The images are used for mobile platforms
 * The videos are for other platforms
 * Movie Textures are not supported in mobile platforms yet.
 * */
public class LevelSelectionHandler : MonoBehaviour {
	
	private CoRoutines CoRo;
	private SceneLoaderHandler SLH;
	private GameObject[] LevelTextList;

	private string userName;
	private int connection;
	private int levelIndex = 0;

#region ANDROID FUCNTIONS
	#if UNITY_ANDROID || UNITY_WEBGL
	private GameObject[] LevelSelectionListMobile;
	// START FUNCTION FOR ANDROID

	// Use this for initialization
	void Start () {
	#if UNITY_ANDROID
		GameObject SLHo = GameObject.Find ("LevelSelectionCanvas");
		SLH = SLHo.GetComponent<SceneLoaderHandler> ();
	#elif UNITY_WEBGL
		GameObject SLHo = GameObject.Find ("LevelSelectionCanvas_STANDALONE");
		SLH = SLHo.GetComponent<SceneLoaderHandler> ();
	#endif

		GameObject CoRoObject = GameObject.FindGameObjectWithTag ("CoRoutines");
		//checking that we can acces our instance
		if (CoRoObject != null) {
			CoRo = CoRoObject.GetComponent <CoRoutines> ();
		}
		GameObject StandAloneList = GameObject.Find ("StandAloneList");
		StandAloneList.SetActive (false);

		//getting the user name from player prefs instead of DB this time
		userName = PlayerPrefs.GetString ("mUserName");
		connection = PlayerPrefs.GetInt ("mConnection");

		GameObject TextList = GameObject.Find ("LevelText");
		GameObject ImageList = GameObject.Find ("LevelSelectionListMobile");

		LevelTextList = new GameObject[TextList.transform.childCount];

		for(int i =0; i<TextList.transform.childCount; i++){
			//print ("i = " + i);
			LevelTextList [i] = TextList.transform.GetChild (i).gameObject;
		}

		//We toggle off their renderers for the start
		foreach (GameObject go in LevelTextList) {
			go.SetActive (false);
		}
			
		LevelSelectionListMobile = new GameObject[ImageList.transform.childCount];

		for(int i =0; i<ImageList.transform.childCount; i++){
			//print (ImageList.transform.GetChild (i).gameObject.name);
			LevelSelectionListMobile [i] = ImageList.transform.GetChild (i).gameObject;
		}

		//We toggle off their renderers for the start
		foreach (GameObject go in LevelSelectionListMobile) {
			go.SetActive (false);
		}
		//print ("levelIndex = " + levelIndex);
		LevelSelectionListMobile [levelIndex].SetActive (true);
		LevelTextList [levelIndex].SetActive (true);
	}
		
	//when user is toggling left
	public void ToggleLeft(){
		//Toggle off current model
		LevelSelectionListMobile[levelIndex].SetActive(false);
		LevelTextList[levelIndex].SetActive(false);
		levelIndex--;
		if (levelIndex < 0) 
			levelIndex = LevelSelectionListMobile.Length - 1;
		LevelSelectionListMobile[levelIndex].SetActive(true);
		LevelTextList[levelIndex].SetActive(true);
	}
	//when user is toggling right
	public void ToggleRight(){
		//Toggle off current model
		LevelSelectionListMobile[levelIndex].SetActive(false);
		LevelTextList[levelIndex].SetActive(false);
		levelIndex++;
		if (levelIndex == LevelSelectionListMobile.Length) 
			levelIndex = 0;
		LevelSelectionListMobile[levelIndex].SetActive(true);
		LevelTextList[levelIndex].SetActive(true);
	}
	//confirmation button
	public void ConfirmButton(){
		//update database with seleciton
		if (connection == 1) {
			CoRo.UpdateData (userName, levelIndex+1, "lvl");
			SLH.LoadNewSceneInt (levelIndex+3);
		} else {
			//SceneManager.LoadScene (levelIndex+3);
			SLH.LoadNewSceneInt (levelIndex+3);
		}
		PlayerPrefs.SetInt ("mLevel", levelIndex+1);  
	}
	#endif
#endregion

#region STANDALONE FUCNTIONS
	#if !UNITY_ANDROID && !UNITY_WEBGL
	private GameObject[] LevelSelectionList;
	// START FUNCTION FOR ANDROID

	// Use this for initialization
	void Start () {
		GameObject SLHo = GameObject.Find ("LevelSelectionCanvas_STANDALONE");
		SLH = SLHo.GetComponent<SceneLoaderHandler> ();

		GameObject CoRoObject = GameObject.FindGameObjectWithTag ("CoRoutines");
		//checking that we can acces our instance
		if (CoRoObject != null) {
			CoRo = CoRoObject.GetComponent <CoRoutines> ();
		}

		GameObject MobileList = GameObject.Find ("LevelSelectionListMobile");
		MobileList.SetActive (false);

		GameObject TextList = GameObject.Find ("LevelText");

		GameObject VideoList = GameObject.Find ("StandAloneList");



		//getting the user name from player prefs instead of DB this time
		userName = PlayerPrefs.GetString ("mUserName");
		connection = PlayerPrefs.GetInt ("mConnection");



		LevelTextList = new GameObject[TextList.transform.childCount];

		for(int i =0; i<TextList.transform.childCount; i++){
			print ("i = " + i);
			LevelTextList [i] = TextList.transform.GetChild (i).gameObject;
		}

		//We toggle off their renderers for the start
		foreach (GameObject go in LevelTextList) {
			go.SetActive (false);
		}


		LevelSelectionList = new GameObject[VideoList.transform.childCount];

		for(int i =0; i<VideoList.transform.childCount; i++){
			print (VideoList.transform.GetChild (i).gameObject.name);
			LevelSelectionList [i] = VideoList.transform.GetChild (i).gameObject;
		}

		//We toggle off their renderers for the start
		foreach (GameObject go in LevelSelectionList) {
			go.SetActive (false);
		}
		print ("levelIndex = " + levelIndex);
		LevelSelectionList [levelIndex].SetActive (true);
		LevelTextList [levelIndex].SetActive (true);
	}

	//when user is toggling left
	public void ToggleLeft(){
		//Toggle off current model
		LevelSelectionList[levelIndex].SetActive(false);
		LevelTextList[levelIndex].SetActive(false);
		levelIndex--;
		if (levelIndex < 0) 
			levelIndex = LevelSelectionList.Length - 1;
		LevelSelectionList[levelIndex].SetActive(true);
		LevelTextList[levelIndex].SetActive(true);
	}
	//when user is toggling right
	public void ToggleRight(){
		//Toggle off current model
		LevelSelectionList[levelIndex].SetActive(false);
		LevelTextList[levelIndex].SetActive(false);
		levelIndex++;
		if (levelIndex == LevelSelectionList.Length) 
			levelIndex = 0;
		LevelSelectionList[levelIndex].SetActive(true);
		LevelTextList[levelIndex].SetActive(true);
	}
	//confirmation button
	public void ConfirmButton(){
		//update database with seleciton
		if (connection == 1) {
			CoRo.UpdateData (userName, levelIndex+1, "lvl");
			SLH.LoadNewSceneInt (levelIndex+3);
		} else {
			//SceneManager.LoadScene (levelIndex+3);
			SLH.LoadNewSceneInt (levelIndex+3);
		}
		PlayerPrefs.SetInt ("mLevel", levelIndex+1);  
	}
	#endif
	#endregion

	public void ExitBtn(){
		Application.Quit ();
		StopEditorPlayback ();
	}

	void StopEditorPlayback(){
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#endif
	}
}
//finito

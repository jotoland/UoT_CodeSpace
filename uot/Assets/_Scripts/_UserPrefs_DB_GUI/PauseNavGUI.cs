using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
/* John G. Toland 4/9/17
 * Naviagion Menu Handler during gameplay menu
 * */
public class PauseNavGUI: MonoBehaviour {
	public GameObject exitBtn;
	public GameObject ShipSelBtn;
	public GameObject resumeBtn;
	public GameObject muteAudio;
	public GameObject pauseBtn;
	public GameObject NavMenu;
	public GameObject AudioMenu;
	public GameObject highScoreBoard;
	public bool PAUSE_BTN_ENABLED;
	private bool GAME_IS_PAUSED;
	public bool AUDIO_MUTED;
	private GameController gc;
	private SceneLoaderHandler SLH;
	private CoRoutines CoRo;
	private bool LOOKING_4GAMEOVER;
	private string username;
	private bool LEFT_SCENE;
	private GameObject Menu;
	public GameObject scoreList;
	public ScoreManager man;

	// Use this for initialization
	void Start () {
		LEFT_SCENE = false;
		LOOKING_4GAMEOVER = true;
		AUDIO_MUTED = false;
		GAME_IS_PAUSED = false;
		PAUSE_BTN_ENABLED = false;
		NavMenu.SetActive (false);
		AudioMenu.SetActive (false);
		highScoreBoard.SetActive (false);

		StartCoroutine (ActivatePauseBtn ());

		username = PlayerPrefs.GetString ("mUserName");

		GameObject CoRoObject = GameObject.FindGameObjectWithTag ("CoRoutines");
		if (CoRoObject != null) {
			CoRo = CoRoObject.GetComponent <CoRoutines> ();
		}

		GameObject gcObject = GameObject.FindGameObjectWithTag ("GameController");
		if (gcObject != null) {
			gc = gcObject.GetComponent <GameController> ();
		}

		GameObject SLHo = GameObject.Find ("JOHNS_NAV_GUI_MOBILE");
		SLH = SLHo.GetComponent<SceneLoaderHandler> ();

		this.gameObject.GetComponent<CanvasRenderer> ().SetAlpha (0.0f);
		this.gameObject.GetComponentInChildren<Text> ().text = "";
	}
	
	// Update is called once per frame
	void Update () {
		if (gc.isGameOver () && LOOKING_4GAMEOVER) {
			LOOKING_4GAMEOVER = false;
			pauseBtn.SetActive (false);
		}
	}
		
	public void ResumeBtn(){
		GAME_IS_PAUSED = false;
		NavMenu.SetActive (false);
		AudioMenu.SetActive (false);
		Time.timeScale = 1;
		AudioListener.pause = false;
	}

	public void ScoreBoardResumeBtn(){
		while (scoreList.transform.childCount > 0) {
			GameObject c = scoreList.transform.GetChild (0).gameObject;
			Transform t = c.transform;
			t.SetParent (null);
			Destroy (c);
		}
		man.ClearChangeCounter ();
		highScoreBoard.SetActive (false);
		GAME_IS_PAUSED = false;
		Time.timeScale = 1;
		AudioListener.pause = false;
	}

	public void ExitBtn(){
		Application.Quit ();
		StopEditorPlayback ();
	}

	void StopEditorPlayback(){
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#endif
	}

	public void MuteAudio(){
		if (AUDIO_MUTED) {
			AudioListener.volume = 1;
			muteAudio.GetComponentInChildren<Text>().text = "Mute Audio";
			AUDIO_MUTED = false;
		} else {
			AudioListener.volume = 0;
			muteAudio.GetComponentInChildren<Text>().text = "Unmute";
			AUDIO_MUTED = true;
		}
	}

	public void AudioMenuBtn(){
		NavMenu.SetActive (false);
		AudioMenu.SetActive (true);
	}

	public void ScoreBoardBtn(){
		NavMenu.SetActive (false);
		highScoreBoard.SetActive (true);
	}

	public void ScoreBoardBackButton(){
		while (scoreList.transform.childCount > 0) {
			GameObject c = scoreList.transform.GetChild (0).gameObject;
			Transform t = c.transform;
			t.SetParent (null);
			Destroy (c);
		}
		man.ClearChangeCounter ();
		highScoreBoard.SetActive (false);
		NavMenu.SetActive (true);

	}

	public void BackBtn(){
		AudioMenu.SetActive (false);
		NavMenu.SetActive (true);
	}

	public void NavPause(){
		if (!GAME_IS_PAUSED) {
			if (PAUSE_BTN_ENABLED) {
				GAME_IS_PAUSED = true;
				NavMenu.SetActive (true);
				Time.timeScale = 0;
				AudioListener.pause = true;
			}
		}
	}

	public void ShipSelectionBtn(){
		CoRo.UpdateData (username, 1, "lvl");
		DestroyClones();
		LEFT_SCENE = true;
		Time.timeScale = 1;
		GAME_IS_PAUSED = false;
		NavMenu.SetActive (false);
		AudioListener.pause = false;
		SLH.LoadNewSceneInt (1);

	}

	public void LevelSelectionBtn(){
		CoRo.UpdateData (username, 9, "lvl");
		DestroyClones ();
		LEFT_SCENE = true;
		Time.timeScale = 1;
		GAME_IS_PAUSED = false;
		NavMenu.SetActive (false);
		AudioListener.pause = false;
		SLH.LoadNewSceneInt (2);
	}

	public bool GameIsPaused(){
		return GAME_IS_PAUSED;
	}

	IEnumerator ActivatePauseBtn(){
		yield return new WaitForSecondsRealtime (2);
		this.gameObject.GetComponent<CanvasRenderer> ().SetAlpha (1.0f);
		this.gameObject.GetComponentInChildren<Text> ().text = "Pause";
		PAUSE_BTN_ENABLED = true;
	}

	public bool isLEFT_SCENE(){
		return LEFT_SCENE;
	}

	public void setLEFT_SCENE(bool leftScene){
		LEFT_SCENE = leftScene;
	}

	private void DestroyClones(){
		GameObject player = GameObject.FindGameObjectWithTag ("Player");
		player.GetComponent<Collider> ().enabled = false;
		GameObject[] clones = GameObject.FindGameObjectsWithTag ("Enemy");
		foreach (GameObject go in clones) {
			Destroy (go);
		}
	}
		
}
//finito
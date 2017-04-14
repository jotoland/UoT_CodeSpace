using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
/// <summary>
/// Game controller.
/// 
/// /// 02/17/17 John G. Toland
/// /// 02/18/17 Dylan Salopek
/// /// 02/20/17 Richard O'Neal
/// Controls the game progress shared amoungst all levels.
/// Starts the initial spawning off all levels.
/// The actual methods t of wave spawning are in the Levels script.
/// The gamecontroller only starts your level using an instance of the Levels Class.
/// /// John G. Toland 4/10/17 updated the respawn method for all levels
/// and also added a toggle collider that toggles the render so the player knows he has respawned.
/// </summary>
public class GameController : MonoBehaviour {
	//used for unit testing
	private bool Test = false;

	private SceneLoaderHandler SLH;
	private PauseNavGUI pB;
	public GameObject restartButton;
	private int connection;
	public Scene currentScene;
	private CoRoutines CoRo;
	private PlayerController pc;
	private Levels lvl;
	private LevelScript_01 lvl_01;
	private Levels05 lvl_05;
	public GameObject[] shipList;
	public GameObject[] rupeeBox;
	public Transform spawnPlayer;
	private int loadLevelWait;
	private int levelCount;
	private bool playerDied;
	private int currentShip;

	//GUI HUD text variables
	public GUIText scoreText;	//score text 
	//public GUIText restartText;	//restart text
	public GUIText gameOverText;//game over text
	public GUIText userNameText;
	public GUIText livesText;
	public GUIText rupeeText;
	public GUIText missileText;//game over text
	public GUIText wingDestrCntText;

	//Game Progress variables
	private string userName;
	private bool gameOver;		//game over flag
	//private bool restart;		//restart flag
	private int score;			//score value 	/made public for testing only J.T.	
	private int missileCount;
	private int lives;
	private int rupees;
	private int wingDestrCnt;

	//Variables used to update the DB.
	private int rupeeUpdateInterval;
	private int scoreUpdateInterval;

	void Start(){
		GameObject SLHo = GameObject.Find ("JOHNS_NAV_GUI_MOBILE");
		SLH = SLHo.GetComponent<SceneLoaderHandler> ();
		//Making sure the audio is unpaused.
		AudioListener.pause = false;

		//Getting the reference to the CoRoutines object.
		GameObject CoRoObject = GameObject.FindGameObjectWithTag ("CoRoutines");
		if (CoRoObject != null) {
			CoRo = CoRoObject.GetComponent <CoRoutines> ();
		}
		GameObject lvlObject = GameObject.FindGameObjectWithTag ("GameController");
		if (lvlObject != null) {
			//print ("level scirpt assigned");
			lvl = lvlObject.GetComponent <Levels> ();
		}
		GameObject lvl_01Object = GameObject.FindGameObjectWithTag ("GameController");
		if (lvl_01Object != null) {
			lvl_01 = lvl_01Object.GetComponent <LevelScript_01> ();
		}

		GameObject pBObject = GameObject.FindGameObjectWithTag ("PauseBtn");
		if (pBObject != null) {
			pB = pBObject.GetComponent <PauseNavGUI> ();
		}

		GameObject lvl_05Object = GameObject.FindGameObjectWithTag ("GameController");
		if (lvl_05Object != null) {
			//print ("level scirpt assigned");
			lvl_05 = lvl_05Object.GetComponent <Levels05> ();
		}


		connection = PlayerPrefs.GetInt ("mConnection");
		currentShip = PlayerPrefs.GetInt ("mShip");

		if (connection == 1) {
			//This coroutine is local to the GameController class. 
			//This needs to wait one second while data is fetched from the DB,
			//then the GetData Coroutine gets the items from the items array and places them respectively.
			userNameText.text = "";
			StartCoroutine ("GetData");
		} else {
			//Getting the currently loaded scene using the SceneManager.
			userName = PlayerPrefs.GetString ("mUserName");
			userNameText.text = userName;
			levelCount = levelCount + 2;
		}

		//Initialization of variables.
		loadLevelWait = 5;
		missileCount = 0;
		scoreUpdateInterval = 0;
		lives = 1;
		score = 0;				
		rupees = 0;
		wingDestrCnt = 0;
		playerDied = false;
		gameOver = false;		
		//restart = false;
		//restartText.text = "";	
		gameOverText.text = "";
		missileText.text = "";
		livesText.text = "";
		rupeeText.text = "";
		scoreText.text = "";
		restartButton.SetActive(false);

		//Getting the currently loaded scene using the SceneManager.
		currentScene = SceneManager.GetActiveScene();

		///Check the name of the currently loaded scene.
		if (currentScene.name == "Level_01") {
			//Begin Hazard spawn level_01.
			lvl_01.StartLvlOne ();
		} else if (currentScene.name == "Level_02") {
			//Begin Hazard spawn level_02.
			lvl.StartGenericLvl ();
			//StartCoroutine (SpawnWavesLevel_02 ());
		} else if (currentScene.name == "Level_03") {
			//Begin Hazard spawn level_03
			lvl.StartGenericLvl ();
			//start your CoRoutine
		} else if (currentScene.name == "Level_04") {
			//Begin Hazard spawn level_04
			lvl.StartGenericLvl ();
			//start your CoRoutine
		}else if(currentScene.name == "Level_05"){
			//lvl.StartGenericLvl ();
			lvl_05.StartLvlFive ();
			//Begin Hazard spawn level_05
			//Start  your CoRoutine
		}
	}

	/// getting the users score from items array no DB interaction but still waits for 1 second for DB interaction with CoRo
	IEnumerator GetData(){
		yield return new WaitForSeconds(2);
		userName = PlayerPrefs.GetString ("mUsername");
		levelCount = PlayerPrefs.GetInt ("mLevel");
		score = int.Parse(PlayerPrefs.GetString ("mPoints"));
		rupees = int.Parse(PlayerPrefs.GetString ("mRupees"));
		lives = int.Parse(PlayerPrefs.GetString ("mLives"));

		//This is a saftey net just incase the DB recieves a lives count below 1.
		//This bug is very interesting it does not occur everytime and only occurs on a very rare case.
		if (lives < 1) {
			lives = 1;
		}

		UpdateScore ();
		UpdateRupees ();
		UpdateLife ();
		UpdateWingDestrCnt ();
		userNameText.text = userName;
	}
/*
	void Update () {
		if (restart) {								//if flag is true
			if(Input.GetKeyDown (KeyCode.R)) {		//if keypress is 'r'
				SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);	//reload scene
			}
		}
	}
*/
	public void RestartGame(){
		if (pB.GameIsPaused()) {
			pB.ResumeBtn ();
		}
		SLH.LoadNewSceneInt (currentScene.buildIndex);
		Time.timeScale = 1;
	}

	public void ReSpawn(){
		print("currentship = " + currentShip);
		print ("currentScene = " + currentScene.name);
		//just for level_01!!!
		if (currentScene.name == "Level_01" || currentScene.name == "Level_04") {
			print ("respawing player");
			Instantiate (shipList [shipList.Length - 1], spawnPlayer.position, spawnPlayer.rotation);
			Instantiate (shipList [currentShip], spawnPlayer.position, spawnPlayer.rotation);
			//any other level respawn player!!!
		} else {
			Instantiate (shipList [currentShip], spawnPlayer.position, spawnPlayer.rotation);
		}
		GameObject pcObject = GameObject.FindGameObjectWithTag ("Player");
		if (pcObject != null) {
			pc = pcObject.GetComponent <PlayerController> ();
			pc.startToggleCollider ();
		} else {
			print ("cannot find player");
		}
	}

	public void spawnRupee(Vector3 position, Quaternion rotation){
		Instantiate (rupeeBox[Random.Range (0, rupeeBox.Length)], position, rotation);
	}

	public void setGameOverText(bool show){
		if (show) {
			print ("[GC from DB]: level count = " + levelCount);
			gameOverText.text = "   Level " + (levelCount);
		} else {
			gameOverText.text = "";
		}
	}
	#region USED FOR UNIT TESTS
	public int getRupeeCount(){
		return rupees;
	}

	public int getScore(){
		return score;
	}

	public int getLives(){
		return lives;
	}

	public void clearValues(){
		rupees = 0;
		score = 0;
		lives = 0;
		scoreUpdateInterval = 0;
		rupeeUpdateInterval = 0;
	}

	public void setTest(){
		Test = true;
	}
	#endregion
	public bool isGameOver(){
		return gameOver;
	}

	public bool isPlayerDead(){
		return playerDied;
	}

	public void setRestart(bool set){
		restartButton.SetActive (true);
		//restartText.text = "Press 'R' for Restart";
		//restart = set;
	}

	public void setPlayerDead(bool set){
		playerDied = set;
	}

	public int getLvlCount(){
		return levelCount;
	}

	public void resetLvlCount(){
		levelCount = 0;
	}

	public int getLoadLvlWait(){
		return loadLevelWait;
	}

	public int getMissleCount(){
		return missileCount;
	}

	public int getLivesCount(){
		return lives;
	}
		
	//function to update score, taking a score value as an argument
	public void AddScore (int newScoreValue) {
		//updating local score
		score += newScoreValue;
		//updating local interval variable
		scoreUpdateInterval += newScoreValue;
		//if interval reached the start coroutine to update the DB
		if (scoreUpdateInterval > 50) {
			scoreUpdateInterval = 0;
			CoRo.UpdateData (userName, score, "pts");
		}
		UpdateScore ();

	}

	public void AddRupees(int newRupeeValue){
		rupees += newRupeeValue;
		rupeeUpdateInterval += newRupeeValue;
		if (rupeeUpdateInterval > 10) {
			rupeeUpdateInterval = 0;
			CoRo.UpdateData (userName, rupees, "rup");
		}
		UpdateRupees ();
	}

	public void AddLife(int newLifeValue){
		lives += newLifeValue;
		if (!Test) {
			CoRo.UpdateData (userName, lives, "liv");
		}
		UpdateLife ();
	}

	//function to update missile count 
	public void AddMissiles(int newMissileCount) {
		missileCount += newMissileCount;	//increment score
		UpdateMissileCount ();			//call to update score string
	}

	public void AddWingDestrCnt(int newDestrCnt) {
		wingDestrCnt += newDestrCnt;
		if (wingDestrCntText) {
			UpdateWingDestrCnt ();
		}
	}
	
	//updates the GUI rupee text
	public void UpdateRupees(){
		rupeeText.text = "Rupees: " + rupees;
	}

	//updates the GUI objects destroyed by wingmen text
	public void UpdateWingDestrCnt() {
		if (wingDestrCntText) {
			wingDestrCntText.text = "Wingmen Kills: " + wingDestrCnt;
		}
	}

	//updates the GUI lives text
	public void UpdateLife(){
		livesText.text = "Lives: " + lives;
	}

	//function for score string
	void UpdateScore () {
		scoreText.text = "Score: " + score;		
	}
	//function to show the player the amount of missiles
	void UpdateMissileCount(){
		missileText.text = "Missiles: " + missileCount;
	}

	//function for game over
	public void GameOver () {
		gameOverText.text = "Game Over!";	//set text when called
		gameOver = true;					//set flag to true
		score = 0;
		UpdateScore ();
		CoRo.UpdateData (userName, score, "pts");
		CoRo.UpdateData (userName, rupees, "rup");
		lives = 1;
		CoRo.UpdateData (userName, lives, "liv");
	}

	//level completion preperation for next level or application exit
	public void levelCompleted(){
		gameOverText.text = "\tVictory!";
		print ("level Count = " + levelCount);
		if (levelCount == 5) {
			levelCount = -1;
		}
		CoRo.UpdateData (userName, levelCount+1, "lvl");
		//print ("level Count = " + levelCount);
		CoRo.UpdateData (userName, lives, "liv");
		CoRo.UpdateData (userName, score, "pts");
		CoRo.UpdateData (userName, rupees, "rup");
	}

}
//finito

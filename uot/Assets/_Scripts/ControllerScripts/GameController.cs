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
/// Controls the Hazard spawing in the game
/// </summary>
public class GameController : MonoBehaviour {

	private CoRoutines CoRo;
	public GameObject[] rupeeBox;
	public GameObject[] shipList;
	public Transform spawnPlayer;
	//level variables
	private int spawnWaveCount;
	private int loadLevelWait;
	private int levelCount;
	public int numOfWavesInLvl;
	private bool beginBossWaveLvl_01;

	//reference to our hazard
	public GameObject[] hazards;
	///postion to spawn waves
	public Vector3 spawnValues;
	//count for loop
	public int hazardCount;
	//holds wait time between each spawn
	public float spawnWait;
	//time before game starts
	public float startWait;
	//time before each wave
	public float waveWait;
	public bool playerDied;

	//GUI HUD text variables
	public GUIText scoreText;	//score text 
	public GUIText restartText;	//restart text
	public GUIText gameOverText;//game over text
	public GUIText userNameText;
	public GUIText livesText;
	public GUIText rupeeText;
	public GUIText missileText;//game over text

	//Game Progress variables
	public string userName;
	private bool gameOver;		//game over flag
	private bool restart;		//restart flag
	public int score;			//score value 	/made public for testing only J.T.	
	public int missileCount;
	public int lives;
	public int rupee;
	public int rupees;

	//Variables used to update the DB.
	private int rupeeUpdateInterval;
	private int scoreUpdateInterval;

	void Start(){
		//Making sure the audio is unpaused.
		AudioListener.pause = false;

		//Getting the reference to the CoRoutines object.
		GameObject CoRoObject = GameObject.FindGameObjectWithTag ("CoRoutines");
		if (CoRoObject != null) {
			CoRo = CoRoObject.GetComponent <CoRoutines> ();
		}

		//Initialization of variables.
		spawnWaveCount = 0;
		loadLevelWait = 5;
		missileCount = 0;
		scoreUpdateInterval = 0;
		lives = 1;
		score = 0;				
		rupees = 0;
		playerDied = false;
		gameOver = false;		
		restart = false;
		beginBossWaveLvl_01 = false;
		restartText.text = "";	
		gameOverText.text = "";
		userNameText.text = "";

		//Updating GUI for the foundational template useually all zeros but lifes is always 1.
		UpdateScore ();	
		UpdateLife ();
		UpdateRupees();


		//Getting the currently loaded scene using the SceneManager.
		Scene currentScene = SceneManager.GetActiveScene();

		//This coroutine is local to the GameController class. 
		//This needs to wait one second while data is fetched from the DB,
		//then the GetData Coroutine gets the items from the items array and places them respectively.
		StartCoroutine ("GetData");

		///Check the name of the currently loaded scene.
		if (currentScene.name == "Level_01") {
			//Begin Hazard spawn level_01.
			missileText.text = "";
			StartCoroutine (SpawnWavesLevel_01 ());
		} else if (currentScene.name == "Level_02") {
			//Begin Hazard spawn level_02.
			StartCoroutine (SpawnWaves());
			//StartCoroutine (SpawnWavesLevel_02 ());
		} else if (currentScene.name == "Level_03") {
			//Begin Hazard spawn level_03
			StartCoroutine (SpawnWaves());
			//start your CoRoutine
		} else if (currentScene.name == "Level_04") {
			//Begin Hazard spawn level_04
			StartCoroutine (SpawnWaves());
			//start your CoRoutine
		}else if(currentScene.name == "Level_05"){
			//Begin Hazard spawn level_05
			//Start  your CoRoutine
		}
	}

	/// getting the users score from items array no DB interaction but still waits for 1 second for DB interaction with CoRo
	IEnumerator GetData(){
		yield return new WaitForSeconds(1);
		userName = (CoRo.items[1]);
		levelCount = int.Parse (CoRo.items [2]);
		score = int.Parse (CoRo.items [4]);
		rupees = int.Parse (CoRo.items [5]);
		lives = int.Parse (CoRo.items [6]);
		//This is a saftey net just incase the DB recieves a lives count below 1.
		//This bug is very interesting it does not occur everytime and only occurs on a very rare case.
		if (lives < 1) {
			lives = 1;
		}
		UpdateScore ();
		UpdateRupees ();
		UpdateLife ();
		userNameText.text = userName;
	}

	void Update () {
		if (restart) {								//if flag is true
			if(Input.GetKeyDown (KeyCode.R)) {		//if keypress is 'r'
				SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);	//reload scene
			}
		}
		///spawning the boss wave for level_01
		if (beginBossWaveLvl_01) {
			StartCoroutine (SpawnBossWaveLevel_01 ());
			beginBossWaveLvl_01 = false;
		}
	}

	/// Spawns the waves.
	/// <returns>The enemy waves.</returns>
	IEnumerator SpawnWavesLevel_01 (){
		yield return new WaitForSeconds (1);
		gameOverText.text = "Level " + (levelCount);
		yield return new WaitForSeconds (startWait);
		gameOverText.text = "";

		while (true){
			for (int i = 0; i < hazardCount; i++){
				GameObject hazard = hazards [Random.Range (0, hazards.Length)];
				Vector3 spawnPosition = new Vector3 (Random.Range (-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
				Quaternion spawnRotation = Quaternion.identity;
				Instantiate (hazard, spawnPosition, spawnRotation);
				yield return new WaitForSeconds (spawnWait);
			}

			yield return new WaitForSeconds (waveWait);
			spawnWaveCount++;
			if (gameOver) {
				restartText.text = "Press 'R' for Restart";
				restart = true;
				break;
			} else if (playerDied) {
				spawnWaveCount = 0;
				ReSpawn ();
				playerDied = false;
			} else if(spawnWaveCount == numOfWavesInLvl && !gameOver) {
				beginBossWaveLvl_01 = true;
				break;

			}
		}
	}

	IEnumerator SpawnBossWaveLevel_01(){
		yield return new WaitForSeconds (startWait);
		while (true){
			for (int i = 0; i < 50; i++){
				GameObject hazard = hazards [Random.Range (0, hazards.Length)];
				Vector3 spawnPosition = new Vector3 (Random.Range (-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
				Quaternion spawnRotation = Quaternion.identity;
				Instantiate (hazard, spawnPosition, spawnRotation);
				yield return new WaitForSeconds (0.1f);
			}

			yield return new WaitForSeconds (waveWait);
			spawnWaveCount++;
			if (gameOver) {
				restartText.text = "Press 'R' for Restart";
				restart = true;
				break;
			} else if (playerDied) {
				spawnWaveCount = 0;
				ReSpawn ();
				playerDied = false;
			} else if(spawnWaveCount == numOfWavesInLvl+2 && !gameOver) {
				//beginBossWaveLevel_01 = true;
				//break;
				print (levelCount);
				//levelCompleted ();
				yield return new WaitForSeconds (loadLevelWait);
				print (levelCount);
				SceneManager.LoadScene (levelCount);
			}
		}
	}

		
	//function to spawn waves of hazards
	IEnumerator SpawnWaves(){
		yield return new WaitForSeconds (startWait);
		while(true){
			for(int i =0; i<hazardCount; i++){
				GameObject hazard = hazards [Random.Range (0, hazards.Length)];//Picks random hazard from hazards array
				Vector3 spawnPosition = new Vector3 (Random.Range (-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
				Quaternion spawnRotation = Quaternion.identity;
				Instantiate (hazard, spawnPosition, spawnRotation);
				yield return new WaitForSeconds (spawnWait);
			}
			yield return new WaitForSeconds (waveWait);
			if (gameOver) {
				restartText.text = "Press 'R' to Restart";	//set text when gameOver is true
				restart = true;	//set flag to true
				break;			//exit SpawnWaves coroutine
			}
		}
	}

	void ReSpawn(){
		Instantiate (shipList[int.Parse(CoRo.items[3])], spawnPlayer.position, spawnPlayer.rotation);
	}

	public void spawnRupee(Vector3 position, Quaternion rotation){
		Instantiate (rupeeBox[Random.Range (0, rupeeBox.Length)], position, rotation);
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

		CoRo.UpdateData (userName, lives, "liv");
		UpdateLife ();
	}

	//function to update missile count 
	public void AddMissiles(int newMissileCount) {
		missileCount += newMissileCount;	//increment score
		UpdateMissileCount ();			//call to update score string
	}

	//updates the GUI rupee text
	public void UpdateRupees(){
		rupeeText.text = "Rupees: " + rupees;
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
		gameOverText.text = "Victory!";
		levelCount = levelCount+2;
		//print ("level Count = " + levelCount);
		CoRo.UpdateData (userName, levelCount, "lvl");
		CoRo.UpdateData (userName, lives, "liv");
		CoRo.UpdateData (userName, score, "pts");
		CoRo.UpdateData (userName, rupees, "rup");
	}

}

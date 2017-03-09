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

	public GUIText scoreText;	//score text 
	public GUIText restartText;	//restart text
	public GUIText gameOverText;//game over text
	public GUIText missileText;//game over text

	private bool gameOver;		//game over flag
	private bool restart;		//restart flag
	public int score;			//score value 	/made public for testing only J.T.	
	public int missileCount = 0;

	//Rupee variables
	public int rupees;
	private int rupeeUpdateInterval;
	public GameObject[] rupeeBox;

	void Start(){
		gameOver = false;		//set flag to false
		restart = false;		//set flag to false
		restartText.text = "";	//initialize empty string
		gameOverText.text = ""; //initialize empty string
		score = 0;				//initialize score to 0 when game starts
		UpdateScore ();			//call update score to reflect score on start
		SpawnWaves ();
		StartCoroutine (SpawnWaves());
	}

	void Update () {
		if (restart) {								//if flag is true
			if(Input.GetKeyDown (KeyCode.R)) {		//if keypress is 'r'
				SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);	//reload scene
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

	//function to update score, taking a score value as an argument
	public void AddScore (int newScoreValue) {
		score += newScoreValue;	//increment score
		UpdateScore ();			//call to update score string
	}

	//function to update missile count 
	public void AddMissiles(int newMissileCount) {
		missileCount += newMissileCount;	//increment score
		UpdateMissileCount ();			//call to update score string
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
	}

	public void spawnRupee(Vector3 position, Quaternion rotation){
		Instantiate (rupeeBox[Random.Range (0, rupeeBox.Length)], position, rotation);
	}

	public void AddRupees(int newRupeeValue){
		rupees += newRupeeValue;
		rupeeUpdateInterval += newRupeeValue;
		if (rupeeUpdateInterval > 10) {
			rupeeUpdateInterval = 0;
			//call to update DB goes here
		}
		///call to update HUD goes here
	}


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Levels : MonoBehaviour {
	
	//all level variables
	private GameController gc;
	public GameObject[] hazards;
	public GameObject[] rupeeBox;
	public Vector3 spawnValues;
	public int hazardCount;
	public float spawnWait;
	public float startWait;
	public float waveWait;
	public int numOfWavesInLvl;
	private int spawnWaveCount;

	//level_01 variables
	private bool beginBossWaveLvl_01;

	// Use this for initialization
	void Start () {
		GameObject gcObject = GameObject.FindGameObjectWithTag ("GameController");
		if (gcObject != null) {
			gc = gcObject.GetComponent <GameController> ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		///spawning the boss wave for level_01
		if (beginBossWaveLvl_01) {
			StartCoroutine (SpawnBossWaveLevel_01 ());
			beginBossWaveLvl_01 = false;
		}
	}

#region methodsToStartCoroutines
	public void StartLvlOne(){
		StartCoroutine (SpawnWavesLevel_01 ());
	}

	public void StartGenericLvl(){
		StartCoroutine (SpawnWaves ());
	}
#endregion

	public bool checkPlayerProgressInLvl(bool isRegularWave){
		if (isRegularWave) {
			spawnWaveCount++;
			print ("wave count = " + spawnWaveCount);
			if (gc.isGameOver ()) {
				gc.setRestart (true);
				return false;
			} else if (gc.isPlayerDead ()) {
				spawnWaveCount = 0;
				gc.ReSpawn ();
				gc.setPlayerDead (false);
				return true;
			} else if (spawnWaveCount == numOfWavesInLvl && !gc.isGameOver ()) {
				enteringBossWave (gc.getLvlCount ());
				return false;
			} else {
				return true;
			}
		} else {
			spawnWaveCount++;
			print ("wave count = " + spawnWaveCount);
			if (gc.isGameOver ()) {
				gc.setRestart (true);
				return false;
			} else if (gc.isPlayerDead ()) {
				spawnWaveCount = numOfWavesInLvl;
				gc.ReSpawn ();
				gc.setPlayerDead (false);
				return true;
			} else {
				return true;
			}
		}
	}

	public void enteringBossWave(int lvlCount){
		switch (lvlCount) {
		case 1:
			beginBossWaveLvl_01 = true;
			break;
		case 2:
			//level 2 boss wave case
			//beginBossWaveLvl_01 = true;						///used for testing
			break;
		case 3:
			//level 3 boss wave case
			//beginBossWaveLvl_01 = true;						///used for testing
			break;
		case 4:
			//level 4 boss wave case
			//beginBossWaveLvl_01 = true;						///used for testing
			break;
		case 5:
			//level 5 boss wave case
			//beginBossWaveLvl_01 = true; 						///used for testing
			break;
		default:
			//nothing to do here
			break;
		}
	}

	/// spawns the rupees from the rupee box after a hazard has been destroyed
	public void spawnRupee(Vector3 position, Quaternion rotation){
		Instantiate (rupeeBox[Random.Range (0, rupeeBox.Length)], position, rotation);
	}

#region Level_01 waveSpawning
	/// Spawns the waves.
	/// <returns>The enemy waves.</returns>
	IEnumerator SpawnWavesLevel_01 (){
		yield return new WaitForSeconds (3f);
		gc.setGameOverText (true);
		yield return new WaitForSeconds (startWait);
		gc.setGameOverText (false);
		while (true){
			for (int i = 0; i < hazardCount; i++){
				GameObject hazard = hazards [Random.Range (0, hazards.Length)];
				Vector3 spawnPosition = new Vector3 (Random.Range (-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
				Quaternion spawnRotation = Quaternion.identity;
				Instantiate (hazard, spawnPosition, spawnRotation);
				yield return new WaitForSeconds (spawnWait);
			}
			yield return new WaitForSeconds (waveWait);
			if(!checkPlayerProgressInLvl(true)){
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
			//spawnWaveCount++;
			print ("wave count inside bosswave = " + spawnWaveCount);
			if (!checkPlayerProgressInLvl (false)) {
				break;
			}
			if(spawnWaveCount == numOfWavesInLvl+1 && !gc.isGameOver()) {
				gc.levelCompleted ();
				yield return new WaitForSeconds (gc.getLoadLvlWait());
				if (gc.getLvlCount () >= 5) {
					gc.resetLvlCount ();
					SceneManager.LoadScene (gc.getLvlCount ());
				} else {
					SceneManager.LoadScene (gc.getLvlCount()+2);
				}
			}
		}
	}
#endregion


#region GenericLvl waveSpawning
	//function to spawn waves of hazards
	IEnumerator SpawnWaves(){
		yield return new WaitForSeconds (3f);
		gc.setGameOverText (true);
		yield return new WaitForSeconds (startWait);
		gc.setGameOverText (false);
		while(true){
			for(int i =0; i<hazardCount; i++){
				GameObject hazard = hazards [Random.Range (0, hazards.Length)];//Picks random hazard from hazards array
				Vector3 spawnPosition = new Vector3 (Random.Range (-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
				Quaternion spawnRotation = Quaternion.identity;
				Instantiate (hazard, spawnPosition, spawnRotation);
				yield return new WaitForSeconds (spawnWait);
			}
			yield return new WaitForSeconds (waveWait);
			if(!checkPlayerProgressInLvl(true)){
				break;
			}
		}
	}
#endregion
}

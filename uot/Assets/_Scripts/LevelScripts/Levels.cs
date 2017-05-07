using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/*
 *  Levels.
 * John G. Toland 3/10/17
 * This script is intended to use amoungst all levels.
 * This script controls the progress of the level.
 * Seperating the progress of the level and the over all gamecontroller allows for 
 * better readability. 
 * The addition of this script also allows form making all variables in GameController private, 
 * acessing these varibles are now done with getters and setters.
 * John G. Toland 4/10/17 Updated the detection of winning the level and Loading the next scene.
 * The earlier version was not the best way to do it.
 * */
///Nicholas Muirhead 4/16/17
///added lvl four boss spawn
public class Levels : MonoBehaviour
{
    //all level variables
	private SceneLoaderHandler SLH;
    private PlayerController pc;
    private GameController gc;
    public GameObject[] hazards;
    public Vector3 spawnValues;
    public int hazardCount;
    public float spawnWait;
    public float startWait;
    public float waveWait;
    public int numOfWavesInLvl;
    private int spawnWaveCount;
    private bool beginBossWaveGeneric;
    private bool beginBoss4;
	private bool NEED_RESPAWN;
	public int BossHazardCount;
	private PauseNavGUI pNG;
	public Scene currentScene;
    public GameObject Boss4;
	public GameObject explosion;
	private GameObject shipList;



    // Use this for initialization
    void Start(){
		NEED_RESPAWN = true;
		GameObject SLHo = GameObject.Find ("JOHNS_NAV_GUI_MOBILE");
		SLH = SLHo.GetComponent<SceneLoaderHandler> ();

		GameObject pauseNavGUI = GameObject.FindGameObjectWithTag ("PauseBtn");
		if(pauseNavGUI != null){
			pNG = pauseNavGUI.GetComponent<PauseNavGUI> ();
		}

        //get instance of gameController for access to game progress fucntions within your level
        GameObject gcObject = GameObject.FindGameObjectWithTag("GameController");
        if (gcObject != null){
            gc = gcObject.GetComponent<GameController>();
        }
		currentScene = SceneManager.GetActiveScene();
		shipList = GameObject.Find ("ShipList");

        GameObject pcObject = GameObject.FindGameObjectWithTag("Player");
        if(pcObject != null)
        {
            pc = pcObject.GetComponent<PlayerController>();
        }

    }

    // Update is called once per frame, this is were you will check to see if it is time for your boss wave to spawn.
    void Update(){
		if (currentScene.name.Contains ("Level_04")) {
			if (gc.isPlayerDead () && NEED_RESPAWN && !gc.isGameOver()) {
				StartCoroutine(ReSpawnLvl_04());

			}
		}

        ///spawning the boss wave for level_01
        if (beginBossWaveGeneric){
			StartCoroutine (SpawnBossWaveGeneric ());
            beginBossWaveGeneric = false;
        }else if (beginBoss4)
        {
            StartCoroutine(SpawnBoss4());
            beginBoss4 = false;
        }
    }

    #region methodsToStartCoroutines
    /// Starts the generic lvl Coroutine.
    public void StartGenericLvl(){
        StartCoroutine(SpawnWaves());
    }
    #endregion

    /// Checks the player progress in lvl.
    public bool checkPlayerProgressInLvl(bool isRegularWave){
        if (isRegularWave){
            spawnWaveCount++;
            print("wave count = " + spawnWaveCount);
            if (gc.isGameOver()){
                gc.setRestart(true);
                return false;
			}else if (gc.isPlayerDead() && !currentScene.name.Contains ("Level_04")){
                spawnWaveCount = 0;
                print("inside player is dead");
                gc.ReSpawn();
                gc.setPlayerDead(false);
                return true;
            }else if (spawnWaveCount == numOfWavesInLvl && !gc.isGameOver()){
                enteringBossWave(gc.getLvlCount());
                return false;
			}else if (pNG.isLEFT_SCENE ()) {
				return false;
			} else {
                return true;
            }
        }else{
            spawnWaveCount++;
            print("wave count = " + spawnWaveCount);
			if (gc.isGameOver ()) {
				gc.setRestart (true);
				return false;
			} else if (gc.isPlayerDead () && !currentScene.name.Contains ("Level_04")) {
				spawnWaveCount = numOfWavesInLvl;
				gc.ReSpawn ();
				gc.setPlayerDead (false);
				return true;
			} else if (spawnWaveCount == numOfWavesInLvl + 1 && !gc.isGameOver () && currentScene.name != "Level_04") {
				if (currentScene.name == "Level_03") {
					shipList.GetComponent<AnimationScript> ().playExitAni ();
				}
				StartCoroutine (LoadNewLvl ());
				return false;
			} else if (pNG.isLEFT_SCENE ()) {
				return false;
			}else {
				return true;
			}
        }
    }

	IEnumerator LoadNewLvl(){
		yield return new WaitForSeconds (3);
		if (gc.getLvlCount() >= 5){
			gc.levelCompleted();
			gc.resetLvlCount();
			yield return new WaitForSeconds (3);
			SLH.LoadNewSceneInt (1);
		}else{
			gc.levelCompleted ();
			yield return new WaitForSeconds (3);
			//level count + 3 (compensation for the login scene and player seleciton scene)
			SLH.LoadNewSceneInt (gc.getLvlCount ()+3);
		}
	}

    /// <summary>
    /// Enterings the boss wave.
    /// </summary>
    /// <param name="lvlCount">Lvl count.</param>
    public void enteringBossWave(int lvlCount){
        switch (lvlCount){
            case 1:
                beginBossWaveGeneric = true;
                break;
            case 2:
                //level 2 boss wave case
                beginBossWaveGeneric = true;                        ///used for testing
                break;
            case 3:
                //level 3 boss wave case
                beginBossWaveGeneric = true;                        ///used for testing
                break;
            case 4:
                //level 4 boss wave case
                beginBoss4 = true;                        ///used for testing
                break;
            case 5:
                //level 5 boss wave case
                beginBossWaveGeneric = true;                        ///used for testing
                break;
            default:
                //nothing to do here
                break;
        }
    }

    #region GenericLvl waveSpawning
    /// <summary>
    /// Spawns the waves.
    /// </summary>
    /// <returns>The waves.</returns>
    IEnumerator SpawnWaves(){
        ///we must wait for 3 seconds for the database to load and update the new information for each level.
		GameObject hazard;
        yield return new WaitForSeconds(3);
        gc.setGameOverText(true);
        yield return new WaitForSeconds(startWait);
        gc.setGameOverText(false);
        while (true){
            for (int i = 0; i < hazardCount; i++){
				if (currentScene.name == "Level_03") {
					hazard = hazards [Random.Range (0, 4)];//Picks random hazard from hazards array
				} else {
					hazard = hazards[Random.Range(0, hazards.Length)];//Picks random hazard from hazards array
				}
                Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                Quaternion spawnRotation = Quaternion.identity;
                Instantiate(hazard, spawnPosition, spawnRotation);
                yield return new WaitForSeconds(spawnWait);
            }
            yield return new WaitForSeconds(waveWait);
            if (!checkPlayerProgressInLvl(true)){
                break;
            }
        }
    }

    /// <summary>
    /// Spawns the boss wave level 01.
    /// </summary>
    /// <returns>The boss wave level 01.</returns>
    IEnumerator SpawnBossWaveGeneric(){
        yield return new WaitForSeconds(startWait);
		GameObject hazard;
        while (true){
            for (int i = 0; i < BossHazardCount; i++) {
				if (currentScene.name == "Level_03") {
					hazard = hazards [Random.Range (2, hazards.Length)];
				} else {
					hazard = hazards[Random.Range(0, hazards.Length)];
				}
                Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                Quaternion spawnRotation = Quaternion.identity;
                Instantiate(hazard, spawnPosition, spawnRotation);
				if (currentScene.name == "Level_03") {
					yield return new WaitForSeconds (0.25f);
				} else {
					yield return new WaitForSeconds(0.1f);
				}
            }
            yield return new WaitForSeconds(waveWait);
            //spawnWaveCount++;
            print("wave count inside bosswave = " + spawnWaveCount);
            if (!checkPlayerProgressInLvl(false)){
                break;
            }
        }
    }
    #endregion
    ///Spawn the boss for level 4
    ///
    IEnumerator SpawnBoss4()
    {
        yield return new WaitForSeconds(startWait);
        pc.numberOfSpawns = 0;
		Vector3 spawnPosition = new Vector3(0, spawnValues.y, spawnValues.z);
		Instantiate(Boss4, spawnPosition, this.gameObject.transform.rotation);
		GameObject BossClone = GameObject.Find ("Level_4_Boss(Clone)");
		if (BossClone) {
			print ("hellow world");
		}
		while (true) {
			yield return new WaitForSeconds (1f);
			checkPlayerProgressInLvl (false);
			float bossHealth = BossClone.GetComponent < Level_4_Boss_health >().CurrentHealth;
			if (!gc.isGameOver () && bossHealth <= 0.0f) {
				yield return new WaitForSeconds (0.8f);
				// adding Richards explosion thanks Rich!! this is a good asset.
				Instantiate(explosion, BossClone.transform.position, BossClone.transform.rotation);
				GameObject.Destroy (BossClone);
				StartCoroutine(LoadNewLvl());
				break;
			}
			yield return new WaitForSeconds (waveWait);
		}
    }

	IEnumerator ReSpawnLvl_04(){
		NEED_RESPAWN = false;
		yield return new WaitForSeconds (2f);
		print ("inside player is dead");
		gc.ReSpawn ();
		gc.setPlayerDead (false);
		yield return new WaitForSeconds (0);
		NEED_RESPAWN = true;
	}
}

//finito
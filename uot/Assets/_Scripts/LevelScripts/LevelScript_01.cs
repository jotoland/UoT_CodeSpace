using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// Level_01 Behavior Script (Spawns objects to the BPM of the music tracks)
/// John G. Toland 3/12/17
/// </summary>
public class LevelScript_01 : MonoBehaviour {
	private SceneLoaderHandler SLH;
	private Scene currentScene;
	private PauseNavGUI pB;
	private GameController gc;
	public Vector3 spawnValues;
	public int hazardCount;
	public int spawnX;
	private int spawnWaveCount;
	private bool isThisObjectScaling;

	#region Level_01 GameObject[] Vars
	public GameObject[] powerUpBox_01;
	public GameObject[] synthHazards_lvl_01;
	public GameObject[] snareHazards_lvl_01;
	public GameObject[] HHHazards_lvl_01;
	public GameObject[] KDHazards_lvl_01;
	public GameObject[] BassHazards_lvl_01;
	public GameObject[] KeyHazards_lvl_01;
	public GameObject[] SSHazards_lvl_01;
	#endregion

	#region Level_01 Algorithm Vars
	//level_01 only! public for testing
	//synth BPM = 46.7f
	//snare BPM = 45.8f
	//HH BPM = 184f
	//KD BPM = 45.3f
	//Bass BPM = 69.5f
	//Key BPM = 47f
	//SS BPM = 98f
	private bool WAITING_4KEYS;
	private bool RESTART_THE_BEAT;
	private bool WAITING_4SOLO;
	private bool WAITING_4SNAR_HH;
	private bool WAITING_4BEATDROP;
	private bool NEED_RESPAWN;
	private bool SPAWN_POWERUP;
	private bool NEED_NEW_LVL;
	public float synthBpm;
	public float snareBpm;
	public float HHBpm;
	public float KDBpm;
	public float BassBpm;
	public float KeyBpm;
	public float SSBpm;
	private int rythmnCount;
	private float lastTime, deltaTime, timer;

	private AudioSource song;
	#endregion

	// Use this for initialization
	void Start () {
		song = GetComponent<AudioSource> ();
		lastTime = 0f;
		deltaTime = 0f;
		timer = 0f;
		rythmnCount = 0;
		isThisObjectScaling = false;

		WAITING_4KEYS = true;
		RESTART_THE_BEAT = false;
		WAITING_4SOLO = true;
		WAITING_4SNAR_HH = true;
		WAITING_4BEATDROP = true;
		NEED_RESPAWN = true;
		SPAWN_POWERUP = true;
		NEED_NEW_LVL = true;

		GameObject SLHo = GameObject.Find ("JOHNS_NAV_GUI_MOBILE");
		SLH = SLHo.GetComponent<SceneLoaderHandler> ();

		//get instance of gameController for access to game progress fucntions within your level
		GameObject gcObject = GameObject.FindGameObjectWithTag ("GameController");
		if (gcObject != null) {
			gc = gcObject.GetComponent <GameController> ();
		}
		GameObject pBObject = GameObject.FindGameObjectWithTag ("PauseBtn");
		if (pBObject != null) {
			pB = pBObject.GetComponent <PauseNavGUI> ();
		}
	}

	// Update is called once per frame, this is were you will check to see if it is time for your boss wave to spawn.
	void Update () {
		if (gc.isPlayerDead () && NEED_RESPAWN && !gc.isGameOver()) {
			StartCoroutine(ReSpawnLvl_01());

		}
		if ((spawnWaveCount%1 == 0) && SPAWN_POWERUP) {
			StartCoroutine(SpawnPowerUps());

		}
		if ((spawnWaveCount == 12 && WAITING_4KEYS)||
			(spawnWaveCount == 44 && WAITING_4KEYS)) {
			print ("Tickle the Ivory a Little More!");
			WAITING_4KEYS = false;
			print("waiting for keys inside start the keys = " + WAITING_4KEYS);
			StartCoroutine (SpawnKeyWavesLevel_01 ());
		}
		if((rythmnCount == 13 && WAITING_4SOLO)){
			print("Bass Synth Solo!!!");
			WAITING_4SOLO = false;
			StartCoroutine(SpawnSSWavesLevel_01());
		}
		if((spawnWaveCount == 97 && WAITING_4SNAR_HH)){
			print("Incoming HH and Snare");
			WAITING_4SNAR_HH = false;
			StartCoroutine(SpawnSnareWavesLevel_01());
			StartCoroutine(SpawnHighHatWavesLevel_01());
		}
		if((spawnWaveCount == 110 && WAITING_4BEATDROP)){
			print("Incoming Beat Drop! (KD/BASS)");
			WAITING_4BEATDROP = false;
			StartCoroutine(SpawnKDWavesLevel_01());
			StartCoroutine(SpawnBassWavesLevel_01());
		}
		if (RESTART_THE_BEAT) {
			RESTART_THE_BEAT = false;
			print ("Restarting the beat from update");
			StartCoroutine (SpawnSynthWavesLevel_01 ());
		}
		if(!song.isPlaying && !gc.isGameOver() && NEED_NEW_LVL && !pB.GameIsPaused () && spawnWaveCount > 100) {
			NEED_NEW_LVL = false;
			StartCoroutine(LoadNewLvl());
		}
		if(!pB.GameIsPaused() && gc.isGameOver()){
			gc.setRestart(true);
		}
	}

	public bool isScaling(){
		return isThisObjectScaling;
	}

	#region methodsToStartCoroutines
	/// <summary>
	/// Starts the lvl one Coroutine.
	/// </summary>
	public void StartLvlOne(){
		StartCoroutine (SpawnSynthWavesLevel_01 ());
	}
	#endregion

	IEnumerator LoadNewLvl(){
		gc.levelCompleted ();
		yield return new WaitForSeconds (3);
		print("[LoadNewLvl_Level_01Script] levelCount = " + gc.getLvlCount ());
		//level count + 3 (compensation for the login scene and player seleciton scene)
		//SceneManager.LoadScene (gc.getLvlCount()+3);
		SLH.LoadNewSceneInt (gc.getLvlCount ()+3);
	}

	IEnumerator ReSpawnLvl_01(){
		NEED_RESPAWN = false;
		yield return new WaitForSeconds (2f);
		print ("inside player is dead");
		gc.ReSpawn ();
		gc.setPlayerDead (false);
		yield return new WaitForSeconds (0);
		NEED_RESPAWN = true;
	}

	IEnumerator SpawnPowerUps(){
		SPAWN_POWERUP = false;
		yield return new WaitForSeconds (3);
		if (spawnWaveCount < 0 && !gc.isGameOver()) {
			yield return new WaitForSeconds (3f);
			gc.setGameOverText (true);
			yield return new WaitForSeconds (60f / synthBpm);
			gc.setGameOverText (false);
		}
		int pos;
		int id = -1;
		while(true){
			pos = id * spawnX;
			for(int i =0; i<5; i++){
				GameObject powerUp = powerUpBox_01 [Random.Range (0, powerUpBox_01.Length)];
				Vector3 spawnPosition = new Vector3 (pos, spawnValues.y, spawnValues.z);
				Quaternion spawnRotation = Quaternion.identity;
				Instantiate (powerUp, spawnPosition, spawnRotation);
				pos = pos * id;
				yield return new WaitForSeconds (3);
			}
		break;
		}
		SPAWN_POWERUP = true;	
	}


	/// <summary>
	/// Checks the player progress in lvl.
	/// </summary>
	/// <returns><c>true</c>, if player progress in lvl was checked, <c>false</c> otherwise.</returns>
	/// <param name="isRegularWave">If set to <c>true</c> is regular wave.</param>
	public bool checkPlayerProgressInLvl(bool isRegularWave){
		if (isRegularWave) {
			spawnWaveCount++;
			print ("wave count = " + spawnWaveCount);
			if ((rythmnCount == 5 && spawnWaveCount < 40) ||
			    (rythmnCount == 11 && spawnWaveCount < 88) ||
			    (rythmnCount == 24 && spawnWaveCount < 200) ||
			    (rythmnCount == 25 && spawnWaveCount < 208)) {
				print ("Putting on the Breaks!!");
				return false;
			} else if (gc.isGameOver ()) {
				gc.setRestart (true);
				return false;
			} /*else if (gc.isPlayerDead ()) {
				print ("inside player is dead");
				gc.ReSpawn ();
				gc.setPlayerDead (false);
				return true;
			} */else {
				return true;
			}
		} else {
			return true;
		}
	}
				
	IEnumerator SpawnSynthWavesLevel_01 (){
		if (spawnWaveCount == 0) {
			yield return new WaitForSeconds (3);
			gc.setGameOverText (true);
			yield return new WaitForSeconds (60f / synthBpm);
			gc.setGameOverText (false);
		} else {
			//could add some cool text to the user right here for when the beat restarts.
		}
		int pos;
		int id = -1;
		while (true) {
			deltaTime = GetComponent<AudioSource> ().time - lastTime;
			timer += deltaTime;
			pos = id * spawnX;
			if (timer >= (60f /snareBpm)) {
				int j = 0;
				for (int i = 0; i < hazardCount; i++) {
					GameObject hazard = synthHazards_lvl_01 [j];
					Vector3 spawnPosition = new Vector3 (pos, spawnValues.y, spawnValues.z);
					Quaternion spawnRotation = Quaternion.identity;
					Instantiate (hazard, spawnPosition, spawnRotation);
					pos = pos * id;
					if (j < synthHazards_lvl_01.Length-1) {
						j++;
					} else {
						j = 0;
					}
					yield return new WaitForSeconds (60f/synthBpm);
				}
			}
			lastTime = GetComponent<AudioSource> ().time;
			yield return new WaitForSeconds (0);
			if (!checkPlayerProgressInLvl (true)) {
				print (" inside Synth = " + spawnWaveCount);
				break;
			}
			rythmnCount++;
			print ("rythmnCount = " + rythmnCount);
			if (rythmnCount == 1 || rythmnCount == 6) {
				WAITING_4KEYS = true;
				print ("Incoming Beat Drop!!!");
				print("waiting for keys inside synthwave = " + WAITING_4KEYS);
				if (rythmnCount == 1) {
					yield return new WaitForSecondsRealtime (4f);
				} else if (rythmnCount == 6) {
					yield return new WaitForSecondsRealtime (0f);
				}
				StartCoroutine (SpawnSnareWavesLevel_01 ());
				StartCoroutine (SpawnHighHatWavesLevel_01 ());
				StartCoroutine (SpawnKDWavesLevel_01 ());
				StartCoroutine (SpawnBassWavesLevel_01 ());
			}
			//print ("synthbpm = " + synthBpm);
		}
	}

	IEnumerator SpawnSnareWavesLevel_01 (){
		int pos;
		int id = -1;
		yield return new WaitForSeconds (60f/snareBpm);
		if (spawnWaveCount >= 97) {
			yield return new WaitForSeconds (1f);
		}
		while (true) {
			deltaTime = GetComponent<AudioSource> ().time - lastTime;
			timer += deltaTime;
			pos = id * spawnX;
			id = id * -1;
			if (timer >= (60f /snareBpm)) {
				int j = 0;
				for (int i = 0; i < hazardCount; i++) {
					GameObject hazard = snareHazards_lvl_01 [j];
					Vector3 spawnPosition = new Vector3 (pos, spawnValues.y, spawnValues.z);
					Quaternion spawnRotation = Quaternion.identity;
					Instantiate (hazard, spawnPosition, spawnRotation);
					pos = pos + id;
					if (j < snareHazards_lvl_01.Length-1) {
						j++;
					} else {
						j = 0;
					}
					yield return new WaitForSeconds (60f/snareBpm);
				}
			}
			lastTime = GetComponent<AudioSource> ().time;
			yield return new WaitForSeconds (0);
			if (!checkPlayerProgressInLvl (true)) {
				print (" inside snare = " + spawnWaveCount);
				break;
			}
			//print ("snarebpm = " + snareBpm);
		}
	}

	IEnumerator SpawnHighHatWavesLevel_01(){
		int pos;
		int id = -1;
		yield return new WaitForSeconds (60f / HHBpm);
		if (spawnWaveCount >= 97) {
			yield return new WaitForSeconds (1f);
		}
		while (true) {
			deltaTime = GetComponent<AudioSource> ().time - lastTime;
			timer += deltaTime;
			pos = id * spawnX;
			id = id * -1;
			if (timer >= (60f / HHBpm)) {
				int j = 0;
				for (int i = 0; i < hazardCount; i++) {
					GameObject hazard = HHHazards_lvl_01 [j];
					Vector3 spawnPosition = new Vector3 (pos, spawnValues.y, spawnValues.z);
					Quaternion spawnRotation = Quaternion.identity;
					Instantiate (hazard, spawnPosition, spawnRotation);
					pos = pos + id;
					if (j < HHHazards_lvl_01.Length-1) {
						j++;
					} else {
						j = 0;
					}
					yield return new WaitForSeconds (60f / HHBpm);
				}
			}
			lastTime = GetComponent<AudioSource> ().time;
			yield return new WaitForSeconds (0);
			if (!checkPlayerProgressInLvl (true)) {
				print (" inside HH = " + spawnWaveCount);
				break;
			}
			//print ("HHbpm = " + HHBpm);
		}
	}

	IEnumerator SpawnKDWavesLevel_01 (){
		int pos;
		int id = -1;
		if (spawnWaveCount >= 110) {
			yield return new WaitForSeconds (1f);
		}
		while (true) {
			deltaTime = GetComponent<AudioSource> ().time - lastTime;
			timer += deltaTime;
			pos = id * -spawnX;
			id = id * -1;
			if (timer >= (60f /KDBpm)) {
				int j = 0;
				for (int i = 0; i < hazardCount; i++) {
					GameObject hazard = KDHazards_lvl_01 [j];
					Vector3 spawnPosition = new Vector3 (pos, spawnValues.y, spawnValues.z);
					Quaternion spawnRotation = Quaternion.identity;
					Instantiate (hazard, spawnPosition, spawnRotation);
					pos = pos - id;
					if (j < KDHazards_lvl_01.Length-1) {
						j++;
					} else {
						j = 0;
					}
					yield return new WaitForSeconds (60f/KDBpm);
				}
			}
			lastTime = GetComponent<AudioSource> ().time;
			yield return new WaitForSeconds (0);
			if (!checkPlayerProgressInLvl (true)) {
				print (" inside KD = " + spawnWaveCount);
				yield return new WaitForSeconds (1);
				break;
			}
			//print ("Kickbpm = " + KDBpm);
		}
	}

	IEnumerator SpawnBassWavesLevel_01 (){
		int pos;
		int id = -1;
		if (spawnWaveCount >= 110) {
			yield return new WaitForSeconds (1f);
		}
		while (true) {
			deltaTime = GetComponent<AudioSource> ().time - lastTime;
			timer += deltaTime;
			pos = id * -spawnX;
			id = id * -1;
			if (timer >= (60f /BassBpm)) {
				int j = 0;
				for (int i = 0; i < hazardCount; i++) {
					GameObject hazard = BassHazards_lvl_01 [j];
					Vector3 spawnPosition = new Vector3 (pos, spawnValues.y, spawnValues.z);
					Quaternion spawnRotation = Quaternion.identity;
					Instantiate (hazard, spawnPosition, spawnRotation);
					pos = pos - id;
					if (j < BassHazards_lvl_01.Length-1) {
						j++;
					} else {
						j = 0;
					}
					yield return new WaitForSeconds (60f/BassBpm);
				}
			}
			lastTime = GetComponent<AudioSource> ().time;
			yield return new WaitForSeconds (0);
			if (!checkPlayerProgressInLvl (true)) {
				print (" inside Bass = " + spawnWaveCount);
				break;
			}
			//print ("Bassbpm = " + BassBpm);
		}
	}

	IEnumerator SpawnKeyWavesLevel_01 (){
		int pos;
		int id = -1;
		while (true) {
			deltaTime = GetComponent<AudioSource> ().time - lastTime;
			timer += deltaTime;
			pos = id * spawnX;
			if (timer >= (60f /KeyBpm)) {
				int j = 0;
				for (int i = 0; i < hazardCount; i++) {
					GameObject hazard = KeyHazards_lvl_01 [j];
					Vector3 spawnPosition = new Vector3 (pos, spawnValues.y, spawnValues.z);
					Quaternion spawnRotation = Quaternion.identity;
					Instantiate (hazard, spawnPosition, spawnRotation);
					pos = pos * id;
					if (j < KeyHazards_lvl_01.Length-1) {
						j++;
					} else {
						j = 0;
					}
					yield return new WaitForSeconds (60f/KeyBpm);
				}
			}
			lastTime = GetComponent<AudioSource> ().time;
			yield return new WaitForSeconds (0);
			if (!checkPlayerProgressInLvl (true)) {
				print ("inside spawnKeyWaves Rythmn count = " + rythmnCount);
				if (rythmnCount == 5) {
					RESTART_THE_BEAT = true;
				}
				print ("inside keys restarting = " + RESTART_THE_BEAT);
				print (" inside Keys = " + spawnWaveCount);
				break;
			}
			//print ("Keybpm = " + KeyBpm);
		}
	}

	IEnumerator SpawnSSWavesLevel_01 (){
		yield return new WaitForSeconds (3f);
		print ("inside Spawning SS wavesLevel_01");
		int pos;
		int id = 1;
		while (true) {
			deltaTime = GetComponent<AudioSource> ().time - lastTime;
			timer += deltaTime;
			pos = id * spawnX;
			id = id * -1;
			if (timer >= (60f /SSBpm)) {
				int j = 0;
				for (int i = 0; i < hazardCount; i++) {
					GameObject hazard = SSHazards_lvl_01 [j];
					Vector3 spawnPosition = new Vector3 (pos, spawnValues.y, spawnValues.z);
					Quaternion spawnRotation = Quaternion.identity;
					Instantiate (hazard, spawnPosition, spawnRotation);
					pos = pos + id;
					if (j < SSHazards_lvl_01.Length-1) {
						j++;
					} else {
						j = 0;
					}
					yield return new WaitForSeconds (60f/SSBpm);
					if (!isThisObjectScaling) {
						isThisObjectScaling = true;
					}

				}
			}
			lastTime = GetComponent<AudioSource> ().time;
			yield return new WaitForSeconds (0);
			if (!checkPlayerProgressInLvl (true)) {
				print (" inside SS = " + spawnWaveCount);
				break;
			}
			//print ("Keybpm = " + KeyBpm);
		}
	}
}
//finito

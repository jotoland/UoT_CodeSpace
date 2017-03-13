using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// Level_01 Behavior Script (Spawns objects to the BPM of the music tracks)
/// John G. Toland 3/12/17
/// </summary>
public class LevelScript_01 : MonoBehaviour {

	private GameController gc;
	public Vector3 spawnValues;
	public int hazardCount;
	private int spawnWaveCount;

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
	public float synthBpm;
	public float snareBpm;
	public float HHBpm;
	public float KDBpm;
	public float BassBpm;
	public float KeyBpm;
	public float SSBpm;
	private int rythmnCount;
	private float lastTime, deltaTime, timer;
	#endregion

	// Use this for initialization
	void Start () {
		#region Level_01 use of Start()
		//level_01 only!
		lastTime = 0f;
		deltaTime = 0f;
		timer = 0f;
		rythmnCount = 0;

		WAITING_4KEYS = true;
		RESTART_THE_BEAT = false;
		WAITING_4SOLO = true;
		WAITING_4SNAR_HH = true;
		WAITING_4BEATDROP = true;
		NEED_RESPAWN = true;
		SPAWN_POWERUP = true;

		#endregion

		//get instance of gameController for access to game progress fucntions within your level
		GameObject gcObject = GameObject.FindGameObjectWithTag ("GameController");
		if (gcObject != null) {
			gc = gcObject.GetComponent <GameController> ();
		}
	}

	// Update is called once per frame, this is were you will check to see if it is time for your boss wave to spawn.
	void Update () {
		#region Level_01 use of Update()
		//LEVEL_01 Only!!
		if (gc.isPlayerDead () && NEED_RESPAWN) {
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
			StartCoroutine (SpawnSynthWavesLevel_01 ());
		}
		#endregion
	}


	#region methodsToStartCoroutines
	/// <summary>
	/// Starts the lvl one Coroutine.
	/// </summary>
	public void StartLvlOne(){
		StartCoroutine (SpawnSynthWavesLevel_01 ());
	}
	#endregion

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
		yield return new WaitForSeconds (4f);
		int pos;
		int id = -1;
		while(true){
			pos = id * 6;
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
			yield return new WaitForSeconds (3f);
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
			pos = id * 6;
			if (timer >= (60f /snareBpm)) {
				for (int i = 0; i < hazardCount; i++) {
					GameObject hazard = synthHazards_lvl_01 [Random.Range (0, synthHazards_lvl_01.Length)];
					Vector3 spawnPosition = new Vector3 (pos, spawnValues.y, spawnValues.z);
					Quaternion spawnRotation = Quaternion.identity;
					Instantiate (hazard, spawnPosition, spawnRotation);
					pos = pos * id;
					yield return new WaitForSeconds (60f/synthBpm);
				}
			}
			lastTime = GetComponent<AudioSource> ().time;
			yield return new WaitForSeconds (0);
			if (!checkPlayerProgressInLvl (true)) {
				print (" inside Synth = " + spawnWaveCount);
				if(spawnWaveCount == 207 && !gc.isGameOver()) {
					gc.levelCompleted ();
					yield return new WaitForSeconds (gc.getLoadLvlWait());
					//if (gc.getLvlCount () >= 5) {
					//gc.resetLvlCount ();
					//SceneManager.LoadScene (gc.getLvlCount ());
					//} else {
					SceneManager.LoadScene (gc.getLvlCount()+2);
					//}
				}
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
			pos = id * 6;
			id = id * -1;
			if (timer >= (60f /snareBpm)) {
				for (int i = 0; i < hazardCount; i++) {
					GameObject hazard = snareHazards_lvl_01 [Random.Range (0, snareHazards_lvl_01.Length)];
					Vector3 spawnPosition = new Vector3 (pos, spawnValues.y, spawnValues.z);
					Quaternion spawnRotation = Quaternion.identity;
					Instantiate (hazard, spawnPosition, spawnRotation);
					pos = pos + id;
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
			pos = id * 6;
			id = id * -1;
			if (timer >= (60f / HHBpm)) {
				for (int i = 0; i < hazardCount; i++) {
					GameObject hazard = HHHazards_lvl_01 [Random.Range (0, HHHazards_lvl_01.Length)];
					Vector3 spawnPosition = new Vector3 (pos, spawnValues.y, spawnValues.z);
					Quaternion spawnRotation = Quaternion.identity;
					Instantiate (hazard, spawnPosition, spawnRotation);
					pos = pos + id;
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
			pos = id * -6;
			id = id * -1;
			if (timer >= (60f /KDBpm)) {
				for (int i = 0; i < hazardCount; i++) {
					GameObject hazard = KDHazards_lvl_01 [Random.Range (0, KDHazards_lvl_01.Length)];
					Vector3 spawnPosition = new Vector3 (pos, spawnValues.y, spawnValues.z);
					Quaternion spawnRotation = Quaternion.identity;
					Instantiate (hazard, spawnPosition, spawnRotation);
					pos = pos - id;
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
			pos = id * -6;
			id = id * -1;
			if (timer >= (60f /BassBpm)) {
				for (int i = 0; i < hazardCount; i++) {
					GameObject hazard = BassHazards_lvl_01 [Random.Range (0, BassHazards_lvl_01.Length)];
					Vector3 spawnPosition = new Vector3 (pos, spawnValues.y, spawnValues.z);
					Quaternion spawnRotation = Quaternion.identity;
					Instantiate (hazard, spawnPosition, spawnRotation);
					pos = pos - id;
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
			pos = id * 6;
			if (timer >= (60f /KeyBpm)) {
				for (int i = 0; i < hazardCount; i++) {
					GameObject hazard = KeyHazards_lvl_01 [Random.Range (0, KeyHazards_lvl_01.Length)];
					Vector3 spawnPosition = new Vector3 (pos, spawnValues.y, spawnValues.z);
					Quaternion spawnRotation = Quaternion.identity;
					Instantiate (hazard, spawnPosition, spawnRotation);
					pos = pos * id;
					yield return new WaitForSeconds (60f/KeyBpm);
				}
			}
			lastTime = GetComponent<AudioSource> ().time;
			yield return new WaitForSeconds (0);
			if (!checkPlayerProgressInLvl (true)) {
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
			pos = id * 6;
			id = id * -1;
			if (timer >= (60f /SSBpm)) {
				for (int i = 0; i < hazardCount; i++) {
					GameObject hazard = SSHazards_lvl_01 [Random.Range (0, SSHazards_lvl_01.Length)];
					Vector3 spawnPosition = new Vector3 (pos, spawnValues.y, spawnValues.z);
					Quaternion spawnRotation = Quaternion.identity;
					Instantiate (hazard, spawnPosition, spawnRotation);
					pos = pos + id;
					yield return new WaitForSeconds (60f/SSBpm);
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

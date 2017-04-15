using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Created By Richard O'Neal 04/17/207
// detects if the player has shot the boss 
// if so deduct health if health = 0 then destroy the game object
public class Lvl05BossHealth : MonoBehaviour {

	public static int Health;
	private GameController gc;
	public GameObject missileExplosion;
	public GameObject explosion;
	public GameObject enemy;
	private bool hasSpawned1 = false;
	private bool hasSpawned2 = false;
	private bool hasSpawned3 = false;
	private static int HealthDeducted = 0;

	void Start(){
		Health = 135;
		GameObject gcObject = GameObject.FindGameObjectWithTag("GameController");
		if (gcObject != null)
		{
			gc = gcObject.GetComponent<GameController>();
		}
	}
	void OnTriggerEnter(Collider other) 
	{
		//check if the boss is being hit by the players bolt
		// if so deducted one health
		if(other.CompareTag("Bolt"))
			{
			if (Health != 0) {
				Health = Health - 1;
				HealthDeducted += 1;
				Debug.Log ("Health deducted = " + Health);
				gameObject.GetComponent<Animation>().Play ();

				Destroy(other.gameObject);

			}

				
			}
		if (other.CompareTag ("Missile")) {

			if (Health != 0) {
				Health = Health - 10;
				HealthDeducted += 10;
				Debug.Log ("Health deducted = " + Health);
				gameObject.GetComponent<Animation>().Play ();
				Instantiate (missileExplosion, other.transform.position, other.transform.rotation);
				Destroy(other.gameObject);

			}
		}

	}

	void Update(){
		//print (Health);
		if(Health == 0)
		{
			GetComponent<Mover> ().enabled = false;
			GetComponent<EvasiveManeuver> ().enabled = false;
			GetComponent<WeaponController> ().enabled = false;
			gc.levelCompleted ();
			Vector3 Position = new Vector3(0, 0, 10);
			Quaternion Rotation = Quaternion.identity;
			Instantiate (explosion, Position, Rotation);
			AudioSource audio = gameObject.GetComponent<AudioSource >();
			audio.PlayOneShot((AudioClip)Resources.Load("Boss Death audio"));
			//Levels05.LoadNewLvl ();
			//GameObject.Find ("GameController").GetComponent<Level_02> ().setBOSS_IS_DEAD (true);
			GetComponent<Rigidbody> ().position = new Vector3 (gameObject.transform.position.x, -20, gameObject.transform.position.z);
			Destroy (this, 3f);
		}
		if(HealthDeducted == 25 && hasSpawned1 != true) {
			
				Vector3 spawnPosition = new Vector3 (Random.Range (-5, 5), 0, 15);
				Quaternion spawnRotation = Quaternion.identity;
				Instantiate (enemy, spawnPosition, spawnRotation);
				hasSpawned1 = true;
		}
		if(HealthDeducted == 50 && hasSpawned2 != true) {

			Vector3 spawnPosition = new Vector3 (Random.Range (-5, 5), 0, 15);
			Quaternion spawnRotation = Quaternion.identity;
			Instantiate (enemy, spawnPosition, spawnRotation);

			hasSpawned2 = true;
		}
		if(HealthDeducted == 75 && hasSpawned3 != true) {

			Vector3 spawnPosition = new Vector3 (Random.Range (-5, 5), 0, 15);
			Quaternion spawnRotation = Quaternion.identity;
			Instantiate (enemy, spawnPosition, spawnRotation);

			hasSpawned3 = true;
		}


		
	}
	public int getHealth(){
		return Health;
	}
}

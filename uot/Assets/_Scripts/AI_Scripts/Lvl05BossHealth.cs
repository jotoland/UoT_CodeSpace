using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Created By Richard O'Neal 04/17/2017
// detects if the player has shot the boss 
// if so deduct health if health = 0 then destroy the game object
public class Lvl05BossHealth : MonoBehaviour {

	public static int Health;
	private GameController gc;

	public GameObject explosion;


	void Start(){
		Health = 120;
		GameObject gcObject = GameObject.FindGameObjectWithTag("GameController");
		if (gcObject != null)
		{
			gc = gcObject.GetComponent<GameController>();
		}
	}
	void OnTriggerEnter(Collider other) 
	{
		if(other.CompareTag("Bolt")|| other.CompareTag("Missile"))
			{
			if (Health != 0) {
				Health = Health - 1;
				Debug.Log ("Health deducted = " + Health);
				gameObject.GetComponent<Animation>().Play ();

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
	}
	public int getHealth(){
		return Health;
	}
}

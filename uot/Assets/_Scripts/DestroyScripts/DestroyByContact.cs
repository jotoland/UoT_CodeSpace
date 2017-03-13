using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Destroy by contact.
/// 
/// 02/17/17 John G. Toland
/// 02/20/17 Richard O'Neal
/// used to destroy hazards with bolts or collisions
/// 
/// </summary>



public class DestroyByContact : MonoBehaviour {

	//reference to public explosion event
	public GameObject explosion;
	public GameObject playerExplosion;
	public int scoreValue;	//value of object
	public GameObject missileExplosion;
	public GameObject MissileDamage;
	private GameController gameController;	//reference to instance of gamecontroller
	private Levels lvl;
	public bool deadPlayer = false;

	void Start () {
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");	//Finding game object that holds gamecontroller script
		if (gameControllerObject != null) {
			gameController = gameControllerObject.GetComponent <GameController>();	//set reference to game controller component
		}
		if (gameControllerObject == null) {
			Debug.Log ("Cannot find 'GameController' script");	//in the case there is no reference object
		}
		GameObject lvlObject = GameObject.FindWithTag ("GameController");	//Finding game object that holds gamecontroller script
		if (lvlObject != null) {
			lvl = gameControllerObject.GetComponent <Levels>();	//set reference to game controller component
		}
	}

	void OnTriggerEnter(Collider other) 
	{
		///do not destroy if its inside the boundary


		if (other.CompareTag("Boundary") || other.CompareTag("Enemy"))

		if (other.CompareTag("Boundary") || other.CompareTag("Enemy") ||
			other.CompareTag("Rupee") || other.tag == "PowerStar" || other.tag == "OneUpHeart")


		if (other.CompareTag("Boundary") || other.CompareTag("Enemy") ||
			other.CompareTag("Rupee") || other.tag == "PowerStar" || other.tag == "OneUpHeart" || other.tag == "PickUp")

		{
			return;
		}
		//creating explosion for asteroids being shot
		if (explosion != null) {
			Instantiate (explosion, transform.position, transform.rotation);
			lvl.spawnRupee (transform.position, other.transform.rotation);
		}
<<<<<<< HEAD
=======

		// explosion for the missle and the spawning of the explosion collider
>>>>>>> DevelopmentBranch
		if (other.tag == "Missile") {
			Instantiate (MissileDamage, other.transform.position, other.transform.rotation);
			Instantiate (missileExplosion, other.transform.position, other.transform.rotation);
			Destroy(other.gameObject);
		}
		if (other.tag == "SplashDamage") {
			Instantiate (explosion, transform.position, transform.rotation);
		}
		//explosion for ramming the asteroid
		if (other.tag == "Player") {
			Instantiate (playerExplosion, other.transform.position, other.transform.rotation);
			if (gameController.getLivesCount() == 1) {
				gameController.AddLife (-1);
				gameController.GameOver ();
			} else {
				///loss of one life
				gameController.AddLife (-1);
			}
			deadPlayer = true;
			gameController.setPlayerDead (true);
			Destroy (other.gameObject);

		}
		gameController.AddScore (scoreValue);
		Destroy(gameObject);
	}


}
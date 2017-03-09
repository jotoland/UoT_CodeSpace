﻿using System.Collections;
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


	void Start () {
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");	//Finding game object that holds gamecontroller script
		if (gameControllerObject != null) {
			gameController = gameControllerObject.GetComponent <GameController>();	//set reference to game controller component
		}
		if (gameControllerObject == null) {
			Debug.Log ("Cannot find 'GameController' script");	//in the case there is no reference object
		}
	}

	void OnTriggerEnter(Collider other) 
	{
		///do not destroy if its inside the boundary
		if (other.CompareTag("Boundary") || other.CompareTag("Enemy") || other.CompareTag("EnemySpider"))
		{
			return;
		}
		//creating explosion for asteroids being shot
		if (explosion != null) {
			Instantiate (explosion, transform.position, transform.rotation);
			gameController.spawnRupee (transform.position, other.transform.rotation);
		}
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
			gameController.GameOver ();
		}
		gameController.AddScore (scoreValue);	//call AddScore for reference controller

		///does not matter which one to destroy first but this makes sense.

		//other.transform.Translate(-10, -10, Time.deltaTime);
		Destroy(other.gameObject);
		Destroy(gameObject);
	}


}
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Destroy by contact.
/// 02/17/17 John G. Toland
/// 02/20/17 Richard O'Neal
/// used to destroy hazards with bolts or collisions

/// 03/20/2017 Andrew Salopek
/// 04/14/2017 Nicholas Muirhead
/// 04/15/2017 Nicholas Muirhead
/// </summary>
public class DestroyByContact : MonoBehaviour {

	//reference to public explosion event
	public GameObject explosion;
	public GameObject playerExplosion;
	public int scoreValue;	//value of object
	public GameObject missileExplosion;
	public GameObject MissileDamage;
	private GameController gameController;	//reference to instance of gamecontroller
	public bool deadPlayer = false;

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
        ///do not destroy Boss_Bolt_4 if colliding with Bolt or other Boss_Bolt_4
        if (gameObject.tag == "Boss_Bolt_4")
        {
            if (other.tag == "Bolt" || other.tag == "Boss_Bolt_4")
            {
                return;
            }
        }
        /// Destory comet trail without dropping rupees
        if(gameObject.tag == "Comet_Trail")
        {
            if(other.tag == "Bolt")
            {
                if(explosion != null)
                {
                    Instantiate(explosion, transform.position, transform.rotation);
                    Destroy(gameObject);
                    return;
                }
            }
        }

        ///do not destroy if its inside the boundary
        if (other.CompareTag("Boundary") || other.CompareTag("Enemy") || other.CompareTag("Lvl05Boss") || 
			other.CompareTag("Rupee") || other.tag == "PowerStar" || other.tag == "OneUpHeart" || other.tag == "PickUp" || other.tag== "BossBoundary" ||
            other.tag == "Comet_Trail" || other.tag == "Lvl_4_Boss" ){
			return;
		}
		//creating explosion for asteroids being shot
		if (explosion != null) {
			Instantiate (explosion, transform.position, transform.rotation);
			if (gameController != null) {
				gameController.spawnRupee (transform.position, other.transform.rotation);
			}
		}

		if (other.tag == "Missile") {
			Instantiate (MissileDamage, other.transform.position, other.transform.rotation);
			Instantiate (missileExplosion, other.transform.position, other.transform.rotation);
			Destroy(other.gameObject);
		}

		if(other.tag == "Bolt_Wingmen"){
			Instantiate (explosion, transform.position, transform.rotation);
			gameController.AddWingDestrCnt (1);
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
		if (other.tag == "WingMan" || other.tag =="WingMan1") {
			Instantiate (playerExplosion, other.transform.position, other.transform.rotation);
			Instantiate (explosion, transform.position, transform.rotation);
//			Destroy (gameObject);
//			Destroy (other.gameObject);

		}
		if (CompareTag("BossWeapon")) {
			if(other.CompareTag("Bolt")){
				return;
			}
				
		}
		if (CompareTag("Enemy")) {
			if(other.CompareTag("BossWeapon")){
				return;
			}

		}
		gameController.AddScore (scoreValue);
		Destroy(gameObject);
		Destroy (other.gameObject);
	}
}
//finito
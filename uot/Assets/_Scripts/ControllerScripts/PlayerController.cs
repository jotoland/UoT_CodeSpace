//Richard O'Neal 2/17/2017
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//System.Serializable will allow us to view this made class in the inspector
[System.Serializable]
//This is to clean up The inspector panel, to allow boundary to
// be a seperate class
public class Boundary 
{
	public float xMin, xMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour {
	
	public float tilt;
	public float speed;
	public Boundary boundary;
	
	public GameObject shot;
	public Transform shotSpawn;
	public float fireRate;
	public GameObject missile;
	public Transform missileSpawn;
	public float MissileCooldown;
	private GameController gameController;
	private float nextFire;
	private float nextMissile;
	private int missileShot = -1;


	void Start () {
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");	//Finding game object that holds gamecontroller script
		if (gameControllerObject != null) {
			gameController = gameControllerObject.GetComponent <GameController>();	//set reference to game controller component
		}
		if (gameControllerObject == null) {
			Debug.Log ("Cannot find 'GameController' script");	//in the case there is no reference object
		}
	}
	void Update ()
	{
		//if mouse button is pressed instantiate the bolt and play shooting sound
		if (Input.GetButton("Fire1") && Time.time > nextFire) 
		{
			nextFire = Time.time + fireRate;
			Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
			GetComponent<AudioSource>().Play ();
		}
		if (Input.GetButton("Fire2") && Time.time > nextMissile) 
		{
			nextMissile = Time.time + MissileCooldown;

			//if the user has no missiles then cant fire missiles
			if (gameController.missileCount == 0) {
				return;
			} else {
				Instantiate(missile, missileSpawn.position, missileSpawn.rotation);
				GetComponent<AudioSource>().Play ();
				gameController.AddMissiles (missileShot);
			}
		}
	}

	void FixedUpdate(){

		// moveHorizontal is how much we want to move left and right, x
		float moveHorizontal = Input.GetAxis ("Horizontal");
		//moveVertical is how much we want to move up and down, y
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		GetComponent<Rigidbody>().velocity = movement * speed;

		//Creates Constraints as boundaries for ship
		GetComponent<Rigidbody>().position = new Vector3(
			Mathf.Clamp (GetComponent<Rigidbody>().position.x, boundary.xMin, boundary.xMax), 
			0.0f, 
			Mathf.Clamp (GetComponent<Rigidbody>().position.z, boundary.zMin, boundary.zMax)
		);
		
		GetComponent<Rigidbody>().rotation = Quaternion.Euler (0.0f, 0.0f, GetComponent<Rigidbody>().velocity.x * -tilt);
		
	}
}

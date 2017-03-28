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
	public Transform[] shotSpawns;
	public float fireRate;
	public GameObject missile;
	public Transform missileSpawn;
	public float MissileCooldown;
	private GameController gameController;
	private float nextFire;
	private float nextMissile;
	private int missileShot = -1;
	public int numberOfSpawns;

	void Start () {
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");	//Finding game object that holds gamecontroller script
		if (gameControllerObject != null) {
			gameController = gameControllerObject.GetComponent <GameController>();	//set reference to game controller component
		}
		if (gameControllerObject == null) {
			Debug.Log ("Cannot find 'GameController' script");	//in the case there is no reference object
		}
		numberOfSpawns = 0;
	}

	public void addShotSpawn(){

		if (numberOfSpawns < 6) {
			numberOfSpawns++;
			print ("number of SPawns = " + numberOfSpawns);
		}
	}

	void Update ()
	{
		//if mouse button is pressed instantiate the bolt and play shooting sound
		if (Input.GetButton("Fire1") && Time.time > nextFire) 
		{
			nextFire = Time.time + fireRate;
			switch (numberOfSpawns) {
			case 1:
				Instantiate (shot, shotSpawns[0].position, shotSpawns[0].rotation);
				Instantiate (shot, shotSpawns[1].position, shotSpawns[1].rotation);
				break;
			case 2:
				Instantiate (shot, shotSpawns [0].position, shotSpawns [0].rotation);
				Instantiate (shot, shotSpawns [1].position, shotSpawns [1].rotation);
				Instantiate (shot, shotSpawns [2].position, shotSpawns [2].rotation);
				break;
			case 3:
				Instantiate (shot, shotSpawns [0].position, shotSpawns [0].rotation);
				Instantiate (shot, shotSpawns [1].position, shotSpawns [1].rotation);
				Instantiate (shot, shotSpawns [2].position, shotSpawns [2].rotation);
				Instantiate (shot, shotSpawns [3].position, shotSpawns [3].rotation);
				break;
			case 4:
				Instantiate (shot, shotSpawns [0].position, shotSpawns [0].rotation);
				Instantiate (shot, shotSpawns [1].position, shotSpawns [1].rotation);
				Instantiate (shot, shotSpawns [2].position, shotSpawns [2].rotation);
				Instantiate (shot, shotSpawns [3].position, shotSpawns [3].rotation);
				Instantiate (shot, shotSpawns [4].position, shotSpawns [4].rotation);
				break;
			case 5:
				Instantiate (shot, shotSpawns [0].position, shotSpawns [0].rotation);
				Instantiate (shot, shotSpawns [1].position, shotSpawns [1].rotation);
				Instantiate (shot, shotSpawns [2].position, shotSpawns [2].rotation);
				Instantiate (shot, shotSpawns [3].position, shotSpawns [3].rotation);
				Instantiate (shot, shotSpawns [4].position, shotSpawns [4].rotation);
				Instantiate (shot, shotSpawns [5].position, shotSpawns [4].rotation);
				break;
			case 6:
				Instantiate (shot, shotSpawns [0].position, shotSpawns [0].rotation);
				Instantiate (shot, shotSpawns [1].position, shotSpawns [1].rotation);
				Instantiate (shot, shotSpawns [2].position, shotSpawns [2].rotation);
				Instantiate (shot, shotSpawns [3].position, shotSpawns [3].rotation);
				Instantiate (shot, shotSpawns [4].position, shotSpawns [4].rotation);
				Instantiate (shot, shotSpawns [5].position, shotSpawns [4].rotation);
				Instantiate (shot, shotSpawns [6].position, shotSpawns [4].rotation);
				break;
			default:
				Instantiate (shot, shotSpawns[0].position, shotSpawns[0].rotation);
				break;
			}	

			GetComponent<AudioSource>().Play ();
		}
		if (Input.GetButton("Fire2") && Time.time > nextMissile) 
		{
			nextMissile = Time.time + MissileCooldown;

			//if the user has no missiles then cant fire missiles
			if (gameController.getMissleCount() == 0) {
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

	public void startToggleCollider(){
		StartCoroutine (toggleCollider ());
	}


	IEnumerator toggleCollider(){
		GetComponent<Collider> ().enabled = false;
		yield return new WaitForSeconds (3f);
		GetComponent<Collider> ().enabled = true;
	}
		
}

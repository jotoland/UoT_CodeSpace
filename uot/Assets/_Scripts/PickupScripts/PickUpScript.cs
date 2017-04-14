using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * Andrew Salopek 3/6/17
 * PickUp Script
 * This script is to be placed on your pick up game object (prefab)
 * The pick up should be tagged to be on your layer for what your pick up does
 * i.e. lives, guns, points, etc
 * 
*/
public class PickUpScript : MonoBehaviour {
	private int MissilePickUp = 1;

	//instances of other classes
	private GameController gameController;
	private PlayerController playerController;	


	//public to make each pick up behave in a unique way
	public bool isAnimated = false;
	public bool isRotating = false;
	public bool isScaling = false;
	//public bool isFloating = false;

	//values for rupees
	public int rupeeValue;
	private int heartValue;


	//rotating variables
	public Vector3 rotationAngle;
	public float rotationSpeed;
	public float speed;

	/*	//floating variables
	public float floatSpeed;
	private bool goingUp = true;
	public float floatRate;
	private float floatTimer;
	*/   
	//scaling variables
	public Vector3 startScale;
	public Vector3 endScale;
	private bool scalingUp = true;
	public float scaleSpeed;
	public float scaleRate;
	private float scaleTimer;
	private GameObject playerControllerObject;

	// Use this for initialization
	void Start () {

		//heart values are set in stone
		heartValue = 1;
		///checking that we can access the instances of the other classes
		GameObject gameControllerObject = GameObject.FindGameObjectWithTag ("GameController");
		playerControllerObject = GameObject.FindGameObjectWithTag ("Player");
		//leave as seperate if statements
		if (gameControllerObject != null) {
			gameController = gameControllerObject.GetComponent <GameController>();
		}
		if (playerControllerObject != null) {														
			playerController = playerControllerObject.GetComponent <PlayerController>();			
		}
		GetComponent<Rigidbody>().velocity = transform.forward * speed;
	}

	//this is the method that is called when a collider enters another collider
	public void OnTriggerEnter(Collider other){
		//if the other object (not the one the script is on) is the player then let the player pick it up!
		if (other.tag == "Player") {
			print (other.tag);
			if (isScaling) {
				isScaling = false;
			}
			GetComponent<AudioSource>().Play ();
			GetComponent<Rigidbody> ().position = new Vector3 (gameObject.transform.position.x, -20, gameObject.transform.position.z);

			//if it is tagged RUPEE then do this
			if (gameObject.tag == "Rupee") {
				//update GUI with rupee count
				gameController.AddRupees (rupeeValue);
			} else if (gameObject.tag == "PowerStar") {									
				//instantiate another shotspawn											
				playerController.addShotSpawn ();										
			} else if (gameObject.tag == "OneUpHeart") {
				//udpate GUI with current life count
				gameController.AddLife (heartValue);
			} else if (gameObject.tag == "PickUp") {
				gameController.AddMissiles (MissilePickUp);
			}

			Object.Destroy (gameObject, 3f);

		}
	}


	// Update is called once per frame (this is where the animation takes place)
	void Update () {
		if(isAnimated){
			if(isRotating){
				transform.Rotate(rotationAngle * rotationSpeed * Time.deltaTime);
			}
			/*			if(isFloating){
                floatTimer += Time.deltaTime;
                Vector3 moveDir = new Vector3(0.0f, 0.0f, floatSpeed);
                transform.Translate(moveDir);
                if (goingUp && floatTimer >= floatRate){
                    goingUp = false;
                    floatTimer = 0;
                    floatSpeed = -floatSpeed;
                }else if(!goingUp && floatTimer >= floatRate){
                    goingUp = true;
                    floatTimer = 0;
                    floatSpeed = +floatSpeed;
                }
            }
*/
			if(isScaling){
				scaleTimer += Time.deltaTime;
				if (scalingUp){
					transform.localScale = Vector3.Lerp(transform.localScale, endScale, scaleSpeed * Time.deltaTime);
				}else if (!scalingUp){
					transform.localScale = Vector3.Lerp(transform.localScale, startScale, scaleSpeed * Time.deltaTime);
				}
				if(scaleTimer >= scaleRate){
					if (scalingUp) { scalingUp = false; }
					else if (!scalingUp) { scalingUp = true; }
					scaleTimer = 0;
				}
			}
		}
		if (playerControllerObject == null) {
			playerControllerObject = GameObject.FindGameObjectWithTag ("Player");
			if (playerControllerObject != null) {														
				playerController = playerControllerObject.GetComponent <PlayerController>();			
			}
		}
	}


}
//finito
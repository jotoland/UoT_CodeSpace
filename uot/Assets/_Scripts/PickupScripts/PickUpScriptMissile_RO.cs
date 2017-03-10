using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PickUpScriptMissile_RO : MonoBehaviour {

    public bool isAnimated = false;
    public bool isRotating = false;
	private int MissilePickUp = 1;
	private GameController gameController;
	public AudioClip pickUpMissile;
    //public bool isFloating = false;
    //public bool isScaling = false;

    public Vector3 rotationAngle;
    public float rotationSpeed;
	public float speed;
/*
    public float floatSpeed;
    private bool goingUp = true;
    public float floatRate;
    private float floatTimer;
   
    public Vector3 startScale;
    public Vector3 endScale;

    private bool scalingUp = true;
    public float scaleSpeed;
    public float scaleRate;
    private float scaleTimer;
*/
	// Use this for initialization
	void Start () {
		GameObject gameControllerObject = GameObject.FindGameObjectWithTag ("GameController");
		if (gameControllerObject != null) {
			gameController = gameControllerObject.GetComponent <GameController>();
		}
		GetComponent<Rigidbody>().velocity = transform.forward * speed;
	}

	public void OnTriggerEnter(Collider other){
		if (other.tag == "Player") {
			gameController.AddMissiles (MissilePickUp);
			//AudioSource.PlayClipAtPoint (pickUpMissile, Camera.main.transform.position);
			Destroy (gameObject);
		}
	}

	// Update is called once per frame
	void Update () {
        if(isAnimated){
            if(isRotating){
                transform.Rotate(rotationAngle * rotationSpeed * Time.deltaTime);
            }
/*            if(isFloating){
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
            */
        }
	}


}

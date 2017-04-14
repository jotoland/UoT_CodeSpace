using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Created By Richard O'Neal 04/17/207
// detects if the player has shot the boss 
// if so deduct health if health = 0 then destroy the game object
public class Lvl05BossHealth : MonoBehaviour {

	private int Health;

	void Start(){
		Health = 20;
	}
	void OnTriggerEnter(Collider other) 
	{
		if(other.CompareTag("Bolt"))
			{
			if (Health != 0) {
				Health = Health - 1;
				Debug.Log ("Health deducted = " + Health);
			}

				
			}

	}

	void Update(){
		print (Health);
		if(Health == 0)
		{
			GetComponent<Mover> ().enabled = false;
			GetComponent<EvasiveManeuver> ().enabled = false;
			GetComponent<WeaponController> ().enabled = false;
			GameObject.Find ("GameController").GetComponent<Level_02> ().setBOSS_IS_DEAD (true);
			GetComponent<Rigidbody> ().position = new Vector3 (gameObject.transform.position.x, -20, gameObject.transform.position.z);
			Destroy (this, 3f);
		}
	}
	public int getHealth(){
		return Health;
	}
}

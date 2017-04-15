using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Created By Richard O'Neal 04/17/2017
// detects if the player has shot the boss 
// if so deduct health if health = 0 then destroy the game object
public class Lvl05BossHealth : MonoBehaviour {

	public int Health;
	void OnTriggerEnter(Collider other) 
	{
		if(other.CompareTag("Bolt"))
			{
				Health -= 1;
				Debug.Log ("Health deducted");

			}
			if(Health == 0)
			{
				DestroyObject(gameObject);
			}
	}
}

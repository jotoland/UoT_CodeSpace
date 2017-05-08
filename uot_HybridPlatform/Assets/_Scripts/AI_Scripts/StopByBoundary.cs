using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// created by Richard O'Neal 04/12/2017
// so the boss knows when he should stop and begin fighting the player
public class StopByBoundary : MonoBehaviour {

	void OnTriggerEnter(Collider other) 
	{
		GameObject boss = GameObject.FindGameObjectWithTag("Lvl05Boss");
		if(other.tag == ("Lvl05Boss"))
			{
			Debug.Log ("Print");
			boss.GetComponent<Mover> ().enabled = false;
			boss.GetComponent<Rigidbody> ().velocity = transform.forward * 0;
			boss.GetComponent<EvasiveManeuver>().enabled = true;
			boss.GetComponent<WeaponController>().enabled = true;
			//boss.GetComponent<Lvl05BossHealth>().enabled = true;

			}
	}
}

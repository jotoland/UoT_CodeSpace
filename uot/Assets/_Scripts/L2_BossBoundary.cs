using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Richard O'Neal 4/13/2017
//Modified from StopByBoundary to use on Lvl2 Gerard Fierro 4/13/2017
public class L2_BossBoundary : MonoBehaviour {
	private bool LOOKING_FOR_BOSS;

	void Start(){
		LOOKING_FOR_BOSS = true;
	}

	void OnTriggerEnter(Collider other) 
	{
		GameObject boss = GameObject.FindGameObjectWithTag("BossLvl_02");
		if (LOOKING_FOR_BOSS) {
			GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ().setCAN_FIRE (true);
		}

		if(other.tag == ("BossLvl_02"))
		{
			if (LOOKING_FOR_BOSS) {
				boss.GetComponent<Mover> ().enabled = false;
				boss.GetComponent<Rigidbody> ().velocity = transform.forward * 0;
				boss.GetComponent<EvasiveManeuver> ().enabled = true;
				boss.GetComponent<WeaponController> ().enabled = true;
				boss.GetComponent<Lvl05BossHealth> ().enabled = true;
				LOOKING_FOR_BOSS = false;
			}

		}

	}
}

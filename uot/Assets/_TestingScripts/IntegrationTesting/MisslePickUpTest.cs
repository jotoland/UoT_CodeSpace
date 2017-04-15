using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* John G. Toland 04/14/17
 * Integration Test: Picking Up Missle Pickups
 * This test is successful when a player picks up
 * a missle pickup and the missile count increments by 1.
 * There is only one missle pickup avaliable in this test.
 * */
public class MisslePickUpTest : MonoBehaviour {
	GameController gc;

	void Start(){
		gc = GameObject.Find ("GameController").GetComponent <GameController> ();
		gc.setTest ();
		gc.clearValues ();
	}

	void OnTriggerEnter(Collider other){
		if (gc.getMissleCount () == 1) {
			IntegrationTest.Pass (this.gameObject);
			Debug.Log (IntegrationTest.passMessage);
		}else {
			Debug.Log (IntegrationTest.failMessage);
		}
		outComeInt (1, gc.getMissleCount ());
		gc.clearValues ();
	}

	public void outComeString(string expected, string actual){
		Debug.Log ("Expected: [" + expected + "] Actual: [" + actual+"]");
	}

	public void outComeInt(int expected, int actual){
		Debug.Log ("Expected: [" + expected + "] Actual: [" + actual+"]");
	}
}
//finito

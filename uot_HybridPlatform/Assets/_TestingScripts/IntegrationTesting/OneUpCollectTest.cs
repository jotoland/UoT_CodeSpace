using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* John G. Toland 04/14/17
 * Integration Test: Collecting Lives
 * This test is successful when the player collects One Up pickups 
 * and there lives incremenets by 1. In this test there are 4 One 
 * Up pickups avaliable.
 * */
public class OneUpCollectTest : MonoBehaviour {
	GameController gc;

	void Start(){
		gc = GameObject.Find ("GameController").GetComponent <GameController> ();
		gc.setTest ();
		gc.clearValues ();
	}

	void OnTriggerEnter(Collider other){
		if (gc.getLives () == 4) {
			IntegrationTest.Pass (this.gameObject);
			Debug.Log (IntegrationTest.passMessage.ToString ());
		}else {
			Debug.Log (IntegrationTest.failMessage);
		}
		outComeInt (4, gc.getLives ());
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

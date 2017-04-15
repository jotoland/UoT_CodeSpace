using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RupeeCollectedTest : MonoBehaviour {
	GameController gc;

	void Start(){
		gc = GameObject.Find ("GameController").GetComponent <GameController> ();
		gc.setTest ();
		gc.clearValues ();
	}

	void OnTriggerEnter(Collider other){
		if (gc.getRupeeCount () == 16) {
			if (gc.getRupeeIntervalValue () == 0) {
				IntegrationTest.Pass (this.gameObject);
				Debug.Log (IntegrationTest.passMessage);
				Debug.Log ("Update DB Threshold : 10"); 
				Debug.Log ("Rupees Test Passed: RupeeCount = " + gc.getRupeeCount () 
					+ " Rupee Interval Value = " + gc.getRupeeIntervalValue ());
			} else {
				Debug.Log (IntegrationTest.failMessage);
			}
		}else {
			Debug.Log (IntegrationTest.failMessage);
		}
		outComeInt (16, gc.getRupeeCount ());
	}

	public void outComeString(string expected, string actual){
		Debug.Log ("Expected: " + expected + " Actual: " + actual);
	}

	public void outComeInt(int expected, int actual){
		Debug.Log ("Expected: " + expected + " Actual: " + actual);
	}

}

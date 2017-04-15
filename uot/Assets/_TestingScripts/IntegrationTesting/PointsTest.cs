using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsTest : MonoBehaviour {
	GameController gc;

	void Start(){
		gc = GameObject.Find ("GameController").GetComponent <GameController> ();
		gc.setTest ();
		gc.clearValues ();
	}

	void OnTriggerEnter(Collider other){
		if (gc.getScore () == 60) {
			if (gc.getScoreIntervalValue () == 0) {
				IntegrationTest.Pass (this.gameObject);
				Debug.Log (IntegrationTest.passMessage);
				Debug.Log ("Update DB Threshold : 50"); 
				Debug.Log ("Points Test Passed: Score = " + gc.getScore () 
					+ " Score Interval Value = " + gc.getScoreIntervalValue ());
			} else {
				Debug.Log (IntegrationTest.failMessage);
			}
		}else {
			Debug.Log (IntegrationTest.failMessage);
		}
		outComeInt (60, gc.getScore ());
	}

	public void outComeString(string expected, string actual){
		Debug.Log ("Expected: " + expected + " Actual: " + actual);
	}

	public void outComeInt(int expected, int actual){
		Debug.Log ("Expected: " + expected + " Actual: " + actual);
	}
}

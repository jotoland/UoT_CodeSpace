using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotSpawnCollectedTest : MonoBehaviour {
	PlayerController pc;

	void Start(){
		pc = GameObject.FindGameObjectWithTag("Player").GetComponent <PlayerController> ();
	}

	void OnTriggerEnter(Collider other){
		if (pc.numberOfSpawns == 3) {
			IntegrationTest.Pass (this.gameObject);
			Debug.Log (IntegrationTest.passMessage);

		}else {
			Debug.Log (IntegrationTest.failMessage);
		}
		outComeInt (3, pc.numberOfSpawns);
	}

	public void outComeString(string expected, string actual){
		Debug.Log ("Expected: " + expected + " Actual: " + actual);
	}

	public void outComeInt(int expected, int actual){
		Debug.Log ("Expected: " + expected + " Actual: " + actual);
	}
}

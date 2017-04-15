using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* John G. Toland 04/14/17
 * Integration Test: Collecting Shot Spawn Pickups
 * This test is successful when a shot spawn pickup is collected
 * and the number of shot spawns avaliable to the player incremments by one
 * in this test the number of shot spawns avaliable is 3
 * */
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
		Debug.Log ("Expected: [" + expected + "] Actual: [" + actual+"]");
	}

	public void outComeInt(int expected, int actual){
		Debug.Log ("Expected: [" + expected + "] Actual: [" + actual+"]");
	}
}
//finito
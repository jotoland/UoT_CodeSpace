using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* John G. Toland 04/14/17
 * Integration Test: Player Loses a Life by Enemy Fire
 * This test is successful when a player loses a life.
 * In this test, the player has 2 lives in the beginning
 * and recieves enemy fire. 
 * */
public class LifeLostTest : MonoBehaviour {
	GameController gc;
	// Use this for initialization
	void Start () {
		gc = GameObject.Find ("GameController").GetComponent<GameController> ();
	}

	void OnDestroy(){
			if (gc.getLives () == 1) {
				IntegrationTest.Pass (this.gameObject);
				outComeInt (1, gc.getLives ());
			}
	}

	public void outComeInt(int expected, int actual){
		Debug.Log ("Expected: [" + expected + "] Actual: [" + actual+"]");
	}
}
//finito

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* John G. Toland 4/14/17
 * Integration Test: Spawning Rupee after Hazard is destroyed
 * Tests is successful when a rupee spawns
 * */
public class SpawningRupeeTest : MonoBehaviour {
	private GameObject rupee;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		rupee = GameObject.FindGameObjectWithTag("Rupee");
		if (rupee) {
			IntegrationTest.Pass (this.gameObject);
			outComeString ("Rupee", rupee.tag);
		}
	}

	public void outComeString(string expected, string actual){
		Debug.Log ("Expected: [" + expected + "] Actual: [" + actual+"]");
	}
}
//finito
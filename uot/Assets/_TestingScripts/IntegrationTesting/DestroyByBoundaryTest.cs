using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByBoundaryTest : MonoBehaviour {
	private bool inBoundary;
	Collider boundary;

	// Use this for initialization
	void Start () {
		inBoundary = false;
	}

	void OnTriggerEnter(Collider other){
		boundary = other;
		if(other.CompareTag ("Boundary")){
			inBoundary = true;
		}
	}

	void OnDestroy(){
		if (inBoundary) {
			IntegrationTest.Pass (this.gameObject);
			outComeString ("Boundary", boundary.tag);
		}
	}

	public void outComeString(string expected, string actual){
		Debug.Log ("Expected: [" + expected + "] Actual: [" + actual+"]");
	}
		
}

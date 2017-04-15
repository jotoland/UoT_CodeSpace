using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningRupeeTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		GameObject rupee = GameObject.FindGameObjectWithTag("Rupee");
		if (rupee) {
			IntegrationTest.Pass (this.gameObject);
		}
	}
}

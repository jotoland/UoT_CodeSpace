using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeLostTest : MonoBehaviour {
	GameController gc;
	// Use this for initialization
	void Start () {
		gc = GameObject.Find ("GameController").GetComponent<GameController> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnDestroy(){
		if (gc.isGameOver ()) {
			if (gc.getLives () == 1) {
				IntegrationTest.Pass (this.gameObject);
			}
		}
	}
}

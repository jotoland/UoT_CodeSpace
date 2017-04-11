using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* John G. Toland 4/10/17 Random Rotator Script
 * Added the ablility to stop the rotation when a player switches scenes
 * */
public class RandomRotator : MonoBehaviour {

	public float tumble;
	private PauseNavGUI pB;
	private GameController gc;

	// Use this for initialization
	void Start () {
		GameObject pBObject = GameObject.FindGameObjectWithTag ("PauseBtn");
		if (pBObject != null) {
			pB = pBObject.GetComponent <PauseNavGUI> ();
		}
		GameObject gcObject = GameObject.FindGameObjectWithTag ("GameController");
		if (gcObject != null) {
			gc = gcObject.GetComponent <GameController> ();
		}
		GetComponent<Rigidbody> ().angularVelocity = Random.insideUnitSphere * tumble;
	}

	void Update(){
		if(!gc.isGameOver ()){
			if (pB.isLEFT_SCENE ()) {
				tumble = 0;
				pB.setLEFT_SCENE (true);
				GetComponent<Rigidbody> ().angularVelocity = Random.insideUnitSphere * tumble;
			}
		}
	}

}
//finito

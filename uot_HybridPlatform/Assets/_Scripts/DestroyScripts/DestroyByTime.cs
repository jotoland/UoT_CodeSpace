using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* John G. Toland 4/10/17 Destroy By Time Script
 * Ensures that gameObjects do not live forever in the scene
 * Added the ability to destroy this object when payer leaves the scene
 * */
public class DestroyByTime : MonoBehaviour {

	//lifetime of gameObjects
	public float lifeTime;
	private PauseNavGUI pB;
	private GameController gc;

	void Start(){
		GameObject pBObject = GameObject.FindGameObjectWithTag ("PauseBtn");
		if (pBObject != null) {
			pB = pBObject.GetComponent <PauseNavGUI> ();
		}

		GameObject gcObject = GameObject.FindGameObjectWithTag ("GameController");
		if (gcObject != null) {
			gc = gcObject.GetComponent <GameController> ();
		}

		Destroy (gameObject, lifeTime);
	}

	void Update(){
		if (!gc.isGameOver ()) {
			if(pB.isLEFT_SCENE ()){
				Destroy (gameObject, 0);
				pB.setLEFT_SCENE (true);
			}
		}
	}
}

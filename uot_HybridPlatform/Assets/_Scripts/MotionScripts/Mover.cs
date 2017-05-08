//Richard O'Neal 2/16/2017
/* John G. Toland 4/10/17
 * Added the scaling algorithm and the ablility to control it
 * Also added the ablility to shot the hazards from moving when a player switches levels from menu
 * */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour {

	public float speed;
	private PauseNavGUI pB;
	private GameController gc;
	private LevelScript_01 lvl_01;
	public bool isGoinToPulse;
	public Material pulseMaterial;

	public bool isScaling = false;
	public Vector3 startScale;
	public Vector3 endScale;
	private bool scalingUp = true;
	public float scaleSpeed;
	public float scaleRate;
	private float scaleTimer;

	private GameObject childModel;
	private MeshRenderer thisRender;
	private Material thisMaterial;
	private int scaleCount = 0;

	void Start (){

		if (isGoinToPulse) {
			childModel = this.transform.GetChild (0).gameObject;
			thisRender = childModel.GetComponent<MeshRenderer> ();
			thisMaterial = thisRender.material;
		}
			
		GameObject pBObject = GameObject.FindGameObjectWithTag ("PauseBtn");
		if (pBObject != null) {
			pB = pBObject.GetComponent <PauseNavGUI> ();
		}

		GameObject gcObject = GameObject.FindGameObjectWithTag ("GameController");
		if (gcObject != null) {
			gc = gcObject.GetComponent <GameController> ();
			lvl_01 = gc.GetComponent<LevelScript_01> ();
		}

		if (this.gameObject.name == "Asteroid_L1_16(Clone)" || 
			this.gameObject.name == "Asteroid_L1_17(Clone)" || 
			this.gameObject.name == "Asteroid_L1_18(Clone)") {
			if (lvl_01.isScaling ()) {
				isScaling = true;
			}
		}
		GetComponent<Rigidbody> ().velocity = transform.forward * speed;
	}

	void Update(){
		if(!gc.isGameOver ()){
			if(pB.isLEFT_SCENE ()){
				speed = 0;
				pB.setLEFT_SCENE (true);
				GetComponent<Rigidbody> ().velocity = transform.forward * speed;
			}
		}
		if(isScaling){
			scaleTimer += Time.deltaTime;
			if (scalingUp){
				if (isGoinToPulse && scaleCount != 1) {
					thisRender.material = pulseMaterial;
				}
				if (isGoinToPulse && scaleCount == 1) {
					isScaling = false;
				}
				transform.localScale = Vector3.Lerp(transform.localScale, endScale, scaleSpeed * Time.deltaTime);
			}else if (!scalingUp){
				if (isGoinToPulse) {
					thisRender.material = thisMaterial;
				}
				transform.localScale = Vector3.Lerp(transform.localScale, startScale, scaleSpeed * Time.deltaTime);
			}
			if(scaleTimer >= scaleRate){
				if (scalingUp) { scalingUp = false; }
				else if (!scalingUp) { scalingUp = true; scaleCount++;}
				scaleTimer = 0;
			}
		}
	}

}
//finito

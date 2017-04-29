using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnimationScript : MonoBehaviour {

	private Scene curScene;
	private Animator aniPlay;
	private GameObject[] shipList;
	private PlayerController pCon;
	private GameObject bound;
	public bool playExit = false;


	void Awake(){
		aniPlay = this.gameObject.GetComponent<Animator> ();
		aniPlay.enabled = false;
		bound = GameObject.Find ("Boundary");
		bound.SetActive (false);

	}
	// Use this for initialization
	void Start () {
		curScene = SceneManager.GetActiveScene ();
		if (curScene.name.Equals ("Level_01")) {
			print ("level one ani play");
			shipList = new GameObject[transform.childCount];
			for(int i =0; i<transform.childCount-2; i++){
				shipList [i] = transform.GetChild (i).gameObject;
			}
			for(int i =0; i<shipList.Length-2; i++){
				pCon = shipList [i].GetComponent<PlayerController> ();
				pCon.enabled = false;
			}
			transform.GetChild(shipList.Length-1).gameObject.SetActive (true);
			aniPlay.enabled = true;
			StartCoroutine (EnableControls());
		}
	}

	public void playExitAni(){
		bound.SetActive (false);
		for(int i =0; i<shipList.Length-2; i++){
			pCon = shipList [i].GetComponent<PlayerController> ();
			pCon.enabled = false;
		}
		transform.GetChild(shipList.Length-1).gameObject.SetActive (true);
		aniPlay.enabled = true;
		aniPlay.SetTrigger ("GameWon");
	}

	// Update is called once per frame
	void Update () {
		if (playExit) {
			playExit = false;
			for(int i =0; i<shipList.Length-2; i++){
				pCon = shipList [i].GetComponent<PlayerController> ();
				pCon.enabled = false;
			}
			transform.GetChild(shipList.Length-1).gameObject.SetActive (true);
			aniPlay.enabled = true;
			aniPlay.SetTrigger ("GameWon");
		}
		
	}

	IEnumerator EnableControls(){
		yield return new WaitForSecondsRealtime (5f);
		aniPlay.enabled = false;
		for(int i =0; i<shipList.Length-2; i++){
			pCon = shipList [i].GetComponent<PlayerController> ();

			pCon.enabled = true;
			bound.SetActive (true);
		}
		transform.GetChild(shipList.Length-1).gameObject.SetActive (false);
	}
}

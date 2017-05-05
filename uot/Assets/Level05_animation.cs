using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//CReated by Richard O'Neal 5/4/2017
public class Level05_animation : MonoBehaviour {
	
	private GameObject[] shipList;
	private GameObject boundary;
	private PlayerController shipControl;
	// Use this for initialization
	void Start () {
		StartCoroutine (AnimationFinish (4));
		boundary = GameObject.Find ("Boundary");
		boundary.SetActive (false);
		shipList = new GameObject[transform.childCount];
		for(int i =0; i<transform.childCount-2; i++){
			shipList [i] = transform.GetChild (i).gameObject;
		}
		for(int i =0; i<shipList.Length-2; i++){
			shipControl = shipList [i].GetComponent<PlayerController> ();
			shipControl.enabled = false;
		}
	}

	IEnumerator AnimationFinish(float time)
	{
		yield return new WaitForSeconds(time);
		Debug.Log ("AnimationFinish");
		//boundary = GameObject.Find ("Boundary");
		boundary.SetActive (true);
		for(int j =0; j<transform.childCount-1; j++){
			shipList [j] = transform.GetChild (j).gameObject;
		}
		for(int j =0; j<shipList.Length-1; j++){
			shipControl = shipList [j].GetComponent<PlayerController> ();
			shipControl.enabled = true;
		}
		// Code to execute after the delay
	}
	// Update is called once per frame

}

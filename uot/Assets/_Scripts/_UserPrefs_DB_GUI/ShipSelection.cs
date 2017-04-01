using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/****
 *John G Toland   2/5/17
 * This is the script for the ShipList,
 * allows the user to increment through an array of ship prefabs
 * This also calls a coroutine that updates the database
 * with the users choice of ship
*/
public class ShipSelection : MonoBehaviour {

	//variables for navigation through array
	private int index;
	public GameObject[] shipList;
	//variables for database communication 
	public string userName;
	public int lvlOne = 1;
	//CoRo instance
	public CoRoutines CoRo;
	private int connection;

	// Use this for initialization
	void Start () {

		GameObject CoRoObject = GameObject.FindGameObjectWithTag ("CoRoutines");
		//checking that we can acces our instance
		if (CoRoObject != null) {
			CoRo = CoRoObject.GetComponent <CoRoutines> ();
		}
		//getting the user name from player prefs instead of DB this time
		userName = PlayerPrefs.GetString ("mUserName");
		connection = PlayerPrefs.GetInt ("mConnection");
		//getting the count of child objects inside the shiplist object located in the hierachy
		shipList = new GameObject[transform.childCount];
		//Fill the array with our models
		for(int i =0; i<transform.childCount; i++){
			shipList [i] = transform.GetChild (i).gameObject;
		}

		//We toggle off their renderers for the start
		foreach (GameObject go in shipList) {
			go.SetActive (false);
		}
		//get data from the data base used in the level scenes
		//necessary for here instead of writing another script that is very similar
		//Getting the currently loaded scene using the SceneManager.
		Scene currentScene = SceneManager.GetActiveScene ();
		if (connection == 1 && currentScene.name != "PlayerSelection") {
			CoRo.GetData (shipList, index, userName);
		} else {
			
			if (currentScene.name == "PlayerSelection") {
				shipList [index].SetActive (true);
			} else {
				shipList [PlayerPrefs.GetInt ("mShip")].SetActive (true);
			}
		}
	}

	#region ToggleButtons
	//when user is toggling left
	public void ToggleLeft(){
		//Toggle off current model
		shipList[index].SetActive(false);
		index--;
		if (index < 0) 
			index = shipList.Length - 1;
		shipList[index].SetActive(true);
	}
	//when user is toggling right
	public void ToggleRight(){
		//Toggle off current model
		shipList[index].SetActive(false);
		index++;
		if (index == shipList.Length) 
			index = 0;
		shipList[index].SetActive(true);
	}
	//confirmation button
	public void ConfirmButton(){
		//update database with seleciton
		if (connection == 1) {
			CoRo.UpdateShipSelect (lvlOne, index, userName);
		} else {
			PlayerPrefs.SetInt ("mLevel", lvlOne);
			PlayerPrefs.SetInt ("mShip", index);
			SceneManager.LoadScene ("Level_01");
		}
	}
	#endregion

}
//finito

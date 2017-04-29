using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ScoreManager : MonoBehaviour {

	int changeCounter = 0;
	Dictionary<string, Dictionary<string, int> > playerScores;

	void Awake(){
		
	}
	// Use this for initialization
	void Start () {
		SetScore ("jack", "Points", 9001);
		SetScore ("jack", "Lives", 9);
		SetScore ("jack", "Rupees", 42000); 
		SetScore ("bob", "Points", 1);
		SetScore ("bob", "Lives", 3);
		SetScore ("bob", "Rupees", 76989); 
		SetScore ("tim", "Points", 98);
		SetScore ("tim", "Lives", 4);
		SetScore ("tim", "Rupees", 0); 
		SetScore ("AAA", "Rupees", 0); 
		SetScore ("BBB", "Rupees", 0); 
		SetScore ("CCC", "Rupees", 0);
		SetScore ("jac", "Points", 9001);
		SetScore ("jac", "Lives", 9);
		SetScore ("jac", "Rupees", 42000); 
		SetScore ("bo", "Points", 1);
		SetScore ("bo", "Lives", 3);
		SetScore ("bo", "Rupees", 76989); 
		SetScore ("ti", "Points", 98);
		SetScore ("ti", "Lives", 4);
		SetScore ("ti", "Rupees", 0); 
		SetScore ("AA", "Rupees", 0); 
		SetScore ("BB", "Rupees", 0); 
		SetScore ("CC", "Rupees", 0);
		SetScore ("ja", "Points", 9001);
		SetScore ("ja", "Lives", 9);
		SetScore ("ja", "Rupees", 42000); 
		SetScore ("b", "Points", 1);
		SetScore ("b", "Lives", 3);
		SetScore ("b", "Rupees", 76989); 
		SetScore ("t", "Points", 98);
		SetScore ("t", "Lives", 4);
		SetScore ("t", "Rupees", 0); 


		print(GetScore ("jack", "Kills"));
		//playerScores ["johnny5"] = new Dictionary<string, int> ();
	}

	void Init(){
		if (playerScores != null) {
			return;
		}
		playerScores = new Dictionary<string, Dictionary<string, int> > ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public int GetScore(string username, string scoreType){
		Init ();
		if (playerScores.ContainsKey (username) == false) {
			return 0;
		}
		if (playerScores [username].ContainsKey (scoreType) == false) {
			return 0;
		}

		return playerScores [username] [scoreType];
	}

	public void SetScore(string username, string scoreType, int value){
		Init ();
		changeCounter++;
		if (playerScores.ContainsKey (username) == false) {
			playerScores [username] = new Dictionary<string, int> ();
		}
		playerScores [username] [scoreType] = value;

	}

	public void ChangeScore(string username, string scoreType, int amount){
		Init ();
		int currScore = GetScore (username, scoreType);
		SetScore (username, scoreType, currScore + amount);
	}

	public string[] GetPlayerNames(){
		Init ();
		return playerScores.Keys.ToArray ();
	}

	public string[] GetPlayerNames(string sortingScoreType){
		Init ();
		return playerScores.Keys.OrderByDescending (n => GetScore (n, sortingScoreType)).ToArray ();
	}

	public void DEBUG_ADD_KILL_TO_JACK(){
		ChangeScore ("jack", "Points", 1);
	}

	public int GetChangeCounter(){
		return changeCounter;
	}
}

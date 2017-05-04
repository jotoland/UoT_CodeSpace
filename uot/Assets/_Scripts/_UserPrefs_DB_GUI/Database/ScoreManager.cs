using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ScoreManager : MonoBehaviour {

	int changeCounter = 0;
	Dictionary<string, Dictionary<string, int> > playerScores;
	private string[] records;
	private string[] items;
	private string[] username;

	private int[] points;
	private int[] rupees;
	private int[] lives;

	void Awake(){
		
	}
	// Use this for initialization
	void Start () {
		
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
		

	public int GetChangeCounter(){
		return changeCounter;
	}

	public void ClearChangeCounter(){
		this.changeCounter = 0;
	}

	public IEnumerator GetDataCo(string userName) {

		string getDataUrl = "https://tempusfugit.000webhostapp.com/usrAc/getHSAc.php";

		WWWForm itemsData = new WWWForm ();
		itemsData.AddField ("mUserName", userName);
		itemsData.AddField ("dbServerName", "localhost");
		itemsData.AddField ("dbUserName", "id917361_johnny5isalive");
		itemsData.AddField ("dbPassword","crappy");
		itemsData.AddField ("dbName","id917361_uot");

		WWW getData = new WWW (getDataUrl, itemsData);

		yield return getData;

		string itemsDataStr = getData.text;
		//print ("Getting the HS: " +itemsDataStr);

		records = itemsDataStr.Split ('*');
		username = new string[records.Length];
		points = new int[records.Length];
		rupees = new int[records.Length];
		lives = new int[records.Length];
		int i = 0;

		foreach (string rec in records) {
			print (rec);
			if (i > 0) {
				items = rec.Split ('|');
				int j = 0;
				foreach (string it in items) {
					print (it);
					if (j > 0) {
						if (j == 1) {
							username [i - 1] = GetDataValue (it, "UserName:");
							//print (username [i -1]);
						} else if (j == 2) {
							points [i - 1] = int.Parse (GetDataValue (it, "Points:"));
							//print (points [i-1]);
						} else if (j == 3) {
							rupees [i - 1] = int.Parse (GetDataValue (it, "Rupees:"));
							//print (rupees [i-1]);
						} else if(j == 4) {
							lives [i-1] = int.Parse (GetDataValue (it, "Lives:"));
							//print (lives [i-1]);
						}
					}
					j++;
				}
			}
			i++;
		}
		int maxScoreBoardLength;
		if (username.Length-1> 15) {
			maxScoreBoardLength = 15;
		} else {
			maxScoreBoardLength = username.Length - 1;
		}
		for(int k = 0; k<maxScoreBoardLength; k++){
			SetScore (username[k], "Points", points[k]);
			SetScore (username[k], "Rupees", rupees[k]);
			SetScore (username[k], "Lives", lives[k]);
			/*
			print ("username [" + k + "]= " + username [k]);
			print ("points [" + k + "]= " + points [k]);
			print ("rupees [" + k + "]= " + rupees [k]);
			print ("lives [" + k + "]= " + lives [k]);
			*/
		}
	}

	//input is the data string and this method splits it appart
	string GetDataValue(string data, string index){
		string value = data.Substring (data.IndexOf(index)+index.Length);
		if(value.Contains("|"))value = value.Remove (value.IndexOf ("|"));
		return value;
	}
		
	void OnEnable(){
		StartCoroutine (GetDataCo (PlayerPrefs.GetString ("mUsername")));
	}
		
}
//finito

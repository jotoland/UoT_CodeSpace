using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScoreListHandler : MonoBehaviour {

	public GameObject entry;
	ScoreManager man;
	int lastChangeCounter;
	// Use this for initialization
	void Start () {
		man = GameObject.Find("ScoreManager").GetComponent<ScoreManager> ();
		lastChangeCounter = man.GetChangeCounter ();
	}
	
	// Update is called once per frame
	void Update () {
		if (man.GetChangeCounter () == lastChangeCounter) {
			return;
		}

		lastChangeCounter = man.GetChangeCounter ();

		while (this.transform.childCount > 0) {
			Transform c = this.transform.GetChild (0);
			c.SetParent (null);
			Destroy (c);
		}

		string[] names = man.GetPlayerNames ("Points");

		foreach (string name in names) {
			GameObject go = Instantiate (entry);
			go.transform.SetParent (this.transform);
			go.transform.Find ("Username").GetComponent<Text> ().text = name;
			go.transform.Find ("Lives").GetComponent<Text> ().text = man.GetScore (name, "Lives").ToString ();
			go.transform.Find ("Rupees").GetComponent<Text> ().text = man.GetScore (name, "Rupees").ToString ();
			go.transform.Find ("Points").GetComponent<Text> ().text = man.GetScore (name, "Points").ToString ();
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/**
 * John G Toland 3/5/17
 * CoRoutines to communicate with the database
 * Update data
 * Get data
 * Login
 * Create account
*/
public class CoRoutines : MonoBehaviour {
	public string[] items;
	private Login L;
	private SceneLoaderHandler SLH;
	private GameObject SLHo;

	// Use this for initialization
	void Start () {
		Scene currentScene = SceneManager.GetActiveScene ();

		GameObject LObject = GameObject.FindGameObjectWithTag ("Login");
		//make sure we can access our instance of CoRo
		if (LObject != null) {
			L = LObject.GetComponent <Login> ();
		}

		if (currentScene.name == "LoginMenu") {
			SLHo = GameObject.Find ("JOHNS_GUIbox");
			SLH = SLHo.GetComponent<SceneLoaderHandler> ();
		} else if (currentScene.name == "PlayerSelection") {
			SLHo = GameObject.Find ("ShipSelectionCanvas");
			SLH = SLHo.GetComponent<SceneLoaderHandler> ();
		}
	}

	//input is the data string and this method splits it appart
	string GetDataValue(string data, string index){
		string value = data.Substring (data.IndexOf(index)+index.Length);
		if(value.Contains("|"))value = value.Remove (value.IndexOf ("|"));
		return value;
	}

	/// the following public methods are used by the other classes to update the database.
	/// These methods start the respective CoRoutines to update recods, query records, or check the connection.
	public void UpdateShipSelect(int lvl, int index, string userName){
		StartCoroutine (UpdateShipSelectCo (lvl, index, userName));
	}

	public void GetData(string userName){
		StartCoroutine (GetDataCo (userName));
	}

	public void CreateAccount(string cUserName, string cPassword){
		StartCoroutine (CreateAccountCo (cUserName, cPassword));
	}

	public void LoginAccount(string userName, string password){
		StartCoroutine (LoginAccountCo (userName, password));
	}

	public void UpdateData(string userName, int data, string dataID){
		StartCoroutine (UpdateDataCo (userName, data, dataID));
	}

	public void CheckConnection(){
		StartCoroutine (Linus ());
	}

#region Coroutines
	//checking the conneciton The very first DB commincation when launching application called!!!!
	//Linus calls Lucy and if Lucy doesnt pick up then we proceed with offline game play!!
	public IEnumerator Linus(){
		
		string CCAccountUrl = "https://tempusfugit.000webhostapp.com/usrAc/LucyAc.php";

		//sends messages to PHP script
		WWWForm form = new WWWForm ();
		form.AddField("mConnection", "johnny5");
		form.AddField ("dbServerName", "localhost");
		form.AddField ("dbUserName", "id917361_johnny5isalive");
		form.AddField ("dbPassword","crappy");
		form.AddField ("dbName","id917361_uot");

		WWW CCWWW = new WWW (CCAccountUrl, form);

		yield return CCWWW;

		string connection = CCWWW.text;
		if (CCWWW.error != null) {
			Debug.Log ("[ello]: No Internet");
			setConnStatus (false, connection);
		} else {
			if (connection == "DB Con Success!") {
				print ("Setting Connection Status");
				setConnStatus (true, connection);
			} else if (connection == "DB Con Failed") {
				setConnStatus (false, connection);
			}
		}
	}
	/// Updates the ship select co.
	public IEnumerator UpdateShipSelectCo(int lvl, int index, string userName){
		
		string updateAccountUrl = "https://tempusfugit.000webhostapp.com/usrAc/updateAc.php";

		//sends messages to PHP script
		WWWForm form = new WWWForm ();
		//variables being sent
		form.AddField ("mUserName", userName);
		form.AddField ("mCurrentLvl", lvl);
		form.AddField ("mCurrentShip", index);
		form.AddField ("dbServerName", "localhost");
		form.AddField ("dbUserName", "id917361_johnny5isalive");
		form.AddField ("dbPassword","crappy");
		form.AddField ("dbName","id917361_uot");

		WWW updateAccountWWW = new WWW (updateAccountUrl, form);

		yield return updateAccountWWW;

		if (updateAccountWWW.error != null) {
			Debug.Log ("[ello]: Cannot connect to Update Account");
		} else {
			string updateAccountReturn = updateAccountWWW.text;
			print(updateAccountReturn);
			if (updateAccountReturn == "Success.") {
				Debug.Log ("[ello]: Success: Account update");
				SLH.LoadNewSceneString ("LevelSelection");
			}
		}
	}
	/// Gets the data co.
	public IEnumerator GetDataCo(string userName) {
		
		string getDataUrl = "https://tempusfugit.000webhostapp.com/usrAc/getRecAc.php";

		WWWForm itemsData = new WWWForm ();
		itemsData.AddField ("mUserName", userName);
		itemsData.AddField ("dbServerName", "localhost");
		itemsData.AddField ("dbUserName", "id917361_johnny5isalive");
		itemsData.AddField ("dbPassword","crappy");
		itemsData.AddField ("dbName","id917361_uot");

		WWW getData = new WWW (getDataUrl, itemsData);

		yield return getData;

		string itemsDataStr = getData.text;
		print ("[GettingData && Activating Ship]: " +itemsDataStr);

		items = itemsDataStr.Split ('|');
		items[1] =  GetDataValue (items [1], "UserName:");
		PlayerPrefs.SetString ("mUsername", items[1]);

		items[2] = GetDataValue (items [2], "Level:");
		PlayerPrefs.SetInt ("mLevel", int.Parse (items[2]));

		items[3] = GetDataValue (items [3], "Ship:");
		PlayerPrefs.SetInt ("mShip", int.Parse(items[3]));
		//print("mShip = " + PlayerPrefs.GetInt ("mShip"));

		items[4] = GetDataValue (items [4], "Points:");
		PlayerPrefs.SetString ("mPoints", items[4]);

		items[5] = GetDataValue (items [5], "Rupees:");
		PlayerPrefs.SetString ("mRupees", items[5]);

		items[6] = GetDataValue (items [6], "Lives:");
		PlayerPrefs.SetString ("mLives", items[6]);
	}
		
	/// Creates the account co.
	IEnumerator CreateAccountCo(string cUserName, string cPassword){
		print ("insideCreateAccountCo");
		string createAccountUrl = "https://tempusfugit.000webhostapp.com/usrAc/crtAc.php";

		//sends messages to PHP script
		WWWForm form = new WWWForm ();
		//variables being sent
		form.AddField ("mUserName", cUserName);
		form.AddField ("Password", cPassword);
		form.AddField ("dbServerName", "localhost");
		form.AddField ("dbUserName", "id917361_johnny5isalive");
		form.AddField ("dbPassword","crappy");
		form.AddField ("dbName","id917361_uot");

		WWW createAccountWWW = new WWW (createAccountUrl, form);

		yield return createAccountWWW;

		if (createAccountWWW.error != null) {
			Debug.Log ("[ello]: Cannot connect to Account Creation");
		} else {
			string createAccountReturn = createAccountWWW.text;
			print (createAccountReturn);
			if (createAccountReturn == "Success.") {
				L.setELm("Success: Account Created", false);
			} else {
				L.setELm(createAccountReturn, true);
			}
		}
		yield return new WaitForSeconds (1);
	}

	/// Logins the account.
	IEnumerator LoginAccountCo(string userName, string password){
		print ("inside login account");
		string loginUrl = "https://tempusfugit.000webhostapp.com/usrAc/logAc.php";
		WWWForm form = new WWWForm();

		form.AddField ("mUserName", userName);
		form.AddField ("Password", password);
		form.AddField ("dbServerName", "localhost");
		form.AddField ("dbUserName", "id917361_johnny5isalive");
		form.AddField ("dbPassword","crappy");
		form.AddField ("dbName","id917361_uot");

		WWW LoginAccountWWW = new WWW (loginUrl, form);

		yield return LoginAccountWWW;

		if (LoginAccountWWW.error != null) {
			L.setELm("[ello]: Can not connect to Login", true);
		} else {
			string loginAccountReturn = LoginAccountWWW.text;
			print (loginAccountReturn);
			if (loginAccountReturn.Contains ("Success.")) {
				yield return new WaitForSecondsRealtime (2);
				GetData (userName);
				L.setELm(loginAccountReturn, false);
				string[] logTextSplit = loginAccountReturn.Split (':'); 
				if (logTextSplit [0] == "0") {
					if (logTextSplit [1] == "Success.") {
						SLH.LoadNewSceneString ("PlayerSelection");
					}
				} else if (logTextSplit [0] == "9") {
					if (logTextSplit [1] == "Success.") {
						SLH.LoadNewSceneString ("LevelSelection");
					}
				} else{
					if (logTextSplit [1] == "Success.") {
						int level = int.Parse (logTextSplit [0]) + 2;
						print ("level = " + level);
						SLH.LoadNewSceneInt (level);
					}
				}
			} else {
				L.setELm(loginAccountReturn, true);
			}
		}
	}

	//this is the coroutine to update the users account (data) on the DB
	IEnumerator UpdateDataCo(string userName, int data, string dataID){
		
		string updateAccountUrl = "https://tempusfugit.000webhostapp.com/usrAc/updateDataAc.php";

		//sends messages to PHP script
		WWWForm form = new WWWForm ();
		//variables being sent
		form.AddField ("mUserName", userName);
		form.AddField ("mCurrentData", data);
		form.AddField ("mDataID", dataID);
		form.AddField ("dbServerName", "localhost");
		form.AddField ("dbUserName", "id917361_johnny5isalive");
		form.AddField ("dbPassword","crappy");
		form.AddField ("dbName","id917361_uot");

		WWW updateAccountWWW = new WWW (updateAccountUrl, form);

		yield return updateAccountWWW;

		if (updateAccountWWW.error != null) {
			Debug.Log ("[ello]: Cannot connect to Update Account");
		} else {
			string updateAccountReturn = updateAccountWWW.text;
			print (updateAccountReturn);
			if (updateAccountReturn == "Success.") {
				print ("[ello]: Success: DATA! updated with DataID: " + dataID);
				if (dataID.Equals ("lvl")) {
					PlayerPrefs.SetInt ("mLevel", data);
				} else if (dataID.Equals ("liv")) {
					PlayerPrefs.SetString ("mLives", data.ToString ());
					print ("data lives = " + data.ToString ());
				} else if (dataID.Equals ("pts")) {
					PlayerPrefs.SetString ("mPoints", data.ToString ());
				} else if (dataID.Equals ("rup")) {
					PlayerPrefs.SetString ("mRupees", data.ToString ());
				}
			}
		}
	}
#endregion

	//setting the connection status so the game will know wether or not ot call Lucy throughout the game
	public void setConnStatus(bool status, string con){
		Debug.Log ("[ello]: "+con);
		if (status) {
			PlayerPrefs.SetInt ("mConnection", 1);
		} else {
			PlayerPrefs.SetInt ("mConnection", 0);
		}
	}

}
//finito

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
	GUIAccount GUIobj;

	// Use this for initialization
	void Start () {
		GUIobj = Camera.main.GetComponent<GUIAccount> ();
	}

	//input is the data string and this method splits it appart
	string GetDataValue(string data, string index){
		string value = data.Substring (data.IndexOf(index)+index.Length);
		if(value.Contains("|"))value = value.Remove (value.IndexOf ("|"));
		return value;
	}
	/// <summary>
	/// the following public methods are used by the other classes to update the database.
	/// These methods start the respective CoRoutines to update recods, query records, or check the connection.
	public void UpdateShipSelect(int lvl, int index, string userName){
		StartCoroutine (UpdateShipSelectCo (lvl, index, userName));
	}

	public void GetData(GameObject[] shipList, int index, string userName){
		StartCoroutine (GetDataCo (shipList, index, userName));
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
	/// <summary>
	/// Linus this instance.
	/// </summary>
	public IEnumerator Linus(){
		string CCAccountUrl = "https://tempusfugit.000webhostapp.com/usrAc/LucyAc.php";
		//sends messages to PHP script
		WWWForm form = new WWWForm ();
		form.AddField("mConnection", "johnny5");
		WWW CCWWW = new WWW (CCAccountUrl, form);

		yield return CCWWW;
		string connection = CCWWW.text;
		if (CCWWW.error != null) {
			Debug.Log ("[ello]: No Internet");
			setConnStatus (false, connection);
		} else {
			if (connection == "DB Con Success!") {
				setConnStatus (true, connection);
			} else if (connection == "DB Con Failed") {
				setConnStatus (false, connection);
			}

		}
	}
	/// <summary>
	/// Updates the ship select co.
	/// </summary>
	/// <returns>The ship select co.</returns>
	/// <param name="lvl">Lvl.</param>
	/// <param name="index">Index.</param>
	/// <param name="userName">User name.</param>
	public IEnumerator UpdateShipSelectCo(int lvl, int index, string userName){
		string updateAccountUrl = "https://tempusfugit.000webhostapp.com/usrAc/updateAc.php";
		//sends messages to PHP script
		WWWForm form = new WWWForm ();
		//variables being sent
		form.AddField ("mUserName", userName);
		form.AddField ("mCurrentLvl", lvl);
		form.AddField ("mCurrentShip", index);

		WWW updateAccountWWW = new WWW (updateAccountUrl, form);

		yield return updateAccountWWW;
		if (updateAccountWWW.error != null) {
			Debug.Log ("[ello]: Cannot connect to Update Account");
		} else {
			string updateAccountReturn = updateAccountWWW.text;
			//print(updateAccountReturn);
			if (updateAccountReturn == "Success.") {
				Debug.Log ("[ello]: Success: Account update");
				SceneManager.LoadScene("Level_01");
			}
		}
	}
	/// <summary>
	/// Gets the data co.
	/// </summary>
	/// <returns>The data co.</returns>
	/// <param name="shipList">Ship list.</param>
	/// <param name="index">Index.</param>
	/// <param name="userName">User name.</param>
	public IEnumerator GetDataCo(GameObject[] shipList, int index, string userName) {
		string getDataUrl = "https://tempusfugit.000webhostapp.com/usrAc/getRecAc.php";
		WWWForm itemsData = new WWWForm ();
		itemsData.AddField ("mUserName", userName);
		WWW getData = new WWW (getDataUrl, itemsData);

		yield return getData;
		string itemsDataStr = getData.text;
		print (itemsDataStr);
		items = itemsDataStr.Split ('|');
		items[1] =  GetDataValue (items [1], "UserName:");
		items[2] = GetDataValue (items [2], "Level:");
		items[3] = GetDataValue (items [3], "Ship:");
		items[4] = GetDataValue (items [4], "Points:");
		items[5] = GetDataValue (items [5], "Rupees:");
		items [6] = GetDataValue (items [6], "Lives:");

		index = int.Parse(items[3]);
		if (shipList [index]) {
			shipList [index].SetActive (true);
		}
	}

	/// <summary>
	/// Creates the account co.
	/// </summary>
	/// <returns>The account co.</returns>
	/// <param name="cUserName">C user name.</param>
	/// <param name="cPassword">C password.</param>
	IEnumerator CreateAccountCo(string cUserName, string cPassword){
		string createAccountUrl = "https://tempusfugit.000webhostapp.com/usrAc/crtAc.php";
		//sends messages to PHP script
		WWWForm form = new WWWForm ();
		//variables being sent
		form.AddField ("mUserName", cUserName);
		form.AddField ("Password", cPassword);

		WWW createAccountWWW = new WWW (createAccountUrl, form);

		yield return createAccountWWW;
		if (createAccountWWW.error != null) {
			Debug.Log ("[ello]: Cannot connect to Account Creation");
		} else {
			string createAccountReturn = createAccountWWW.text;
			GUIobj.outPutString = createAccountReturn;
			print (GUIobj.outPutString);
			if (createAccountReturn == "Success.") {
				GUIobj.outPutString = "Success: Account Created";
				GUIobj.showMessage = false;
				GUIobj.showMessageGood = true;
				GUIobj.currentMenu = "Login";
			} else {
				GUIobj.showMessage = true;
			}
		}
	}

	/// Logins the account.
	/// <summary>
	/// Logins the account co.
	/// </summary>
	/// <returns>The account co.</returns>
	/// <param name="userName">User name.</param>
	/// <param name="password">Password.</param>
	IEnumerator LoginAccountCo(string userName, string password){
		/*
 	* Debug Tools for IEnumerator methods()
		print ("Attempting to Login");
		yield return new WaitForSeconds (1);
*/
		string loginUrl = "https://tempusfugit.000webhostapp.com/usrAc/logAc.php";
		WWWForm form = new WWWForm();

		form.AddField ("mUserName", userName);
		form.AddField ("Password", password);
		WWW LoginAccountWWW = new WWW (loginUrl, form);
		yield return LoginAccountWWW;

		if (LoginAccountWWW.error != null) {
			GUIobj.outPutString = "[ello]: Can not connect to Login";
			GUIobj.showMessage = true;
		} else {
			GUIobj.outPutString = LoginAccountWWW.text;
			print (GUIobj.outPutString);
			if (GUIobj.outPutString.Contains ("Success.")) {
				GUIobj.showMessageGood = true;
				string[] logTextSplit = GUIobj.outPutString.Split (':'); 
				if (logTextSplit [0] == "0") {
					if (logTextSplit [1] == "Success.") {
						SceneManager.LoadScene ("PlayerSelection");
					}
				} else{
					if (logTextSplit [1] == "Success.") {
						int level = int.Parse (logTextSplit [0]) + 1;
						SceneManager.LoadScene (level);
					}
				}
			} else {
				GUIobj.showMessage = true;
			}
		}
	}

	//this is the coroutine to update the users account (data) on the DB
	/// <summary>
	/// Updates the data co.
	/// </summary>
	/// <returns>The data co.</returns>
	/// <param name="userName">User name.</param>
	/// <param name="data">Data.</param>
	/// <param name="dataID">Data I.</param>
	IEnumerator UpdateDataCo(string userName, int data, string dataID){
		string updateAccountUrl = "https://tempusfugit.000webhostapp.com/usrAc/updateDataAc.php";
		//sends messages to PHP script
		WWWForm form = new WWWForm ();
		//variables being sent
		form.AddField ("mUserName", userName);
		form.AddField ("mCurrentData", data);
		form.AddField ("mDataID", dataID);

		WWW updateAccountWWW = new WWW (updateAccountUrl, form);

		yield return updateAccountWWW;
		if (updateAccountWWW.error != null) {
			Debug.LogError ("[ello]: Cannot connect to Update Account");
		} else {
			string updateAccountReturn = updateAccountWWW.text;
			print (updateAccountReturn);
			if (updateAccountReturn == "Success.") {
				print ("[ello]: Success: DATA! updated with DataID: " + dataID);
			}
		}
	}
#endregion
	//setting the connection status so the game will know wether or not ot call Lucy throughout the game
	/// <summary>
	/// Sets the conn status.
	/// </summary>
	/// <param name="status">If set to <c>true</c> status.</param>
	/// <param name="con">Con.</param>
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

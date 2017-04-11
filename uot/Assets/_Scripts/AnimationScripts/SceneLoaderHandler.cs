using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
/* John G. Toland 4/10/17
 * This script handles loading each scene Async
 * Allows for smoother and faster load times between scenes
 * Displays a cleaner view to the user while the scene is done and next scene is loading
 * */
public class SceneLoaderHandler : MonoBehaviour {

	private Text loadingText;
	private GameObject ImageCanvas;
	private Image LoadingImage;
	private bool loadScene = false;
	private bool ENTERING_NEW_SCENE = false;
	private int scene;
	private float flashSpeed = 7.0f;

	void Start(){
		ImageCanvas = GameObject.Find ("LoadingImage");
		ImageCanvas.SetActive (false);
	}

	void Update() {

		if (ENTERING_NEW_SCENE && !loadScene) {
			// ...set the loadScene boolean to true to prevent loading a new scene more than once...
			loadScene = true;
			ENTERING_NEW_SCENE = false;
			ImageCanvas.SetActive (true);
			//bring up the loading image and text
			LoadingImage = ImageCanvas.GetComponent<Image> ();
			GameObject child = LoadingImage.transform.GetChild (0).gameObject;
			loadingText = child.GetComponent<Text> ();
			// ...change the instruction text to read "Loading..."
			loadingText.text = "Loading...";
			// ...and start a coroutine that will load the desired scene.
			StartCoroutine(LoadNewSceneIntCoRoutine());
		}

		if (loadScene) {
			// ...then pulse the transparency of the loading text to let the player know that the computer is still working.
			LoadingImage.color = Color.Lerp(LoadingImage.color,  new Color(0.0f, 0.0f, 0.0f, 1.0f), flashSpeed * Time.deltaTime);
			loadingText.color = new Color(loadingText.color.r, loadingText.color.g, loadingText.color.b, Mathf.PingPong(Time.time, 1));
		}
	}
		
	public void LoadNewSceneString(string scene){
		ENTERING_NEW_SCENE = true;
		if (scene == "PlayerSelection") {
			this.scene = 1;
		} else if (scene == "LevelSelection") {
			this.scene = 2;
		}else if (scene == "Level_01") {
			this.scene = 3;
		}else if (scene == "Level_02") {
			this.scene = 4;
		}else if (scene == "Level_03") {
			this.scene = 5;
		}else if (scene == "Level_04") {
			this.scene = 6;
		}else if (scene == "Level_05") {
			this.scene = 7;
		}
	}

	public void LoadNewSceneInt(int scene){
		ENTERING_NEW_SCENE = true;
		this.scene = scene;
	}

	IEnumerator LoadNewSceneIntCoRoutine() {
		// This line waits for 3 seconds before executing the next line in the coroutine.
		// This line is only necessary for this demo. The scenes are so simple that they load too fast to read the "Loading..." text.
		yield return new WaitForSeconds(3);
		// Start an asynchronous operation to load the scene that was passed to the LoadNewScene coroutine.
		AsyncOperation async = SceneManager.LoadSceneAsync(this.scene);
		// While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
		while (!async.isDone) {
			yield return null;
		}
	}
}
//finito

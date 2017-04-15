using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using UnityEngine;
/* John G. Toland 4/14/17 Unit tests using NUnit framework !!! PASSING UNIT TESTS !!!
 * This is the script that houses the unit tests that are intented to pass NOT fail
 * */
namespace UoT_PassingUnitTest{
	
#region PassingUnitTests_UoT
	[TestFixture]
	[Category("PassingUnitTests")]
	internal class PassingUnitTests{
		
		[Test]
		public void MainCameraFound(){
			GameObject mainCamera = GameObject.Find ("Main Camera");
			Assert.IsNotNull (mainCamera);
		}

		[Test]
		public void DisplayTextFound(){
			GameObject displayText = GameObject.Find ("DisplayText");
			Assert.IsNotNull (displayText);
		}

		[Test]
		public void NavGUIFound(){
			GameObject NavGUI = GameObject.Find ("JOHNS_NAV_GUI_MOBILE");
			Assert.IsNotNull (NavGUI);
		}

		[Test]
		public void NavGUIActiveAndEnabled(){
			GameObject NavGUI = GameObject.Find ("JOHNS_NAV_GUI_MOBILE");
			PauseNavGUI pB = NavGUI.GetComponentInChildren<PauseNavGUI> ();
			Assert.IsTrue (pB.isActiveAndEnabled);
		}
			
		[Test]
		public void ShipListFound(){
			GameObject shipList = GameObject.Find ("ShipList");
			Assert.IsNotNull (shipList);
			Assert.IsTrue(shipList.CompareTag ("ShipList"));
		}

		[Test]
		public void AudioMixerFound(){
			GameObject audio = GameObject.Find ("AudioMixerControl");
			Assert.IsNotNull (audio);
			Assert.IsTrue(audio.CompareTag ("Mixer"));
		}

		[Test]
		public void AudioMixerScriptIsActiveAndEnabled(){
			GameObject audio = GameObject.Find ("AudioMixerControl");
			MixerLevels audioScript = audio.GetComponent<MixerLevels> ();
			Assert.IsTrue (audioScript.isActiveAndEnabled);
		}

		[Test]
		public void DestroyByBoundaryScriptIsActiveAndEnabled(){
			GameObject boundary = GameObject.Find ("Boundary");
			DestroyByBoundary destroyByBoundaryScript = boundary.GetComponent<DestroyByBoundary> ();
			Assert.IsTrue (destroyByBoundaryScript.isActiveAndEnabled);
		}

		[Test]
		public void GameControllerScriptIsActive(){
			GameObject gameController = GameObject.Find ("GameController");
			GameController gcScript = gameController.GetComponent<GameController> ();
			Assert.IsTrue (gcScript.isActiveAndEnabled);
		}
			
		[Test]
		public void ShipListChildCount(){
			GameObject shipList = GameObject.Find ("ShipList");
			int childCount = shipList.transform.childCount;
			Assert.AreEqual (12,childCount);
		}

		[Test]
		public void CoRoutinesFound(){
			GameObject testCoRoutine = GameObject.Find ("CoRoutines");
			Assert.IsNotNull (testCoRoutine);
		}

		[Test]
		public void CoRoutinesIsActiveAndEnabled(){
			GameObject testCoRoutine = GameObject.Find ("CoRoutines");
			CoRoutines CoRo = testCoRoutine.GetComponent<CoRoutines> ();
			Assert.IsTrue (CoRo.isActiveAndEnabled);
		}

		[Test]
		public void ShipIsActiveInHierarchy(){
			GameObject activeShip = GameObject.Find ("Player_00");
			Assert.IsTrue (activeShip.activeInHierarchy);
		}

		[Test]
		public void ShipNotActiveInHierarchy(){
			GameObject activeShip = GameObject.Find ("Player_00");
			activeShip.SetActive (false);
			Assert.IsFalse (activeShip.activeInHierarchy);
			activeShip.SetActive (true);
		}

		[Test]
		public void ColliderIsTrigger(){
			GameObject activeShip = GameObject.Find ("Player_00");
			Collider shipCollider = activeShip.GetComponent <Collider> ();
			Assert.IsTrue (shipCollider.isTrigger);
		}

		[Test]
		public void MenuIsActiveInHierarchy(){
			GameObject menu = GameObject.Find ("Menu");
			menu.SetActive (false);
			Assert.IsFalse (menu.activeInHierarchy);
			menu.SetActive (true);
		}

		[Test]
		public void PlayerShipIsDestroyed(){
			GameObject activeShip = GameObject.Find ("DestroyableGameObject");
			GameObject.DestroyImmediate (activeShip);
			GameObject destroyedShip = GameObject.Find ("DestroyableGameObject");
			Assert.Null (destroyedShip);
		}

		[Test]
		public void RuppeeIsCollected(){
			GameObject gameController = GameObject.Find ("GameController");
			GameController gcScript = gameController.GetComponent<GameController> ();
			gcScript.AddRupees (1);
			Assert.AreEqual (1, gcScript.getRupeeCount ());
			gcScript.clearValues ();
			Assert.AreEqual (0, gcScript.getRupeeCount ()); 
		}

		[Test]
		public void PointsAreCollected(){
			GameObject gameController = GameObject.Find ("GameController");
			GameController gcScript = gameController.GetComponent<GameController> ();
			gcScript.AddScore (10);
			Assert.AreEqual (10, gcScript.getScore ());
			gcScript.clearValues ();
			Assert.AreEqual (0, gcScript.getScore());
		}

		[Test]
		public void LifesAreCollected(){
			GameObject gameController = GameObject.Find ("GameController");
			GameController gcScript = gameController.GetComponent<GameController> ();
			gcScript.setTest ();
			gcScript.clearValues ();
			gcScript.AddLife (1);
			Assert.AreEqual (2, gcScript.getLives ());
			gcScript.clearValues ();
			Assert.AreEqual (1, gcScript.getLives ());
		}

		[Test]
		public void ShotSpawnIsCollected(){
			GameObject player = GameObject.FindGameObjectWithTag ("Player");
			PlayerController playerScript = player.GetComponent<PlayerController> ();
			playerScript.addShotSpawn ();
			Assert.AreEqual (1, playerScript.numberOfSpawns);
			playerScript.clearValues ();
			Assert.AreEqual (0, playerScript.numberOfSpawns);
		}

		[Test]
		public void TestMaxShotSpawns(){
			GameObject player = GameObject.FindGameObjectWithTag ("Player");
			PlayerController playerScript = player.GetComponent<PlayerController> ();
			int maxShotSpawns = 6;
			for (int i = 0; i < 10; i++) {
				playerScript.addShotSpawn ();
			}
			Assert.AreNotEqual (10, playerScript.numberOfSpawns);
			Assert.AreEqual (maxShotSpawns, playerScript.numberOfSpawns);
			playerScript.clearValues ();
			Assert.AreEqual(0, playerScript.numberOfSpawns);
		}

	}
#endregion
//finito


}

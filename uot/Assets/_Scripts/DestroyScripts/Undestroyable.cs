using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 
/// 03/01/17 Gerard Fierro
/// Used for objects that cannot be destroyed just avoided
/// 
/// </summary>



public class Undestroyable : MonoBehaviour
{

    //public explosion event reference
    public GameObject playerExplosion;
    //instance of gamecontroller reference
    private GameController gameController;
	public bool deadPlayer = false;


    void Start()
    {
        //Finding game object that holds gamecontroller script
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            //set reference to game controller component
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        if (gameControllerObject == null)
        {
            //if not reference object is found
            Debug.Log("Cannot find 'GameController' script");
        }
    }

    void OnTriggerEnter(Collider other)
	{
		///object is only destroyed by leaving the boundary.
		if (other.CompareTag ("Boundary") || other.CompareTag ("Enemy")) {
			return;
		}


		//player explodes if touches the object
		if (other.tag == "Player") {
			Instantiate (playerExplosion, other.transform.position, other.transform.rotation);
			if (gameController.getLivesCount () == 1) {
				gameController.AddLife (-1);
				gameController.GameOver ();
			} else {
				///loss of one life
				gameController.AddLife (-1);
			}
			deadPlayer = true;
			gameController.setPlayerDead (true);
			Destroy (other.gameObject);

		}
	}

}
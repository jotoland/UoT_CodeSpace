using UnityEngine;
using System.Collections;

//Richard O'Neal 02/20/17
public class WeaponController : MonoBehaviour
{
	public GameObject shot;
	public Transform shotSpawn;
	public float fireRate;
	public float delay;
	private PauseNavGUI pB;
	private GameController gc;
	private bool canFire = true;

	void Start ()
	{
		GameObject pBObject = GameObject.FindGameObjectWithTag ("PauseBtn");
		if (pBObject != null) {
			pB = pBObject.GetComponent <PauseNavGUI> ();
		}

		GameObject gcObject = GameObject.FindGameObjectWithTag ("GameController");
		if (gcObject != null) {
			gc = gcObject.GetComponent <GameController> ();
		}
		InvokeRepeating ("Fire", delay, fireRate);
	}

	void Fire ()
	{
		if (canFire) {
			Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
			GetComponent<AudioSource>().Play();
		}

	}

	void Update(){
		if(!gc.isGameOver ()){
			if(pB.isLEFT_SCENE ()){
				canFire = false;
				pB.setLEFT_SCENE (true);
			}
		}
	}

}

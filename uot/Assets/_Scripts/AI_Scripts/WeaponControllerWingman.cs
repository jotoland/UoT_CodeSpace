using UnityEngine;
using System.Collections;

//Andrew Salopek
//03/20/2017
public class WeaponControllerWingman : MonoBehaviour
{
	public GameObject shot;
	public Transform shotSpawn;
	public float shotRate;

//	void Update() {
//		GameObject enemy = GameObject.FindGameObjectWithTag ("Enemy");
//		if (enemy) {
//			InvokeRepeating ("Fire", 1f, 2f);
//		}
//	}

	void Start ()
	{
			InvokeRepeating ("Fire", 0f, shotRate);
	}

	void Fire ()
	{
		GameObject enemy = GameObject.FindGameObjectWithTag ("Enemy");
		if (enemy) {
			Transform other = enemy.transform;
			float dist = Vector3.Distance(other.position, transform.position);
			print("Distance to other: " + dist);
			if (dist < 12.0) {
				Instantiate (shot, shotSpawn.position, shotSpawn.rotation);
				GetComponent<AudioSource> ().Play ();
			}
		}
	}

}

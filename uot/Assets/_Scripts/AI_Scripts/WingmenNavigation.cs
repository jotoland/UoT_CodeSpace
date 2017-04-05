using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

//Andrew Salopek
//03/20/2017

public class WingmenNavigation : MonoBehaviour {
	//public GameObject m_Player;
	public GameObject playerExplosion;
	private NavMeshAgent navMeshAgent;
	private GameObject player;
	// Use this for initialization
	void Start () {		
		navMeshAgent = GetComponent<NavMeshAgent> ();
//		m_Player = GameObject.FindGameObjectWithTag ("Player");
		//wait ();
		//InvokeRepeating ("Move", 0f, .01667f);
		InvokeRepeating ("Move", 3f, .01667f);

	}

	IEnumerator wait () {
		yield return new WaitForSeconds (3f);
		InvokeRepeating ("Move", 0f, .01667f);

	}

	void Move () {
		GameObject player = GameObject.FindGameObjectWithTag ("Player");
		if (player) {
			//			GameObject player = GameObject.FindGameObjectWithTag ("Player");
			//			navMeshAgent.destination = m_Player.position;
			if (player) {
				if (navMeshAgent.tag == "WingMan1") {
					navMeshAgent.destination = player.transform.position + transform.right * 3;
				} else {
					navMeshAgent.destination = player.transform.position + transform.right*(-3);
				}
			}
			GameObject enemy = GameObject.FindGameObjectWithTag ("Enemy");
			if (enemy) {
				navMeshAgent.destination = enemy.transform.position;
			}
		} else {
			Instantiate (playerExplosion, gameObject.transform.position, gameObject.transform.rotation);
			Destroy (gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
//		m_Player = GameObject.FindGameObjectWithTag ("Player");
//		if (m_Player) {
////			GameObject player = GameObject.FindGameObjectWithTag ("Player");
////			navMeshAgent.destination = m_Player.position;
//			if (player) {
//				navMeshAgent.destination = player.transform.position;
//
//			}
//			GameObject enemy = GameObject.FindGameObjectWithTag ("Enemy");
//			if (enemy) {
//				navMeshAgent.destination = enemy.transform.position;
//			}
//		} else {
//			Instantiate (playerExplosion, gameObject.transform.position, gameObject.transform.rotation);
//			Destroy (gameObject);
//		}
	}
}

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
	}
		

	void Update () {
		GameObject player = GameObject.FindGameObjectWithTag ("Player");
		if (player) {
			if (player) {
				if (navMeshAgent.tag == "WingMan1") {
					navMeshAgent.destination = player.transform.position + transform.right * 5;
				} else {
					navMeshAgent.destination = player.transform.position + transform.right*(-5);
				}
			}
			GameObject enemy = GameObject.FindGameObjectWithTag ("Enemy");
			if (enemy) {
				navMeshAgent.destination = enemy.transform.position;
			}
		}
	}
}

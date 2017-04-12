using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

//Andrew Salopek
//03/20/2017

public class WingmenNavigation : MonoBehaviour {
	public Transform m_Player;
	public GameObject playerExplosion;
	private NavMeshAgent navMeshAgent;
	// Use this for initialization
	void Start () {		
		navMeshAgent = GetComponent<NavMeshAgent> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (m_Player) {
			navMeshAgent.destination = m_Player.position;
		} else {
			Instantiate (playerExplosion, gameObject.transform.position, gameObject.transform.rotation);
			Destroy (gameObject);
		}
	}
}

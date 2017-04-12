//Richard O'Neal 2/16/2017

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour {

	public float speed;

	void Start (){	
		
	GetComponent<Rigidbody>().velocity = transform.forward * speed;
	
	}

}

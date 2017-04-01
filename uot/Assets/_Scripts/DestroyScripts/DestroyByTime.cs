using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByTime : MonoBehaviour {

	//lifetime of gameObjects
	public float lifeTime;

	void Start(){
		Destroy (gameObject, lifeTime);
	}
}

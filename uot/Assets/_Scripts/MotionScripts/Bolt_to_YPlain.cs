using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Nicholas Muirhead 4/16/2017

//Fixes the problem where the tilt of a ship would take the shot off of the y plain
//this cause the bolt to appear to miss the player even when look like it should be a direct hit
//it also fixed the problem that shots fired at a tilt looked narrower than they really were
public class Bolt_to_YPlain : MonoBehaviour {
    private float ydegree;

    void Start () {
        ydegree = Mathf.Atan(gameObject.GetComponent<Rigidbody>().velocity.x / gameObject.GetComponent<Rigidbody>().velocity.z) * Mathf.Rad2Deg;
        transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
        gameObject.transform.rotation = Quaternion.Euler(0f, ydegree, 0f);
	}
    
}

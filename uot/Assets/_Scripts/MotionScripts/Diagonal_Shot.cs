using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Nicholas Muirhead 4/16/2017
public class Diagonal_Shot : MonoBehaviour {
    public float zspeed;
    public float xspeed;
	void Start () {
        GetComponent<Rigidbody>().velocity = (transform.forward * zspeed) + (transform.right * xspeed);
    }
	
}

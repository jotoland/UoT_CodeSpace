
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Destroy by boundary.
/// 
/// /// 02/17/17 Richard O'Neal
/// /// 02/17/17 John G. Toland
/// 
/// used to destroy hazards with bolts with the boundary (as the leave the game view)
/// </summary>

public class DestroyByBoundary : MonoBehaviour {
	
	void OnTriggerExit (Collider other) {
		Destroy(other.gameObject);
	}
		
}

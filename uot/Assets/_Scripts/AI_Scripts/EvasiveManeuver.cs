using UnityEngine;
using System.Collections;
//Richard O'Neal 02/20/17
public class EvasiveManeuver : MonoBehaviour
{
	//for clamping the enemy ship
	public Boundary boundary;
	public float tilt;
	//helps pick target manuver
	public float dodge;
	//cleans up the movement
	public float smoothing;
	//range for random range need 2 fields (min, max)
	public Vector2 startWait;
	public Vector2 maneuverTime;
	public Vector2 maneuverWait;
	//the current speed from the mover script
	private float currentSpeed;
	//holds the destination value
	private float targetManeuver;

	void Start ()
	{
		currentSpeed = GetComponent<Rigidbody>().velocity.z;
		StartCoroutine(Evade());
	}

	///sets the value of the destination the slowly moves towards it (push manuever)
	IEnumerator Evade ()
	{
		//random range to give it variance before they move
		yield return new WaitForSeconds (Random.Range (startWait.x, startWait.y));
		while (true)
		{
									              //mulitplied by the reverse of its sign negative becomes positive
			//setting destination				  //and positive becomes negative
			targetManeuver = Random.Range (1, dodge) * -Mathf.Sign (transform.position.x);
			//wait while it does some movement
			yield return new WaitForSeconds (Random.Range (maneuverTime.x, maneuverTime.y));
			//no longer move
			targetManeuver = 0;
			//wait for sometime
			yield return new WaitForSeconds (Random.Range (maneuverWait.x, maneuverWait.y));
			//loop continues
		}
	}
	
	void FixedUpdate ()
	{
		///moving towards the targetManeuver
		float newManeuver = Mathf.MoveTowards (GetComponent<Rigidbody>().velocity.x, targetManeuver, smoothing * Time.deltaTime);
		//keeps the speed equal to the mover script
		GetComponent<Rigidbody>().velocity = new Vector3 (newManeuver, 0.0f, currentSpeed);
		//clamping the position of the enemy inside the screen
		GetComponent<Rigidbody>().position = new Vector3
		(//these are the clamps
			Mathf.Clamp(GetComponent<Rigidbody>().position.x, boundary.xMin, boundary.xMax), 
			0.0f, 
			Mathf.Clamp(GetComponent<Rigidbody>().position.z, boundary.zMin, boundary.zMax)
		);
		//giving the ship some tilt
		GetComponent<Rigidbody>().rotation = Quaternion.Euler (0, 0, GetComponent<Rigidbody>().velocity.x * -tilt);
	}

}

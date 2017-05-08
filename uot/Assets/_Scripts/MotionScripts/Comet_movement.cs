using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Nicholas Muirhead 3/20/17
public class Comet_movement : MonoBehaviour {

    public float speed;
    public float horizontal_speed;
    public float tumble;
    public Boundary boundary;
    

    void Start () {
        //int array that is used to calculate if the comet starts off moving left or right
        int[] leftOrRight = { -1, 1 };
        int i = Random.Range(0, 2);
        horizontal_speed *= leftOrRight[i];
        //uses that speed and horizontal speed to calculate that vector in which the comet starts to move
        GetComponent<Rigidbody>().velocity = transform.forward * speed + transform.right * horizontal_speed;
        GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * tumble;

    }
    void FixedUpdate()
    {
        if(GetComponent<Rigidbody>().position.x < boundary.xMin-5.5)
        {
            
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            transform.rotation = Quaternion.identity;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().velocity = (transform.forward * speed) + (transform.right * -1 * horizontal_speed);
            GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * tumble;
        }
        else if(GetComponent<Rigidbody>().position.x > boundary.xMax+5.5)
        {
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            transform.rotation = Quaternion.identity;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().velocity = (transform.forward * speed) + (transform.right * -1 * horizontal_speed);
            GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * tumble;
        }
    } 
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Nicholas Muirhead 3/24/17
//Leave a trail of asteroids behind the head asteroid
public class trail : MonoBehaviour {
    public GameObject TrailComet;

    void Start()
    {	//corutines that spwan gameobjects behind the parent object
        StartCoroutine(waitStart());
        StartCoroutine(trailWait());
            
                       
    }
    IEnumerator waitStart()
    {
    	//wait to spawn the frist of the trail
    	//this makes sure that the child object does not spawn stay off screen
        yield return new WaitForSeconds(.8f);
        Instantiate(TrailComet, transform.position, Quaternion.identity);
    }
    IEnumerator trailWait()
    {
    	//while the parent object has not been destroyed leave a trail
    	//of cloned objects behind every .4 seconds
        while(gameObject != null&& gameObject.activeInHierarchy)
        {
            yield return new WaitForSeconds(.4f);
            Instantiate(TrailComet, transform.position, Quaternion.identity);
        }
    }
}

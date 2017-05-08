using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Nicholas Muirhead 3/24/17
//Leave a trail of asteroids behind the head asteroid
public class trail : MonoBehaviour {
    public GameObject TrailComet;

    void Start()
    {
        StartCoroutine(waitStart());
        StartCoroutine(trailWait());    
    }

    IEnumerator waitStart()
    {
        yield return new WaitForSeconds(.8f);
        Instantiate(TrailComet, transform.position, Quaternion.identity);
    }

    IEnumerator trailWait()
    {
        while (gameObject != null && gameObject.activeInHierarchy)
        {
            yield return new WaitForSeconds(.4f);
            Instantiate(TrailComet, transform.position, Quaternion.identity);
        }
    }

}

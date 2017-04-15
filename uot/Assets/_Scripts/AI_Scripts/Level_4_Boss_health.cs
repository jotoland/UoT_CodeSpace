using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Nicholas Muirhead 4/14/2017

public class Level_4_Boss_health : MonoBehaviour
{
    //Gives the boss at end of level 4 its health
    public int Health;
    public GameObject explosion;

    void OnTriggerEnter(Collider other)
    {
        //if the boss is hit with a bolt decrease the bosses health by one
        if (other.CompareTag("Bolt"))
        {
            Health -= 1;
            Debug.Log("Health deducted");
        }
        //if the bosses health is zero then it is destoryed
        if (Health == 0)
        {
            if(explosion != null)
            {
                Instantiate(explosion, transform.position, transform.rotation);
            }
            DestroyObject(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Nicholas Muirhead 4/15/2017
//Nicholas Muirhead 4/16/2017
public class Level_4_Boss_AI : MonoBehaviour
{
    public GameObject Big_shot;
    public GameObject Big_shotDL;
    public GameObject Big_shotDR;
    public GameObject small_shot;
    public Transform ShotSpawn;
    public Transform ShotSpawn_Left;
    public Transform ShotSpawn_LeftM;
    public Transform ShotSpawn_LM;
    public Transform ShotSpawn_Right;
    public Transform ShotSpawn_RightM;
    public Transform ShotSpawn_RM;
    public float fireRate;
    public float delay;
    private float PercentHealth;
    private int attackchoice;
    void Start()
    {
        PercentHealth = gameObject.GetComponent<Level_4_Boss_health>().CurrentHealth / gameObject.GetComponent<Level_4_Boss_health>().StartHealth;
        print("PH1 " + PercentHealth);
        
        Invoke("delayFire", 2f);
    }

    void delayFire()
    {
        InvokeRepeating("attack", 0f, 1.5f);
    }

    void attack()
    {
        PercentHealth = gameObject.GetComponent<Level_4_Boss_health>().CurrentHealth / gameObject.GetComponent<Level_4_Boss_health>().StartHealth;
        if (PercentHealth >= .8f)
        {
           
            attackchoice = Random.Range(1, 5);
            print("attcknum" + attackchoice);
            if (attackchoice == 1)
            {
                StartCoroutine(LeftM_RightM_M_Fire());
            }
            else if (attackchoice == 2)
            {
                StartCoroutine(LeftM_RightM_Fire());
            }
            else if (attackchoice == 3)
            {
                StartCoroutine(LM_RM_Fire());
            }
            else if (attackchoice == 4)
            {
                StartCoroutine(Fire());
            }

        }
        else if(PercentHealth >= .3f)
        {
            attackchoice = Random.Range(1, 5);
            if (attackchoice == 1)
            {
                StartCoroutine(CrissCross());
            }
            else if (attackchoice == 2)
            {
                StartCoroutine(LeftStrike());
            }
            else if (attackchoice == 3)
            {
                StartCoroutine(RightStrike());
            }
            else if (attackchoice == 4)
            {
                StartCoroutine(Fire());
            }
        }else
        {
            attackchoice = Random.Range(1, 4);
            if (attackchoice == 1)
            {
                StartCoroutine(DoubleCross());
            }
            else if (attackchoice == 2)
            {
                StartCoroutine(CrissCross_LM_RM());
            }
            else if (attackchoice == 3)
            {
                StartCoroutine(Fire());
            }
        }

    }
    IEnumerator DoubleCross()
    {
        GetComponent<AudioSource>().Play();
        Instantiate(Big_shotDR, ShotSpawn_Left.position, ShotSpawn_Left.rotation);
        Instantiate(Big_shotDL, ShotSpawn_Right.position, ShotSpawn_Right.rotation);
        Instantiate(Big_shotDR, ShotSpawn_LM.position, ShotSpawn_LM.rotation);
        Instantiate(Big_shotDL, ShotSpawn_RM.position, ShotSpawn_RM.rotation);
        yield return new WaitForSeconds(1.5f);
    }
    IEnumerator CrissCross_LM_RM()
    {
        GetComponent<AudioSource>().Play();
        Instantiate(Big_shot, ShotSpawn_LeftM.position, ShotSpawn_LeftM.rotation);
        Instantiate(Big_shot, ShotSpawn_RightM.position, ShotSpawn_RightM.rotation);
        Instantiate(Big_shotDR, ShotSpawn_Left.position, ShotSpawn_Left.rotation);
        Instantiate(Big_shotDL, ShotSpawn_Right.position, ShotSpawn_Right.rotation);
        yield return new WaitForSeconds(1.5f);
    }

    IEnumerator CrissCross()
    {
        GetComponent<AudioSource>().Play();
        Instantiate(Big_shotDR, ShotSpawn_Left.position, ShotSpawn_Left.rotation);
        Instantiate(Big_shotDL, ShotSpawn_Right.position, ShotSpawn_Right.rotation);
        yield return new WaitForSeconds(1.5f);
    }

    IEnumerator RightStrike()
    {
        GetComponent<AudioSource>().Play();
        Instantiate(Big_shotDL, ShotSpawn_Right.position, ShotSpawn_Right.rotation);
        Instantiate(Big_shotDL, ShotSpawn_RM.position, ShotSpawn_RM.rotation);
        yield return new WaitForSeconds(.1f);
        Instantiate(Big_shotDL, ShotSpawn_LeftM.position, ShotSpawn_LeftM.rotation);
        yield return new WaitForSeconds(1.4f);
    }

    IEnumerator LeftStrike()
    {
        GetComponent<AudioSource>().Play();
        Instantiate(Big_shotDR, ShotSpawn_Left.position, ShotSpawn_Left.rotation);
        Instantiate(Big_shotDR, ShotSpawn_LM.position, ShotSpawn_LM.rotation);
        yield return new WaitForSeconds(.1f);
        Instantiate(Big_shotDR, ShotSpawn_RightM.position, ShotSpawn_RightM.rotation);
        yield return new WaitForSeconds(1.4f);
    }
    IEnumerator LeftM_RightM_M_Fire()
    {
        for(int i = 0; i < 1; i++)
        {
            GetComponent<AudioSource>().Play();
            Instantiate(Big_shot, ShotSpawn.position, ShotSpawn.rotation);
            yield return new WaitForSeconds(.1f);
            Instantiate(Big_shot, ShotSpawn_LeftM.position, ShotSpawn_LeftM.rotation);
            Instantiate(Big_shot, ShotSpawn_RightM.position, ShotSpawn_RightM.rotation);
            
           
            yield return new WaitForSeconds(1.4f);
        }

    }

    IEnumerator LeftM_RightM_Fire()
    {
        for(int i = 0; i < 1; i++)
        {
            Instantiate(Big_shot, ShotSpawn_LeftM.position, ShotSpawn_LeftM.rotation);
            Instantiate(Big_shot, ShotSpawn_RightM.position, ShotSpawn_RightM.rotation);
            GetComponent<AudioSource>().Play();
            yield return new WaitForSeconds(1.5f);
        }
        
    }

    IEnumerator LM_RM_Fire()
    {
        for (int i = 0; i < 1; i++)
        {
            Instantiate(Big_shot, ShotSpawn_LM.position, ShotSpawn_LM.rotation);
            Instantiate(Big_shot, ShotSpawn_RM.position, ShotSpawn_RM.rotation);
            GetComponent<AudioSource>().Play();
            yield return new WaitForSeconds(1.5f);
        }
    }

    IEnumerator Fire()
    {
        for (int i = 0; i < 1; i++)
        {
            Instantiate(Big_shot, ShotSpawn.position, ShotSpawn.rotation);
            Instantiate(Big_shot, ShotSpawn_Left.position, ShotSpawn_Left.rotation);
            Instantiate(Big_shot, ShotSpawn_Right.position, ShotSpawn_Right.rotation);
            GetComponent<AudioSource>().Play();
            yield return new WaitForSeconds(1.5f);
        }
    }
    
}

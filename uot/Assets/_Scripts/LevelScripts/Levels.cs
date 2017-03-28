using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// Levels.
/// John G. Toland 3/10/17
/// This script is intended to use amoungst all levels.
/// This script controls the progress of the level.
/// Seperating the progress of the level and the over all gamecontroller allows for 
/// better readability. 
/// The addition of this script also allows form making all variables in GameController private, 
/// acessing these varibles are now done with getters and setters.
/// </summary>
public class Levels : MonoBehaviour
{
    //all level variables
    //Scene currentScene;
    private GameController gc;
    public GameObject[] hazards;
    public Vector3 spawnValues;
    public int hazardCount;
    public float spawnWait;
    public float startWait;
    public float waveWait;
    public int numOfWavesInLvl;
    private int spawnWaveCount;
    private bool beginBossWaveGeneric;
	public int BossHazardCount;


    // Use this for initialization
    void Start()
    {
        //currentScene = SceneManager.GetActiveScene ();
        //get instance of gameController for access to game progress fucntions within your level
        GameObject gcObject = GameObject.FindGameObjectWithTag("GameController");
        if (gcObject != null)
        {
            gc = gcObject.GetComponent<GameController>();
        }
    }

    // Update is called once per frame, this is were you will check to see if it is time for your boss wave to spawn.
    void Update()
    {
        ///spawning the boss wave for level_01
        if (beginBossWaveGeneric)
        {
            StartCoroutine(SpawnBossWaveGeneric());
            beginBossWaveGeneric = false;
        }
    }

    #region methodsToStartCoroutines
    /// <summary>
    /// Starts the generic lvl Coroutine.
    /// </summary>
    public void StartGenericLvl()
    {
        StartCoroutine(SpawnWaves());
    }
    #endregion

    /// <summary>
    /// Checks the player progress in lvl.
    /// </summary>
    /// <returns><c>true</c>, if player progress in lvl was checked, <c>false</c> otherwise.</returns>
    /// <param name="isRegularWave">If set to <c>true</c> is regular wave.</param>
    public bool checkPlayerProgressInLvl(bool isRegularWave)
    {
        if (isRegularWave)
        {
            spawnWaveCount++;
            print("wave count = " + spawnWaveCount);
            if (gc.isGameOver())
            {
                gc.setRestart(true);
                return false;
            }
            else if (gc.isPlayerDead())
            {
                spawnWaveCount = 0;
                print("inside player is dead");
                gc.ReSpawn();
                gc.setPlayerDead(false);
                return true;
            }
            else if (spawnWaveCount == numOfWavesInLvl && !gc.isGameOver())
            {
                enteringBossWave(gc.getLvlCount());
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            spawnWaveCount++;
            print("wave count = " + spawnWaveCount);
            if (gc.isGameOver())
            {
                gc.setRestart(true);
                return false;
            }
            else if (gc.isPlayerDead())
            {
                spawnWaveCount = numOfWavesInLvl;
                gc.ReSpawn();
                gc.setPlayerDead(false);
                return true;
            }
            else
            {
                return true;
            }
        }
    }

    /// <summary>
    /// Enterings the boss wave.
    /// </summary>
    /// <param name="lvlCount">Lvl count.</param>
    public void enteringBossWave(int lvlCount)
    {
        switch (lvlCount)
        {
            case 1:
                beginBossWaveGeneric = true;
                break;
            case 2:
                //level 2 boss wave case
                beginBossWaveGeneric = true;                        ///used for testing
                break;
            case 3:
                //level 3 boss wave case
                beginBossWaveGeneric = true;                        ///used for testing
                break;
            case 4:
                //level 4 boss wave case
                beginBossWaveGeneric = true;                        ///used for testing
                break;
            case 5:
                //level 5 boss wave case
                beginBossWaveGeneric = true;                        ///used for testing
                break;
            default:
                //nothing to do here
                break;
        }
    }

    #region GenericLvl waveSpawning
    /// <summary>
    /// Spawns the waves.
    /// </summary>
    /// <returns>The waves.</returns>
    IEnumerator SpawnWaves()
    {
        ///we must wait for 3 seconds for the database to load and update the new information for each level.
        yield return new WaitForSeconds(3f);
        gc.setGameOverText(true);
        yield return new WaitForSeconds(startWait);
        gc.setGameOverText(false);
        while (true)
        {
            for (int i = 0; i < hazardCount; i++)
            {
                GameObject hazard = hazards[Random.Range(0, hazards.Length)];//Picks random hazard from hazards array
                Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                Quaternion spawnRotation = Quaternion.identity;
                Instantiate(hazard, spawnPosition, spawnRotation);
                yield return new WaitForSeconds(spawnWait);
            }
            yield return new WaitForSeconds(waveWait);
            if (!checkPlayerProgressInLvl(true))
            {
                break;
            }
        }
    }

    /// <summary>
    /// Spawns the boss wave level 01.
    /// </summary>
    /// <returns>The boss wave level 01.</returns>
    IEnumerator SpawnBossWaveGeneric()
    {
        yield return new WaitForSeconds(startWait);
        while (true)
        {
            for (int i = 0; i < BossHazardCount; i++)
            {
                GameObject hazard = hazards[Random.Range(0, hazards.Length)];
                Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                Quaternion spawnRotation = Quaternion.identity;
                Instantiate(hazard, spawnPosition, spawnRotation);
                yield return new WaitForSeconds(0.1f);
            }
            yield return new WaitForSeconds(waveWait);
            //spawnWaveCount++;
            print("wave count inside bosswave = " + spawnWaveCount);
            if (!checkPlayerProgressInLvl(false))
            {
                break;
            }
            if (spawnWaveCount == numOfWavesInLvl + 1 && !gc.isGameOver())
            {
                gc.levelCompleted();
                yield return new WaitForSeconds(gc.getLoadLvlWait());
                if (gc.getLvlCount() >= 5)
                {
                    gc.resetLvlCount();
                    SceneManager.LoadScene(gc.getLvlCount());
                }
                else
                {
                    SceneManager.LoadScene(gc.getLvlCount() + 2);
                }
            }
        }
    }

    #endregion
}
//finito
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static MinionClass;

public class RandomMinionSpawning : MonoBehaviour
{
    SpawnLocations[] spawnLocations;
    void Start()
    { 
        //MinonSpawn();
    }
    private void Reset()
    {
        //SpawnLocations[] location = random;
        //location.minions[random]
        //get random pos between parL and parR
    }
}

public class SpawnLocations : MonoBehaviour
{
    Transform parL;
    Transform parR;
    /*bool canSpawn;

    void OnTriggerEnter2D(GameObject collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            canSpawn = false;
        }
    }*/

    SpawnLocations(Transform parL, Transform parR)
    {
        this.parL=parL;
        this.parR=parR;
    }

    public GameObject[] minions;
}

/* Spawn Minions at Random Positions
 * 
 * Asign diferent areas/locations sutch as desert, watter and beach, these are pre determined
 * 
 * Eatch minion has it's onw set of these areas where it can spawn whithin
 * 
 * Eatch one of these areas neeed a parameter, between t1 and t2 (trans)
 * 
 * 
 * Random Minion Spawning will randomize witch one of the locations the minion will spawn at
 * then we take the parameters of that location
 * we randomize  a position within that area
 * spawn the minion at that location
 * 
 * 
 * the locations will then have a variable to keep track of if player is in it or not, if player is in the area minions will not be alowed to spawn in there
 * then we can update the random minion spawning to only select between locations where the player isn't close
 * if there's no locations the minion can be spawned at it will not spawn
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * Locations (spawn parameters)
 * Location has potential minions to spawn
 * 
 * Randomizer select location
 * Location randomize minion
 * 
 * randomize location for minion within location parameters
 * 
 * location has bool for player
 * if player close location can not be selected
 */
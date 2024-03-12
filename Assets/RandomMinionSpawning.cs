using UnityEngine;
using static MinionClass;

public class RandomMinionSpawning : MonoBehaviour
{
    [SerializeField] RandSpawnLocations[] randSpawnLocations;
    [SerializeField] float spawnZValue;

    void Start()
    {
        SpawnRandom();
    }

    private void SpawnRandom()
    {
        foreach (RandSpawnLocations location in randSpawnLocations)
        {
            if (location != null && location.minions.Length > 0)
            {
                int randMinion = Random.Range(0, location.minions.Length);
                float randX = Random.Range(location.paramL.position.x, location.paramR.position.x);
                float randY = Random.Range(location.paramL.position.y, location.paramR.position.y);                
                Vector3 randomPosition = new Vector3(randX, randY, spawnZValue);

                GameObject minion = Instantiate(location.minions[randMinion], randomPosition, Quaternion.identity);
            }
        }
    }
}

[System.Serializable]
public class RandSpawnLocations
{
    public Transform paramL;
    public Transform paramR;
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
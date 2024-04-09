using System;
using UnityEngine;
using static MinionClass;
using Random = UnityEngine.Random;

public class RandomMinionSpawning : MonoBehaviour
{
    [SerializeField] RandSpawnLocations[] randSpawnLocations;
    [SerializeField] float spawnZValue;
    [SerializeField] float cycleDelay;
    [SerializeField] float cycleTimer;

    void Start()
    {
        cycleTimer = cycleDelay;

        foreach (RandSpawnLocations location in randSpawnLocations)
        {
            // set max/min
            location.maxPoint = location.recParam.position + new Vector3(location.recParam.sizeDelta.x, location.recParam.sizeDelta.y);
            location.minPoint = location.recParam.position;

            if (location != null && location.minions.Length > 0)
            {
                int randMinion = Random.Range(0, location.minions.Length);
                float randX = Random.Range(location.minPoint.x, location.maxPoint.x);
                float randY = Random.Range(location.minPoint.y, location.maxPoint.y);
                Vector3 randomPosition = new Vector3(randX, randY, spawnZValue);

                GameObject minion = Instantiate(location.minions[randMinion], randomPosition, Quaternion.identity);
            }
        }
    }

    private void Spawn(RandSpawnLocations locaton)
    {
        if (locaton.minions.Length <= 0)
            return;

        float xSpawn = Random.Range(locaton.minPoint.x, locaton.maxPoint.x);

        RaycastHit2D hit = Physics2D.Raycast(new Vector2(xSpawn, Random.Range(locaton.minPoint.y, locaton.maxPoint.y)), Vector2.down, locaton.maxPoint.y-locaton.minPoint.y);
        if (hit.collider == null)
            return;

        GameObject enity = locaton.minions[Random.Range(0, locaton.minions.Length)];
        Vector2 enitySize = enity.GetComponent<SpriteRenderer>().bounds.size;
        if (Physics2D.Raycast(hit.point, Vector2.up, enitySize.y).collider != null)
            return;
        if (Physics2D.Raycast(hit.point + Vector2.left * enitySize.x * 0.5f, Vector2.right, enitySize.x).collider != null)
            return;
        /* // fix if whant a max amount of minions that can spawn
        bool foundNull = false;
        for (int i = 0; i < spawnedEntitys.Length; i++)
        {
            if (spawnedEntitys[i] == null)
            {
                foundNull = true;
                spawnedEntitys[i] = Instantiate(enity, hit.point + Vector2.up * enitySize.y * 0.5f, Quaternion.identity);
                break;
            }
        }
        if (!foundNull)
        {
            for (int i = 0; i < spawnedEntitys.Length; i++)
            {
                if (Mathf.Abs(spawnedEntitys[i].transform.position.x - transform.position.x) > spawingDistance.y || Mathf.Abs(spawnedEntitys[i].transform.position.y - transform.position.y) > spawingDistance.y)
                    Destroy(spawnedEntitys[i]);
            }
        }
        */
    }

    void Update()
    {
        if (cycleTimer > 0)
            cycleTimer -= Time.deltaTime;
        else
        {
            foreach(RandSpawnLocations location in randSpawnLocations)
            {
                if (location.cycle != 0)
                {
                    location.cycle--;
                    continue;
                }
                location.cycle = location.cycleDelay;

                if (Random.Range(location.spawnChance.x,location.spawnChance.y+1) == location.spawnChance.x)
                    Spawn(location);
            }
            cycleTimer += cycleDelay;
        }
    }
}

[System.Serializable]
public class RandSpawnLocations
{
    public RectTransform recParam;
    [NonSerialized] public Vector2 maxPoint;
    [NonSerialized] public Vector2 minPoint;
    public int cycleDelay; // how many cycles before spawn
    [NonSerialized] public int cycle; // amount of cycles left
    public Vector2Int spawnChance;
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
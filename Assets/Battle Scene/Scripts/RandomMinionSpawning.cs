using System;
using System.Collections.Generic;
using UnityEngine;
using static MinionClass;
using Random = UnityEngine.Random;

public class RandomMinionSpawning : MonoBehaviour
{
    [SerializeField] uint maxEnitys;
    [SerializeField] float cycleDelay;

    [SerializeField] float maxDisFromPlayer;
    [SerializeField] float minDisFromPlayer;
    Transform player;

    [SerializeField] List<RandSpawnLocations> randSpawnLocations;
    //[SerializeField] float spawnZValue;
    float cycleTimer;
    public void AddSpawnLoacation(RandSpawnLocations randSpawn)
    {
        randSpawnLocations.Add(randSpawn);
    }
    void Start()
    {
        player = GameObject.Find("Player").transform;
        cycleTimer = cycleDelay;
        
        foreach (RandSpawnLocations location in randSpawnLocations)
        {
            // set max/min
            location.maxPoint = location.recParam.position + new Vector3(location.recParam.sizeDelta.x, location.recParam.sizeDelta.y);
            location.minPoint = location.recParam.position;
            
            /*if (location != null && location.minions.Length > 0)
            {
                int randMinion = Random.Range(0, location.minions.Length);
                float randX = Random.Range(location.minPoint.x, location.maxPoint.x);
                float randY = Random.Range(location.minPoint.y, location.maxPoint.y);
                Vector3 randomPosition = new Vector3(randX, randY, spawnZValue);
                
                GameObject minion = Instantiate(location.minions[randMinion], randomPosition, Quaternion.identity);
            }*/
        }
        
    }

    private void Spawn(RandSpawnLocations locaton)
    {
        if (locaton.minions.Length <= 0 || EntityManager.instance.minions.Count >= maxEnitys)
            return;

        float xSpawn;
        RaycastHit2D hit;

        int cycels = 0;
        while (true)
        {
            if (cycels >= 50)
                return;

            cycels++;
             xSpawn = Random.Range(locaton.minPoint.x, locaton.maxPoint.x);
            //Debug.Log("SpawnIn: "+ locaton.maxPoint.y +" - " +  locaton.minPoint.y + " = "+ (locaton.maxPoint.y - locaton.minPoint.y));
            hit = Physics2D.Raycast(new Vector2(xSpawn, Random.Range(locaton.minPoint.y, locaton.maxPoint.y)), Vector2.down, (locaton.maxPoint.y - locaton.minPoint.y)*2, ~256);
            if (hit.collider == null)
                continue;

            Vector2 enitySize;
            GameObject enity = locaton.minions[Random.Range(0, locaton.minions.Length)].minion;
            if (enity.GetComponent<SpriteRenderer>() == null)
                enitySize = enity.GetComponentInChildren<SpriteRenderer>().bounds.size;
            else
                enitySize = enity.GetComponent<SpriteRenderer>().bounds.size;

            if (Physics2D.Raycast(hit.point, Vector2.up, enitySize.y).collider != null)
                continue;
            if (Physics2D.Raycast(hit.point + Vector2.left * enitySize.x * 0.5f, Vector2.right, enitySize.x).collider != null)
                continue;

            break;
        }
        int minIndex = Random.Range(0, locaton.minions.Length);
        GameObject minSpawn = Instantiate(locaton.minions[minIndex].minion, hit.point, Quaternion.identity);
        minSpawn.SetActive(true);

        if (locaton.minions[minIndex].radomizeBattleSatats)
        {
            for (int i = 0; i < minSpawn.GetComponent<MinionWondering>().battleMinions.Length; i++)
            {
                MinionClass.MinionStats minionStats = minSpawn.GetComponent<MinionWondering>().battleMinions[i].stats;
                if (locaton.minions[minIndex].minionsRndStats.ATK > 0)
                    minionStats.ATK = Math.Abs(minionStats.ATK + Random.Range(-locaton.minions[minIndex].minionsRndStats.ATK, locaton.minions[minIndex].minionsRndStats.ATK));
                if (locaton.minions[minIndex].minionsRndStats.AttackSpeed > 0)
                    minionStats.AttackSpeed = MathF.Round(Math.Abs(minionStats.AttackSpeed + Random.Range(-locaton.minions[minIndex].minionsRndStats.AttackSpeed, locaton.minions[minIndex].minionsRndStats.AttackSpeed)) * 100) / 100;
                if (locaton.minions[minIndex].minionsRndStats.HP > 0)
                    minionStats.HP = Math.Abs(minionStats.HP + Random.Range(-locaton.minions[minIndex].minionsRndStats.HP - GameData.difficultyMultiplayer * 3, locaton.minions[minIndex].minionsRndStats.HP+GameData.difficultyMultiplayer*5));
                if (locaton.minions[minIndex].minionsRndStats.Speed > 0)
                    minionStats.Speed = MathF.Round(Math.Abs(minionStats.Speed + Random.Range(-locaton.minions[minIndex].minionsRndStats.Speed, locaton.minions[minIndex].minionsRndStats.Speed)) * 100) / 100;
                if (locaton.minions[minIndex].minionsRndStats.Range > 0)
                    minionStats.Range = MathF.Round(Math.Abs(minionStats.Range + Random.Range(-locaton.minions[minIndex].minionsRndStats.Range, locaton.minions[minIndex].minionsRndStats.Range)) * 100) / 100;
                //if (locaton.minions[minIndex].minionsRndStats.Cost > 0)
                //    minionStats.Cost = Math.Abs(minionStats.Cost + Random.Range(-locaton.minions[minIndex].minionsRndStats.Cost, locaton.minions[minIndex].minionsRndStats.Cost));
            }

        }
    }

    void Update()
    {
        if (cycleTimer > 0)
            cycleTimer -= Time.deltaTime;
        else
        {
            foreach(RandSpawnLocations location in randSpawnLocations)
            {
                if (location.recParam.position.x - player.position.x > maxDisFromPlayer || Mathf.Abs(location.recParam.position.x - player.position.x) < minDisFromPlayer)
                    return;
                Debug.DrawRay(location.recParam.position, (location.recParam.position.x - player.position.x)*Vector2.left,Color.yellow,1f);
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
public class minRndStats
{
    public GameObject minion;
    public bool radomizeBattleSatats;
    public MinionStats minionsRndStats;
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
    
    public minRndStats[] minions;
    /*public GameObject[] minions;
    [Header("Random Stats")]
    public bool radomizeBattleSatats;
    public MinionStats minionsRndStats;*/

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
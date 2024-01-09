using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WhyDoIExist
{
    [System.Serializable]
    class MinionPreciseSpawn// : MonoBehaviour
    {
        public GameObject minion;
        public Transform spawnPosition;
    }
}
public class MinionSpawning : MonoBehaviour
{
    [SerializeField] MinionPreciseSpawn[] minions;

    void MinonSpawn(MinionPreciseSpawn spawnMinion)
    {
        GameObject a = Instantiate(spawnMinion.minion,spawnMinion.spawnPosition);
        a.transform.SetParent(transform,true);
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach(MinionPreciseSpawn a in minions)
        {
            MinonSpawn(a);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

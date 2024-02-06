using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;
public class MinionSpawning : MonoBehaviour
{
    [System.Serializable]
    class MinionPreciseSpawn
    {
        public GameObject minion;
        public Transform spawnPosition;
        public bool radomizeBattleSatats;
        public MinionClass.MinionStats minionsRndStats;

    }
    [SerializeField] MinionDeck minionDeck;
    [SerializeField] PlayerParty playerDeck;
    [SerializeField] MinionPreciseSpawn[] minions;

    void MinonSpawn(MinionPreciseSpawn spawnMinion)
    {
        GameObject a = Instantiate(spawnMinion.minion,spawnMinion.spawnPosition);
        a.transform.SetParent(transform,true);

        if (spawnMinion.radomizeBattleSatats)
        {
            for (int i = 0; i < a.GetComponent<MinionWondering>().battleMinions.Length; i++)
            {
                MinionClass.MinionStats minionStats = a.GetComponent<MinionWondering>().battleMinions[i].stats;
                if (spawnMinion.minionsRndStats.ATK > 0)
                    minionStats.ATK = Math.Abs(minionStats.ATK + Random.Range(-spawnMinion.minionsRndStats.ATK, spawnMinion.minionsRndStats.ATK));
                if (spawnMinion.minionsRndStats.AttackSpeed > 0)
                    minionStats.AttackSpeed = Math.Abs(minionStats.AttackSpeed + Random.Range(-spawnMinion.minionsRndStats.AttackSpeed, spawnMinion.minionsRndStats.AttackSpeed));
                if (spawnMinion.minionsRndStats.HP > 0)
                    minionStats.HP = Math.Abs(minionStats.HP + Random.Range(-spawnMinion.minionsRndStats.HP, spawnMinion.minionsRndStats.HP));
                if (spawnMinion.minionsRndStats.Speed > 0)
                    minionStats.Speed = Math.Abs(minionStats.Speed + Random.Range(-spawnMinion.minionsRndStats.Speed, spawnMinion.minionsRndStats.Speed));
                if (spawnMinion.minionsRndStats.Range > 0)
                    minionStats.Range = Math.Abs(minionStats.Range + Random.Range(-spawnMinion.minionsRndStats.Range, spawnMinion.minionsRndStats.Range));
                if (spawnMinion.minionsRndStats.Cost > 0)
                    minionStats.Cost = Math.Abs(minionStats.Cost + Random.Range(-spawnMinion.minionsRndStats.Cost, spawnMinion.minionsRndStats.Cost));
            }

        }
        
    }

    // Start is called before the first frame update
    private void Awake()
    {
        minionDeck.SetInstance();
        playerDeck.SetInstance();
    }
    void Start()
    {

        foreach (MinionPreciseSpawn a in minions)
        {
            MinonSpawn(a);
        }
    }
}

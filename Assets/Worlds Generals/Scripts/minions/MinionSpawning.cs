using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;
public class MinionSpawning : MonoBehaviour
{
    [SerializeField] MinionClass.MinionPreciseSpawn[] minions;

    public void MinonSpawn(MinionClass.MinionPreciseSpawn spawnMinion)
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
                    minionStats.AttackSpeed = MathF.Round(Math.Abs(minionStats.AttackSpeed + Random.Range(-spawnMinion.minionsRndStats.AttackSpeed, spawnMinion.minionsRndStats.AttackSpeed)) * 100) / 100;
                if (spawnMinion.minionsRndStats.HP > 0)
                    minionStats.HP = Math.Abs(minionStats.HP + Random.Range(-spawnMinion.minionsRndStats.HP, spawnMinion.minionsRndStats.HP));
                if (spawnMinion.minionsRndStats.Speed > 0)
                    minionStats.Speed = MathF.Round(Math.Abs(minionStats.Speed + Random.Range(-spawnMinion.minionsRndStats.Speed, spawnMinion.minionsRndStats.Speed)) * 100) / 100;
                if (spawnMinion.minionsRndStats.Range > 0)
                    minionStats.Range = MathF.Round(Math.Abs(minionStats.Range + Random.Range(-spawnMinion.minionsRndStats.Range, spawnMinion.minionsRndStats.Range)) * 100) / 100;
                if (spawnMinion.minionsRndStats.Cost > 0)
                    minionStats.Cost = Math.Abs(minionStats.Cost + Random.Range(-spawnMinion.minionsRndStats.Cost, spawnMinion.minionsRndStats.Cost));
            }

        }
        
    }

    // Start is called before the first frame update
    private void Awake()
    {
        MinionDeck.SetInstance();
    }
    void Start()
    {

        foreach (MinionClass.MinionPreciseSpawn a in minions)
        {
            MinonSpawn(a);
        }
    }
}

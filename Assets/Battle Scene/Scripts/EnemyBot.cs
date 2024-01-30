using System;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;
public class EnemyBot : MonoBehaviour
{
    [SerializeField] Transform spawnPos;
    [SerializeField] GameObject []minions;
    [SerializeField] MinionDeck minionDeckSaveData;

    // If Scripting. Can be Removed if Game is done
    bool IfScripting()
    {
        return GetComponent<BattleCanvas>().isScripting;
    }

    int[] costs;
    float timer;
    [SerializeField] float resetTimer;
    int mana;
    // Start is called before the first frame update
    void Start()
    {
        if (!IfScripting()) // Remove When Game is doen
            minionDeckSaveData.CreateMinionsPrefabs();
        string[] guids = minionDeckSaveData.PrefabPathsForLoad();
        minions = new GameObject[guids.Length];
        for (int i = 0; i < guids.Length; i++)
        {
            minions[i] = Resources.Load(guids[i]) as GameObject;
            minions[i].SetActive(true);
            //Debug.Log(guids[i]);
        }

        costs = new int[minions.Length];
        for (int i = 0; i < minions.Length; i++)
        {
            costs[i] = minions[i].GetComponent<MinionBattleBasic>().stats.Cost;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (timer <= 0)
        {
            mana += 1;
            TrySummon();
            timer += resetTimer;
        }
        else
            timer -= Time.deltaTime;
    }
    void TrySummon()
    {
        int index = Random.Range(0, minions.Length);

        if (costs[index] <= mana)
        {
            mana -= costs[index];
            GameObject minion = Instantiate(minions[Random.Range(0, minions.Length)], spawnPos.position, Quaternion.identity);
        }
    }
}

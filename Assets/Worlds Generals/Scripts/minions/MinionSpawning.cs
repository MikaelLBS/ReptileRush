using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
public class MinionSpawning : MonoBehaviour
{
    [System.Serializable]
    class MinionsRndStats// : MonoBehaviour
    {
        public float ATK;
        public float AttackSpeed;
        public float HP;
        public float Speed;
        public float Range;
    }
    [System.Serializable]
    class MinionPreciseSpawn// : MonoBehaviour
    {
        public GameObject minion;
        public Transform spawnPosition;
        public MinionsRndStats minionStats;
        public bool radomizeBattleSatats;
        public MinionsRndStats minionsRndStats;

    }

    [SerializeField] MinionPreciseSpawn[] minions;
    public uint minionID = 0;

    void MinonSpawn(MinionPreciseSpawn spawnMinion)
    {
        GameObject a = Instantiate(spawnMinion.minion,spawnMinion.spawnPosition);
        a.transform.SetParent(transform,true);

        if (spawnMinion.radomizeBattleSatats)
        {
            GameObject[] b = a.GetComponent<MinionWondering>().battleMinions;
            MinionBattleBasic minionsBattleScrip;

            for (int i = 0; i < b.Length; i++)
            {
                minionsBattleScrip = b[i].GetComponent<MinionBattleBasic>();

                spawnMinion.minionStats.ATK = minionsBattleScrip.ATK + Random.Range(-spawnMinion.minionsRndStats.ATK, spawnMinion.minionsRndStats.ATK);
                spawnMinion.minionStats.ATK  = minionsBattleScrip.AttackSpeed + Random.Range(-spawnMinion.minionsRndStats.AttackSpeed, spawnMinion.minionsRndStats.AttackSpeed);
                spawnMinion.minionStats.ATK = minionsBattleScrip.HP + spawnMinion.minionsRndStats.HP;
                spawnMinion.minionStats.ATK = minionsBattleScrip.Speed + spawnMinion.minionsRndStats.Speed;
                spawnMinion.minionStats.ATK = minionsBattleScrip.Range + spawnMinion.minionsRndStats.Range;

                //a.GetComponent<MinionWondering>().battleMinions[i].GetComponent<MinionBattleBasic>() = minionsBattleScrip;
            }

        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach(MinionPreciseSpawn a in minions)
        {
            /*
            GameObject b = Instantiate(a.minion, a.spawnPosition);
            b.name = a.minion.name;

            string localPath = "Assets/Prefabs/" + b.name + minionID + ".prefab";
            // Make sure the file name is unique, in case an existing Prefab has the same name.
            localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);
            PrefabUtility.SaveAsPrefabAssetAndConnect(b, localPath, InteractionMode.AutomatedAction);
            minionID++;

            Destroy(b);
            */

            // LOOK IN TO "SetActiveRecursively"

            MinonSpawn(a);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

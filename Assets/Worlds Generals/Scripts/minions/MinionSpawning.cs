using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;
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

            for (int i = 0; i < spawnMinion.minion.GetComponent<MinionWondering>().battleMinions.Length; i++)
            {
                var minionStats = spawnMinion.minion.GetComponent<MinionWondering>().battleMinions[i].stats;
                if (minionStats.ATK < 0)
                    minionStats.ATK += Random.Range(-spawnMinion.minionsRndStats.ATK, spawnMinion.minionsRndStats.ATK);
                if (minionStats.AttackSpeed < 0)
                    minionStats.AttackSpeed += Random.Range(-spawnMinion.minionsRndStats.AttackSpeed, spawnMinion.minionsRndStats.AttackSpeed);
                if (minionStats.HP < 0)
                    minionStats.HP += Random.Range(-spawnMinion.minionsRndStats.HP, spawnMinion.minionsRndStats.HP);
                if (minionStats.Speed < 0)
                    minionStats.Speed += Random.Range(-spawnMinion.minionsRndStats.Speed, spawnMinion.minionsRndStats.Speed);
                if (minionStats.Range < 0)
                    minionStats.Range += Random.Range(-spawnMinion.minionsRndStats.Range, spawnMinion.minionsRndStats.Range);
            }

        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        string[] prefabsPlath = new string[1];
        prefabsPlath[0] = "Assets/Prefabs";
        string[] guids = AssetDatabase.FindAssets("t:Prefab", prefabsPlath);

        foreach (string guid in guids)
        {
            //Debug.Log(AssetDatabase.GUIDToAssetPath(guid));
            //GameObject t = (GameObject)AssetDatabase.LoadAssetAtPath(guid, typeof(GameObject));
            //GameObject.Instantiate((UnityEngine.Object)Resources.Load(guid), Vector3.zero, Quaternion.identity);
            AssetDatabase.DeleteAsset(AssetDatabase.GUIDToAssetPath(guid));
        }

        foreach (MinionPreciseSpawn a in minions)
        {

            GameObject b = Instantiate(a.minion, a.spawnPosition);
            b.name = a.minion.name;

            string localPath = "Assets/Prefabs/" + b.name + minionID + ".prefab";
            localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);

            PrefabUtility.SaveAsPrefabAssetAndConnect(b, localPath, InteractionMode.AutomatedAction);
            minionID++;
            
            Destroy(b);

            MinonSpawn(a);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static MinionClass;

[CreateAssetMenu]
public class MinionDeck : ScriptableObject
{
    public static MinionDeck Instance { get; private set; }
    public void SetInstance()
    {
        Instance = this;
    }

    public GameObject basicBattleMinion; // Minion Used if minion GameObject dont exist
    public MinionClass.BattleMinion[] minions;

    // Creats prefabs of minions in Assets/resources/Prefabs/MinionDeck/
    public void CreateMinionsPrefabs()
    {
        
        foreach (MinionClass.BattleMinion minion in minions)
        {
            if (minion.minion == null)
                minion.minion = basicBattleMinion;
            minion.minion.SetActive(false);
            GameObject minionGameObject = Instantiate(minion.minion);

            if (minion.animator != null)
                minionGameObject.GetComponent<Animator>().runtimeAnimatorController = minion.animator; // MabeWorks

            MinionBattleBasic basicMinionBattle = minionGameObject.GetComponent<MinionBattleBasic>();
            if (minion.stats.ATK >= 0)
                basicMinionBattle.stats.ATK = minion.stats.ATK;
            if (minion.stats.AttackSpeed >= 0)
                basicMinionBattle.stats.AttackSpeed = minion.stats.AttackSpeed;
            if (minion.stats.Speed >= 0)
                basicMinionBattle.stats.Speed = minion.stats.Speed;
            if (minion.stats.Range >= 0)
                basicMinionBattle.stats.Range = minion.stats.Range;
            if (minion.stats.HP >= 0)
                basicMinionBattle.stats.HP = minion.stats.HP;
            if (minion.stats.Cost >= 0)
                basicMinionBattle.stats.Cost = minion.stats.Cost;

            basicMinionBattle.isEnemy = true;

            string localPath = "Assets/resources/Prefabs/MinionDeck/" + minion.minion.name + ".prefab";
            localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);
            PrefabUtility.SaveAsPrefabAssetAndConnect(minionGameObject, localPath, InteractionMode.AutomatedAction);
            Destroy(minionGameObject);
            minion.minion.SetActive(true);

        }
    }
    // Gets all the paths of the prebabed minions
    public string[] PrefabPaths()
    {
        string[] prefabsPlath = new string[1];
        prefabsPlath[0] = "Assets/resources/Prefabs/MinionDeck";
        string[] guids = AssetDatabase.FindAssets("t:Prefab", prefabsPlath);
        return guids;
    }
    // Gets all the paths of the prebabed minions in a form that you can use in the Resources.Load() function
    public string[] PrefabPathsForLoad()
    {
        string[] prefabsPlath = new string[1];
        prefabsPlath[0] = "Assets/resources/Prefabs/MinionDeck";
        string[] guids = AssetDatabase.FindAssets("t:Prefab", prefabsPlath);
        for (int i = 0; i < guids.Length; i++)
        {
            guids[i] = AssetDatabase.GUIDToAssetPath(guids[i]);
            guids[i] = guids[i].Remove(0,17);
            int dotPos = 0;
            for (int j = 0; j < guids[i].Length; j++)
            {
                if (guids[i][j] == '.')
                    dotPos = j;
            }
            guids[i] = guids[i].Remove(dotPos, 7);
        }

        return guids;
    }

}

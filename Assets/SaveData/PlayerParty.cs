using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu]
public class PlayerParty : ScriptableObject
{
    public static PlayerParty Instance { get; private set; }
    public void SetInstance()
    {
        Instance = this;
    }
    public GameObject[] minions;

    // Creats prefabs of minions in Assets/resources/Prefabs/PlayerDeck/
    public void CreateMinionsPrefabs()
    {

        foreach (GameObject minion in minions)
        {
            minion.SetActive(false);
            GameObject minionGameObject = Instantiate(minion);

            //MinionBattleBasic basicMinionBattle = minionGameObject.GetComponent<MinionBattleBasic>();
            minionGameObject.GetComponent<MinionBattleBasic>().isEnemy = false;

            string localPath = "Assets/resources/Prefabs/PlayerDeck/" + minion.name + ".prefab";
            localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);
            PrefabUtility.SaveAsPrefabAssetAndConnect(minionGameObject, localPath, InteractionMode.AutomatedAction);
            Destroy(minionGameObject);
            minion.SetActive(true);
        }
    }
    // Gets all the paths of the prebabed minions
    public string[] PrefabPaths()
    {
        string[] prefabsPlath = new string[1];
        prefabsPlath[0] = "Assets/resources/Prefabs/PlayerDeck";
        string[] guids = AssetDatabase.FindAssets("t:Prefab", prefabsPlath);
        return guids;
    }
    // Gets all the paths of the prebabed minions in a form that you can use in the Resources.Load() function
    public string[] PrefabPathsForLoad()
    {
        string[] prefabsPlath = new string[1];
        prefabsPlath[0] = "Assets/resources/Prefabs/PlayerDeck";
        string[] guids = AssetDatabase.FindAssets("t:Prefab", prefabsPlath);
        for (int i = 0; i < guids.Length; i++)
        {
            guids[i] = AssetDatabase.GUIDToAssetPath(guids[i]);
            guids[i] = guids[i].Remove(0, 17);
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

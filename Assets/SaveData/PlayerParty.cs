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
    public List<MinionClass.MinionSave> minions;
    public GameObject[] loadedMinions;
    public short maxMinions;
    public int sceneIndex;

    // Creates a prefab of the inputed GameObject in Assets/resources/Prefabs/PlayerDeck/
    public void AddMinion(GameObject minion)
    {
        minion.SetActive(false);
        GameObject minionGameObject = Instantiate(minion);

        minionGameObject.GetComponent<MinionBattleBasic>().isEnemy = false;

        MinionClass.MinionSave minionSave = new(minionGameObject.GetComponent<MinionBattleBasic>(), minionGameObject.GetComponent<RuntimeAnimatorController>());
        minions.Add(minionSave);
    }

    // Creats prefabs of minions in Assets/resources/Prefabs/PlayerDeck/
    public GameObject[] LoadMinions()
    {
        int i = 0;
        loadedMinions = new GameObject[minions.Count];
        foreach (MinionClass.MinionSave minion in minions)
        {
            minion.minion.SetActive(false);
            GameObject minionGameObject = Instantiate(minion.minion);

            if (minion.animator != null)
                minionGameObject.GetComponent<Animator>().runtimeAnimatorController = minion.animator; // MabeWorks

            MinionBattleBasic basicMinionBattle = minionGameObject.GetComponent<MinionBattleBasic>();
            basicMinionBattle.basePrefab = minion.minion;
            basicMinionBattle.partyIndex = minion.slotIndex;
            basicMinionBattle.icon = minion.icon;

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

            //minion.minionName = 
            basicMinionBattle.minionName = minion.minionName;

            basicMinionBattle.isEnemy = false;

            loadedMinions[i] = minionGameObject;
            i++;
        }
        return loadedMinions;
    }
}

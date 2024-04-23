using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor;
using UnityEngine;
using static MinionClass;

[CreateAssetMenu]
public class MinionDeck : ScriptableObject
{
    public static MinionDeck Instance { get; private set; }
    public static void SetInstance()
    {
        if (Instance == null)
            Instance = Resources.Load<MinionDeck>("MinionDeck");
    }

    public GameObject basicBattleMinion; // Minion Used if minion GameObject dont exist
    public MinionClass.BattleMinion[] minions;
    public uint minionsAmount;
    public GameObject[] loadedMinions { get; private set; }

    // Creats prefabs of minions in Assets/resources/Prefabs/MinionDeck/
    public GameObject[] LoadMinions()
    {
        loadedMinions = new GameObject[minions.Length];
        int i = 0;
        foreach (MinionClass.BattleMinion minion in minions)
        {
            if (minion.minion == null)
                minion.minion = basicBattleMinion;
            minion.minion.SetActive(false);
            GameObject minionGameObject = Instantiate(minion.minion);

            if (minion.animator != null)
                minionGameObject.GetComponent<Animator>().runtimeAnimatorController = minion.animator; // MabeWorks

            MinionBattleBasic basicMinionBattle = minionGameObject.GetComponent<MinionBattleBasic>();
            basicMinionBattle.basePrefab = minion.minion;

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

            basicMinionBattle.stats.HP += GameData.difficultyMultiplayer;

            basicMinionBattle.minionName = minion.minionName;
            if (minion.icon == null)
                minion.icon = basicBattleMinion.GetComponent<MinionBattleBasic>().icon;
            basicMinionBattle.icon = minion.icon;

            if (basicMinionBattle.GetComponent<SpriteRenderer>() == null)
                basicMinionBattle.GetComponentInChildren<SpriteRenderer>().color = minion.color;
            else
                basicMinionBattle.GetComponent<SpriteRenderer>().color = minion.color;

            basicMinionBattle.stats.Cost = basicMinionBattle.stats.HP / 10 + basicMinionBattle.stats.ATK + (int)Mathf.Round(basicMinionBattle.stats.AttackSpeed + basicMinionBattle.stats.Speed);
            //Debug.Log(basicMinionBattle.stats.HP / 10 + " + " + basicMinionBattle.stats.ATK + " + " + (int)Mathf.Round(basicMinionBattle.stats.AttackSpeed + basicMinionBattle.stats.Speed));

            basicMinionBattle.isEnemy = true;
            loadedMinions[i] = minionGameObject;
            i++;

        }
        return loadedMinions;
    }

}

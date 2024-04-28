using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu]
public class PlayerParty : ScriptableObject, IDataPersitiens
{
    public static PlayerParty Instance { get; private set; }

    public static void SetInstance()
    {
        if (Instance == null)
            Instance = Resources.Load<PlayerParty>("PlayerParty");
    }
    public List<MinionClass.MinionSave> minions;
    [NonSerialized] public GameObject[] loadedMinions;
    public short maxMinions;

    public int sceneIndex; // used when exiting battle to get to correct world

    [NonSerialized] public bool wonBattle;
    [NonSerialized] public bool isGameOver;
    [NonSerialized] public bool isExitingBattle;
    [NonSerialized] public bool isBoss;

    [NonSerialized] public bool battleHasEnded;

    // Creates a prefab of the inputed GameObject in Assets/resources/Prefabs/PlayerDeck/
    public void AddMinion(GameObject minion)
    {
        minion.SetActive(false);
        GameObject minionGameObject = Instantiate(minion);

        MinionBattleBasic bScript = minionGameObject.GetComponent<MinionBattleBasic>();
        bScript.isEnemy = false;
        if (bScript.basePrefab == null)
            bScript.basePrefab = Resources.Load<GameObject>("Battles/" + EntityManager.GetPrefabName(minion.name));
        MinionClass.MinionSave minionSave = new(minionGameObject.GetComponent<MinionBattleBasic>(), minionGameObject.GetComponent<RuntimeAnimatorController>());
        minionSave.stats.HP -= GameData.difficultyMultiplayer;
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

            if (basicMinionBattle.GetComponent<SpriteRenderer>() == null)
                basicMinionBattle.GetComponentInChildren<SpriteRenderer>().color = minion.color;
            else
                basicMinionBattle.GetComponent<SpriteRenderer>().color = minion.color;

            basicMinionBattle.minionName = minion.minionName;

            basicMinionBattle.isEnemy = false;

            loadedMinions[i] = minionGameObject;
            i++;
        }
        return loadedMinions;
    }
    public void LoadData(GameData data)
    {
        sceneIndex = data.sceneIndex;
        loadedMinions = new GameObject[0];

        for (int i = 0; i < data.PartyMinions.Length; i++)
        {
            minions[i].LoadFromGeneric(data.PartyMinions[i].minionSave);

            minions[i].minion = Resources.Load<GameObject>(data.PartyMinions[i].minionPrefabPath);
            minions[i].icon = Resources.Load<Sprite>(data.PartyMinions[i].iconAssetPath);
            minions[i].animator = Resources.Load<RuntimeAnimatorController>(data.PartyMinions[i].animatorAssetPath);
        }
    }
    public void SaveData(ref GameData data)
    {
        data.sceneIndex = sceneIndex;

        data.PartyMinions = new MinionClass.MinionFileSave[minions.Count];
        for (int i = 0; i < minions.Count; i++)
        {
            data.PartyMinions[i].minionSave = new MinionClass.GenericMinionSave(minions[i]);
            data.PartyMinions[i].minionPrefabPath = "Battles/"+minions[i].minion.name;
            data.PartyMinions[i].animatorAssetPath = "Anime/" + minions[i].animator.name;
            data.PartyMinions[i].iconAssetPath = "Anime/" + minions[i].icon.name;
        }
    }
}

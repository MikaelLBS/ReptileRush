using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EntityManager : MonoBehaviour, IDataPersitiens
{
    public static EntityManager instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        PlayerParty.SetInstance();
    }

    public List<GameObject> minions = new List<GameObject>();
    public void AddMinion(GameObject minion)
    {
        minions.Add(minion);
    }
    public void LoadData(GameData data)
    {
        if (data.WildMinions == null)
            return;

        foreach (MinionClass.WildMinionSave wild in data.WildMinions)
        {

            if (wild.hasEnterdBattle && PlayerParty.Instance.isExitingBattle)
            {
                wild.hasEnterdBattle = false;
                PlayerParty.Instance.isExitingBattle = false;
                if (PlayerParty.Instance.wonBattle)
                    continue;
            }

            //Debug.Log("Loaded minion: " + wild.wildMinionPrefabPath);
            GameObject minion = Instantiate(Resources.Load<GameObject>(wild.wildMinionPrefabPath));
            MinionWondering minionWonderingScript = minion.GetComponent<MinionWondering>();
            minionWonderingScript.battleMinions = new MinionClass.BattleMinion[wild.battleMinions.Length];
            for (int i = 0; i < minionWonderingScript.battleMinions.Length; i++)
            {
                minionWonderingScript.battleMinions[i] = new MinionClass.BattleMinion();
                minionWonderingScript.battleMinions[i].LoadFromGeneric(wild.battleMinions[i].minionSave);

                minionWonderingScript.battleMinions[i].minion = Resources.Load<GameObject>(wild.battleMinions[i].minionPrefabPath);
                minionWonderingScript.battleMinions[i].icon = Resources.Load<Sprite>(wild.battleMinions[i].iconAssetPath);
                minionWonderingScript.battleMinions[i].animator = Resources.Load<RuntimeAnimatorController>(wild.battleMinions[i].animatorAssetPath);
            }
            minion.transform.position = new Vector2(wild.worldCoords.x, wild.worldCoords.y);
            minionWonderingScript.speed = wild.speed;
            minionWonderingScript.randomTimer = new Vector2 (wild.randomTimer.x, wild.randomTimer.y);
            minionWonderingScript.hasEnterdBattle = wild.hasEnterdBattle;

            if (wild.hasEnterdBattle)
            {
                minionWonderingScript.AddMinionDeck();
                SceneManager.LoadScene("Battle");
            }
        }
    }
    public void SaveData(ref GameData data)
    {
        data.startSpawnForMinionsWorld1 = false;
        data.WildMinions = new MinionClass.WildMinionSave[minions.Count];
        for (int i = 0; i < minions.Count; i++)
        {
            data.WildMinions[i] = new MinionClass.WildMinionSave();
            MinionWondering minionWonderingScript = minions[i].GetComponent<MinionWondering>();
            data.WildMinions[i].battleMinions = new MinionClass.BattleMinionSave[minionWonderingScript.battleMinions.Length];

            data.WildMinions[i].wildMinionPrefabPath = "Wilds/" + GetPrefabName(minions[i].name);

            for (int j = 0; j < minionWonderingScript.battleMinions.Length; j++)
            {
                data.WildMinions[i].battleMinions[j] = new MinionClass.BattleMinionSave();
                if (minionWonderingScript.battleMinions[j].minion != null)
                    data.WildMinions[i].battleMinions[j].minionPrefabPath = "Battles/" + GetPrefabName(minionWonderingScript.battleMinions[j].minion.name);
                if (minionWonderingScript.battleMinions[j].icon != null)
                    data.WildMinions[i].battleMinions[j].iconAssetPath = "Icons/" + minionWonderingScript.battleMinions[j].icon.name;
                if (minionWonderingScript.battleMinions[j].animator != null)
                    data.WildMinions[i].battleMinions[j].animatorAssetPath = "Anime/" + minionWonderingScript.battleMinions[j].animator.name;

                data.WildMinions[i].battleMinions[j].minionSave = new MinionClass.GenericBattleMinion(minionWonderingScript.battleMinions[j]);
            }
            // Save the rest
            data.WildMinions[i].worldCoords = (minions[i].transform.position.x, minions[i].transform.position.y);
            data.WildMinions[i].speed = minionWonderingScript.speed;
            data.WildMinions[i].randomTimer = (minionWonderingScript.randomTimer.x, minionWonderingScript.randomTimer.y);
            data.WildMinions[i].hasEnterdBattle = minionWonderingScript.hasEnterdBattle;
        }
    }
    string GetPrefabName(string instanceName)
    {
        for (int i = 0;i < instanceName.Length;i++)
        {
            if (instanceName[i] == '(')
                return instanceName.Remove(i);
        }
        return instanceName;
    }
}

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
        for (int i = 0; i < data.WildMinions.Length; i++)
        {
            if (data.WildMinions[i].hasEnterdBattle && PlayerParty.Instance.isExitingBattle)
            {
                data.WildMinions[i].hasEnterdBattle = false;
                if (PlayerParty.Instance.wonBattle)
                    continue;
            }
            GameObject minion = SaveToWild(ref data.WildMinions[i], true);
            if (data.WildMinions[i].hasEnterdBattle)
            {
                minion.GetComponent<MinionWondering>().AddMinionDeck();
                SceneManager.LoadScene("Battle");
            }
        }
    }
    public void SaveData(ref GameData data)
    {
        PlayerParty.Instance.isExitingBattle = false;

        data.startSpawnForMinionsWorld1 = false;
        data.WildMinions = new MinionClass.WildMinionSave[minions.Count];
        for (int i = 0; i < minions.Count; i++)
        {
            GameObject tempMinion = minions[i];
            WildToSave(ref tempMinion, ref data.WildMinions[i]);
        }
    }
    public static GameObject SaveToWild(ref MinionClass.WildMinionSave wild, bool isActive)
    {
        if (wild == null)
            return null;
        GameObject minion = Instantiate(Resources.Load<GameObject>(wild.wildMinionPrefabPath));
        minion.SetActive(isActive);
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
        minionWonderingScript.randomTimer = new Vector2(wild.randomTimer.x, wild.randomTimer.y);
        minionWonderingScript.hasEnterdBattle = wild.hasEnterdBattle;

        return minion;
    }
    public static void WildToSave(ref GameObject minion, ref MinionClass.WildMinionSave save)
    {
        save = new MinionClass.WildMinionSave();
        MinionWondering minionWonderingScript = minion.GetComponent<MinionWondering>();
        save.battleMinions = new MinionClass.BattleMinionSave[minionWonderingScript.battleMinions.Length];

        save.wildMinionPrefabPath = "Wilds/" + GetPrefabName(minion.name);

        for (int j = 0; j < minionWonderingScript.battleMinions.Length; j++)
        {
            save.battleMinions[j] = new MinionClass.BattleMinionSave();
            if (minionWonderingScript.battleMinions[j].minion != null)
                save.battleMinions[j].minionPrefabPath = "Battles/" + GetPrefabName(minionWonderingScript.battleMinions[j].minion.name);
            if (minionWonderingScript.battleMinions[j].icon != null)
                save.battleMinions[j].iconAssetPath = "Icons/" + minionWonderingScript.battleMinions[j].icon.name;
            if (minionWonderingScript.battleMinions[j].animator != null)
                save.battleMinions[j].animatorAssetPath = "Anime/" + minionWonderingScript.battleMinions[j].animator.name;

            save.battleMinions[j].minionSave = new MinionClass.GenericBattleMinion(minionWonderingScript.battleMinions[j]);
        }
        // Save the rest
        save.worldCoords = (minion.transform.position.x, minion.transform.position.y);
        save.speed = Mathf.Abs(minionWonderingScript.speed);
        save.randomTimer = (minionWonderingScript.randomTimer.x, minionWonderingScript.randomTimer.y);
        save.hasEnterdBattle = minionWonderingScript.hasEnterdBattle;
    }
    static string GetPrefabName(string instanceName)
    {
        for (int i = 0;i < instanceName.Length;i++)
        {
            if (instanceName[i] == '(')
                return instanceName.Remove(i);
        }
        return instanceName;
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EntityManager : MonoBehaviour, IDataPersitiens
{
    static Color worldColor;
    public static EntityManager instance;
    int amountOflevels;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        PlayerParty.SetInstance();
        worldColor = new Color(0, 0.1f, 0.1f, 0) * GameData.difficultyMultiplayer;
    }

    public List<GameObject> minions = new List<GameObject>();
    public void AddMinion(GameObject minion)
    {
        minions.Add(minion);
    }
    IEnumerator bossEnd()
    {
        GenerationHandeler genHandeler = FindFirstObjectByType<GenerationHandeler>();
        Transform player = GameObject.Find("Player").transform;
        float time = 2;
        float phaseTimer = 0.15f;
        float setPhaseTimer = 0.15f;
        while (time > 0)
        {

            if (phaseTimer <= 0)
            {
                genHandeler.RemoveTile(player.position+Vector3.down*2);
                genHandeler.RemoveTile(player.position + Vector3.down);

                for (int x = -2; x < 3; x++)
                {
                    for (int y = -10; y < 3; y++)
                    {
                        if (Random.Range(0,2) == 0)
                            genHandeler.RemoveTile(player.position + new Vector3(x,y));
                    }
                }

                phaseTimer += setPhaseTimer;
                setPhaseTimer -= 0.01f;
            }

            phaseTimer -= Time.fixedDeltaTime;
            time -= Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (GameData.difficultyMultiplayer == amountOflevels-1)
            sceneIndex = 3;

        PlayerParty.Instance.sceneIndex = 3;
        DataPersistenceManager.Instance.NewLevelDataReset();
        DataPersistenceManager.Instance.WriteSaveFile();
        SceneManager.LoadScene(sceneIndex);
    }
    public void LoadData(GameData data)
    {
        amountOflevels = data.amountOfLevels;

        if (data.WildMinions == null)
            return;
        for (int i = 0; i < data.WildMinions.Length; i++)
        {
            if (data.WildMinions[i].hasEnterdBattle && PlayerParty.Instance.isExitingBattle)
            {
                data.WildMinions[i].hasEnterdBattle = false;
                if (PlayerParty.Instance.wonBattle)
                {
                    if (data.WildMinions[i].isBoss)
                    {
                        if (data.hasEnterFinalBoss)
                            victoryScreen.SetActive(true);
                        else
                            StartCoroutine(bossEnd());
                    }
                    continue;
                }
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
            if (tempMinion.GetComponent<MinionWondering>().isBoss)
            {
                CustomWildToSave(ref tempMinion, ref data.WildMinions[i], "Bosses/");
            }
            else
                WildToSave(ref tempMinion, ref data.WildMinions[i]);
            //Debug.Log(data.WildMinions[i].wildMinionPrefabPath);

        }
        //Debug.Log("------");
    }
    public static GameObject SaveToWild(ref MinionClass.WildMinionSave wild, bool isActive)
    {
        if (wild == null)
            return null;
        //Debug.Log(wild.wildMinionPrefabPath);
        GameObject tempMinion = Resources.Load<GameObject>(wild.wildMinionPrefabPath);
        if (tempMinion == null)
            return null;
        GameObject minion = Instantiate(tempMinion);
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

            (float r, float g, float b, float a) color = wild.battleMinions[i].color;
            minionWonderingScript.battleMinions[i].color = new Color(color.r, color.g, color.b, color.a);

        }
        minion.transform.position = new Vector2(wild.worldCoords.x, wild.worldCoords.y);
        minionWonderingScript.speed = wild.speed;
        minionWonderingScript.randomTimer = new Vector2(wild.randomTimer.x, wild.randomTimer.y);
        minionWonderingScript.hasEnterdBattle = wild.hasEnterdBattle;

        if (minion.GetComponent<SpriteRenderer>() == null)
            minionWonderingScript.GetComponentInChildren<SpriteRenderer>().color = new Color(wild.color.r, wild.color.g, wild.color.b, wild.color.a)- worldColor;
        else
            minionWonderingScript.GetComponent<SpriteRenderer>().color = new Color(wild.color.r, wild.color.g, wild.color.b, wild.color.a) - worldColor;
        minionWonderingScript.transform.localScale = new Vector2(wild.size.x, wild.size.y);
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

            Color color = minionWonderingScript.battleMinions[j].color;
            save.battleMinions[j].color = (color.r, color.g, color.b, color.a);

            save.battleMinions[j].minionSave = new MinionClass.GenericBattleMinion(minionWonderingScript.battleMinions[j]);
        }
        // Save the rest
        save.worldCoords = (minion.transform.position.x, minion.transform.position.y);
        save.speed = Mathf.Abs(minionWonderingScript.speed);
        save.randomTimer = (minionWonderingScript.randomTimer.x, minionWonderingScript.randomTimer.y);
        save.hasEnterdBattle = minionWonderingScript.hasEnterdBattle;

        Color minionColor;
        if (minion.GetComponent<SpriteRenderer>() == null)
            minionColor = minion.GetComponentInChildren<SpriteRenderer>().color;
        else
            minionColor = minion.GetComponent<SpriteRenderer>().color;
        save.color = (minionColor.r,minionColor.g,minionColor.b,minionColor.a);
        save.size = (minion.transform.localScale.x, minion.transform.localScale.y);
    }
    public static void CustomWildToSave(ref GameObject minion, ref MinionClass.WildMinionSave save, string wildPath)
    {
        save = new MinionClass.WildMinionSave();
        MinionWondering minionWonderingScript = minion.GetComponent<MinionWondering>();
        save.battleMinions = new MinionClass.BattleMinionSave[minionWonderingScript.battleMinions.Length];

        save.wildMinionPrefabPath = wildPath + GetPrefabName(minion.name);

        for (int j = 0; j < minionWonderingScript.battleMinions.Length; j++)
        {
            save.battleMinions[j] = new MinionClass.BattleMinionSave();
            if (minionWonderingScript.battleMinions[j].minion != null)
                save.battleMinions[j].minionPrefabPath = "Battles/" + GetPrefabName(minionWonderingScript.battleMinions[j].minion.name);
            if (minionWonderingScript.battleMinions[j].icon != null)
                save.battleMinions[j].iconAssetPath = "Icons/" + minionWonderingScript.battleMinions[j].icon.name;
            if (minionWonderingScript.battleMinions[j].animator != null)
                save.battleMinions[j].animatorAssetPath = "Anime/" + minionWonderingScript.battleMinions[j].animator.name;

            Color color = minionWonderingScript.battleMinions[j].color;
            save.battleMinions[j].color = (color.r, color.g, color.b, color.a);

            save.battleMinions[j].minionSave = new MinionClass.GenericBattleMinion(minionWonderingScript.battleMinions[j]);
        }
        // Save the rest
        save.worldCoords = (minion.transform.position.x, minion.transform.position.y);
        save.speed = Mathf.Abs(minionWonderingScript.speed);
        save.randomTimer = (minionWonderingScript.randomTimer.x, minionWonderingScript.randomTimer.y);
        save.hasEnterdBattle = minionWonderingScript.hasEnterdBattle;
        save.isBoss = true;

        Color minionColor;
        if (minion.GetComponent<SpriteRenderer>() == null)
            minionColor = minion.GetComponentInChildren<SpriteRenderer>().color;
        else
            minionColor = minion.GetComponent<SpriteRenderer>().color;
        save.color = (minionColor.r, minionColor.g, minionColor.b, minionColor.a);
        save.size = (minion.transform.localScale.x, minion.transform.localScale.y);
    }
    public static string GetPrefabName(string instanceName)
    {
        for (int i = 0;i < instanceName.Length;i++)
        {
            if (instanceName[i] == '(')
                return instanceName.Remove(i);
        }
        return instanceName;
    }
    [SerializeField] GameObject victoryScreen;
    // test things
    [SerializeField] bool bossEndTrigger;
    private void OnValidate()
    {
        if (bossEndTrigger)
        {
            bossEndTrigger = false;
            StartCoroutine(bossEnd());
        }
    }
}

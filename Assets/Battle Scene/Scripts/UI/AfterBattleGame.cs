//using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class AfterBattleGame : MonoBehaviour
{
    [SerializeField] EnemyBot bot;
    [SerializeField] RectTransform buttonHolder;
    [SerializeField] GameObject buttonPrefab;
    [SerializeField] float pictureSize;
    Button[] summonButtons;
    [Header("RewardMinion")]
    [SerializeField] GameObject rewardMinionButton;
    [SerializeField] TMPro.TextMeshProUGUI nameText;
    [SerializeField] TMPro.TextMeshProUGUI costText;
    //string rewardMinionPrefabPath;
    [Header("Stats Box")]
    [SerializeField] float yOffPut;
    [SerializeField] GameObject statsHolder;
    [SerializeField] TMPro.TextMeshProUGUI statsBoxMinionName;
    [SerializeField] TMPro.TextMeshProUGUI statsBoxStats;
    [SerializeField] float fadeSpeed;
    IEnumerator fadeInIEnumerator;
    void CreateButtons()
    {
        Vector2 startEndPosX = new Vector2(buttonHolder.position.x - buttonHolder.sizeDelta.x / 2, buttonHolder.position.x + buttonHolder.sizeDelta.x / 2);
        float sizeBetweenStartAndEndPoints = startEndPosX.y - startEndPosX.x;

        // if button is lager then placeHolder make the picture size smaller
        if (pictureSize * (PlayerParty.Instance.minions.Count + 1) > sizeBetweenStartAndEndPoints)
            pictureSize = sizeBetweenStartAndEndPoints / (PlayerParty.Instance.minions.Count + 2);

        float distanceBetweenPic = (sizeBetweenStartAndEndPoints) / PlayerParty.Instance.minions.Count;

        float distance = startEndPosX.x + pictureSize / 2 - distanceBetweenPic + (sizeBetweenStartAndEndPoints - pictureSize * PlayerParty.Instance.loadedMinions.Length ) / PlayerParty.Instance.loadedMinions.Length / 2;

        // creates buttons
        summonButtons = new Button[PlayerParty.Instance.minions.Count];
        float[] xCoordsButtons = new float[PlayerParty.Instance.minions.Count];
        for (int i = 0; i < PlayerParty.Instance.minions.Count; i++)
        {
            distance += distanceBetweenPic;
            xCoordsButtons[i] = distance;
        }
        for (int i = 0; i < PlayerParty.Instance.minions.Count; i++)
        {
            MinionBattleBasic minData = PlayerParty.Instance.loadedMinions[i].GetComponent<MinionBattleBasic>();

            //distance += distanceBetweenPic;
            GameObject button = Instantiate(buttonPrefab, new Vector2(xCoordsButtons[minData.partyIndex], buttonHolder.position.y), Quaternion.identity);
            button.transform.SetParent(transform);
            button.GetComponent<RectTransform>().sizeDelta = Vector2.one * pictureSize;
            button.GetComponent<Image>().sprite = minData.icon;

            AfterGameMinionButton afterBattleGame = button.GetComponent<AfterGameMinionButton>();
            afterBattleGame.indexInPlayerParty = i;
            afterBattleGame.minionnName = minData.minionName;
            afterBattleGame.minionBattleScript = minData;
            //button.GetComponent<AfterGameMinionButton>().index = i;

            button.GetComponent<ShowHoveringIcon>().afterBattleGameScript = this;

            summonButtons[i] = button.GetComponent<Button>();
        }
    }
    void CreateRewardMinion()
    {
        
        GameObject rewMinion = bot.minions[Random.Range(0,bot.minions.Length)];
        MinionBattleBasic rewMinionData = rewMinion.GetComponent<MinionBattleBasic>();
        PlayerParty.Instance.AddMinion(rewMinion);
        rewardMinionButton.transform.SetParent(transform);
        rewardMinionButton.GetComponent<Image>().sprite = rewMinionData.icon;
        
        nameText.text = rewMinionData.minionName;
        costText.text = rewMinionData.stats.Cost.ToString();
        
    }
    public void RewardMinionButtonDown()
    {
        PlayerParty.Instance.minions.RemoveAt(PlayerParty.Instance.minions.Count-1);
        SceneManager.LoadScene(PlayerParty.Instance.sceneIndex);
    }
    public void ShowMinionStats(int index,Vector2 pos)
    {
        if (fadeInIEnumerator != null)
            StopCoroutine(fadeInIEnumerator);
        fadeInIEnumerator = FadeInStatsBox(true);
        StartCoroutine(fadeInIEnumerator);

        if (index == -1)
            index = PlayerParty.Instance.minions.Count-1;
        statsHolder.transform.position = pos-Vector2.down*yOffPut;
        statsBoxMinionName.text = PlayerParty.Instance.minions[index].minionName;
        statsBoxStats.text = UppdateStatsText(PlayerParty.Instance.minions[index].stats);
    }
    IEnumerator FadeInStatsBox(bool fadeIn)
    {
        Image image = statsHolder.GetComponent<Image>();
        Color color = image.color;
        if (fadeIn)
        {
            statsHolder.SetActive(true);
            color.a = 0;
        }

        int tempMulti = -1;
        if (fadeIn)
            tempMulti = 1;

        while (true)
        {
            color.a += Time.deltaTime*tempMulti*fadeSpeed;
            image.color = color;
            if (fadeIn && color.a >= 1f)
                break;
            else if (!fadeIn && color.a <= 0f)
                break;
            yield return null;
        }
        if (!fadeIn)
            statsHolder.SetActive(false);
        color.a = 1;
        image.color = color;
    }
    public void HideMinionStats()
    {
        if (fadeInIEnumerator != null)
            StopCoroutine(fadeInIEnumerator);
        fadeInIEnumerator = FadeInStatsBox(false);
        StartCoroutine(fadeInIEnumerator);
    }
    string UppdateStatsText(MinionClass.MinionStats stats)
    {
        string statsText =
            "ATK: " + stats.ATK +
            "\nATK Speed: " + Mathf.Round(1 / stats.AttackSpeed * 100) / 100 +
            "\nHP: " + stats.HP +
            "\nRange: " + Mathf.Round(stats.Range * 10) +
            "\nSpeed: " + Mathf.Round(stats.Speed * 10) +
            "\nCost: " + stats.Cost;

        return statsText;
    }
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerParty.Instance.minions.Count == PlayerParty.Instance.maxMinions)
        {
            CreateButtons();
            CreateRewardMinion();
        }
        else if (PlayerParty.Instance.minions.Count < PlayerParty.Instance.maxMinions)
        {
            PlayerParty.Instance.AddMinion(bot.minions[Random.Range(0, bot.minions.Length)]);
            SceneManager.LoadScene(PlayerParty.Instance.sceneIndex);
        }
        else
        {
            PlayerParty.Instance.minions.RemoveAt(PlayerParty.Instance.minions.Count - 1);
            SceneManager.LoadScene(PlayerParty.Instance.sceneIndex);
        }
        // creating Prefab
        /*string[] guids = PlayerParty.Instance.GetPrefabPathsForLoad();
        if (guids.Length == PlayerParty.Instance.maxMinions)
        {
            minions = new GameObject[guids.Length];
            for (int i = 0; i < guids.Length; i++)
            {
                minions[i] = Resources.Load(guids[i]) as GameObject;
                minions[i].SetActive(true);

            }
            CreateButtons();
            CreateRewardMinion();
        }
        else if (guids.Length > PlayerParty.Instance.maxMinions)
        {
            guids = PlayerParty.Instance.GetPrefabPaths();
            for (int i = 0; i < guids.Length- PlayerParty.Instance.maxMinions+1; i++)
                AssetDatabase.DeleteAsset(AssetDatabase.GUIDToAssetPath(guids[i]));

            GameObject rewMinion = bot.minions[Random.Range(0, bot.minions.Length)];
            rewardMinionPrefabPath = PlayerParty.Instance.AddMinion(rewMinion);
            SceneManager.LoadScene(PlayerParty.Instance.sceneIndex);
        }
        else
        {
            GameObject rewMinion = bot.minions[Random.Range(0, bot.minions.Length)];
            rewardMinionPrefabPath = PlayerParty.Instance.AddMinion(rewMinion);

            SceneManager.LoadScene(PlayerParty.Instance.sceneIndex);
        }*/
    }
}

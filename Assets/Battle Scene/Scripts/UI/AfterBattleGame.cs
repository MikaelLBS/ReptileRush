//using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
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
    GameObject[] minions;
    Button[] summonButtons;
    [Header("RewardMinion")]
    [SerializeField] GameObject rewardMinionButton;
    [SerializeField] TMPro.TextMeshProUGUI nameText;
    [SerializeField] TMPro.TextMeshProUGUI costText;
    string rewardMinionPrefabPath;
    void CreateButtons()
    {
        Vector2 startEndPosX = new Vector2(buttonHolder.position.x - buttonHolder.sizeDelta.x / 2, buttonHolder.position.x + buttonHolder.sizeDelta.x / 2);
        float sizeBetweenStartAndEndPoints = startEndPosX.y - startEndPosX.x;

        // if button is lager then placeHolder make the picture size smaller
        if (pictureSize * (minions.Length + 1) > sizeBetweenStartAndEndPoints)
            pictureSize = sizeBetweenStartAndEndPoints / (minions.Length + 2);

        float distanceBetweenPic = (sizeBetweenStartAndEndPoints) / minions.Length;

        float distance = startEndPosX.x + pictureSize / 2 - distanceBetweenPic + (sizeBetweenStartAndEndPoints - pictureSize * minions.Length) / minions.Length / 2;

        string[] guids = PlayerParty.Instance.GetPrefabPaths();
        // creates buttons
        summonButtons = new Button[minions.Length];
        for (int i = 0; i < minions.Length; i++)
        {
            MinionBattleBasic minData = minions[i].GetComponent<MinionBattleBasic>();

            distance += distanceBetweenPic;
            GameObject button = Instantiate(buttonPrefab, new Vector2(distance, buttonHolder.position.y), Quaternion.identity);
            button.transform.SetParent(transform);
            button.GetComponent<RectTransform>().sizeDelta = Vector2.one * pictureSize;
            button.GetComponent<Image>().sprite = minData.icon;

            AfterGameMinionButton afterBattleGame = button.GetComponent<AfterGameMinionButton>();
            afterBattleGame.path = guids[i];
            afterBattleGame.minionnName = minData.minionName;
            afterBattleGame.minionBattleScript = minData;
            //button.GetComponent<AfterGameMinionButton>().index = i;

            summonButtons[i] = button.GetComponent<Button>();
        }
    }
    void CreateRewardMinion()
    {
        GameObject rewMinion = bot.minions[Random.Range(0,bot.minions.Length)];
        MinionBattleBasic rewMinionData = rewMinion.GetComponent<MinionBattleBasic>();
        rewardMinionPrefabPath = PlayerParty.Instance.AddMinion(rewMinion);
        rewardMinionButton.transform.SetParent(transform);
        rewardMinionButton.GetComponent<Image>().sprite = rewMinionData.icon;

        nameText.text = rewMinionData.minionName;
        costText.text = rewMinionData.stats.Cost.ToString();
    }
    public void RewardMinionButtonDown()
    {
        AssetDatabase.DeleteAsset(rewardMinionPrefabPath);
        SceneManager.LoadScene(PlayerParty.Instance.sceneIndex);
    }
    // Start is called before the first frame update
    void Start()
    {
        string[] guids = PlayerParty.Instance.GetPrefabPathsForLoad();
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
        }
    }
}

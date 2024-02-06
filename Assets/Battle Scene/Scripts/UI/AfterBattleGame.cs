//using System;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    //GameObject[] minions;
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
            CreateRewardMinion();
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

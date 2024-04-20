using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using static MinionBattleSpecial;

public class PlayerUI : MonoBehaviour
{
    SwapButtonPlayerUI[] buttonScripts;
    float[] xCoordsButtons;
    float[] xCoordsButtonsSplitLines;

    [SerializeField] float pictureSize; // button size
    [SerializeField] RectTransform buttonHolder; // where the button are placed
    [SerializeField] GameObject buttonPrefab; // minion summon button
    void CreateButtons()
    {
        Vector2 startEndPosX = new Vector2(buttonHolder.position.x - buttonHolder.sizeDelta.x / 2, buttonHolder.position.x + buttonHolder.sizeDelta.x / 2);
        float sizeBetweenStartAndEndPoints = startEndPosX.y - startEndPosX.x;

        // if button is lager then placeHolder make the picture size smaller
        if (pictureSize * (PlayerParty.Instance.minions.Count + 1) > sizeBetweenStartAndEndPoints)
        {
            pictureSize = sizeBetweenStartAndEndPoints / (PlayerParty.Instance.minions.Count + 2);
            Debug.Log("Trigger");
        }

        float distanceBetweenPic = (sizeBetweenStartAndEndPoints) / PlayerParty.Instance.minions.Count;

        float distance = startEndPosX.x + pictureSize / 2 - distanceBetweenPic + (sizeBetweenStartAndEndPoints - pictureSize * PlayerParty.Instance.minions.Count) / PlayerParty.Instance.minions.Count / 2;

        // creates buttons
        buttonScripts = new SwapButtonPlayerUI[PlayerParty.Instance.minions.Count];
        xCoordsButtons = new float[PlayerParty.Instance.minions.Count];
        xCoordsButtonsSplitLines = new float[PlayerParty.Instance.minions.Count -1];
        for (int i = 0; i < PlayerParty.Instance.minions.Count;i++)
        {
            distance += distanceBetweenPic;
            xCoordsButtons[i] = distance;
            if (i != PlayerParty.Instance.minions.Count - 1)
                xCoordsButtonsSplitLines[i] = distance + distanceBetweenPic / 2;
        }
        for (int i = 0; i < PlayerParty.Instance.minions.Count; i++)
        {
            MinionClass.MinionSave minData = PlayerParty.Instance.minions[i];

            GameObject button = Instantiate(buttonPrefab, new Vector2(xCoordsButtons[minData.slotIndex], buttonHolder.position.y), Quaternion.identity);
            button.transform.SetParent(buttonHolder);
            button.GetComponent<RectTransform>().sizeDelta = Vector2.one * pictureSize;
            button.GetComponent<Image>().sprite = minData.icon;

            SwapButtonPlayerUI buttonScriptPlayerUI = button.GetComponent<SwapButtonPlayerUI>();
            buttonScriptPlayerUI.minion = minData.minion;
            buttonScriptPlayerUI.startIndex = i;
            buttonScriptPlayerUI.index = minData.slotIndex;
            buttonScriptPlayerUI.xSplitLines = xCoordsButtonsSplitLines;
            buttonScriptPlayerUI.playerUI = this;

            if (buttonScripts[minData.slotIndex] == null)
                buttonScripts[minData.slotIndex] = buttonScriptPlayerUI;
            else
            {
                for (int j = 0; j < buttonScripts.Length;j++)
                    if (buttonScripts[j] == null)
                    {
                        buttonScripts[j] = buttonScriptPlayerUI;
                        minData.slotIndex = j;
                        button.transform.position = new Vector2(xCoordsButtons[j], buttonHolder.position.y);
                        break;
                    }
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerParty.Instance.isGameOver)
            return;
        DataPersistenceManager.Instance.LoadPartyData();
        CreateButtons();
    }
    public void UppdateButtonsPos(int buttonIndex, int movedIndex)
    {

        SwapButtonPlayerUI tempSwapButton = buttonScripts[buttonIndex];
        SwapButtonPlayerUI tempButton = buttonScripts[movedIndex];
        if (buttonIndex > movedIndex)
        {
            for (int i = 0; i < buttonScripts.Length; i++)
            {
                if (i < movedIndex+1 || i > buttonIndex)
                    continue;
                SwapButtonPlayerUI tempButton2 = buttonScripts[i];
                buttonScripts[i] = tempButton;
                tempButton = tempButton2;

            }
        }
        else
        {
            for (int i = buttonScripts.Length-1; i >= 0; i--)
            {
                if (i > movedIndex-1 || i < buttonIndex)
                    continue;
                SwapButtonPlayerUI tempButton2 = buttonScripts[i];
                buttonScripts[i] = tempButton;
                tempButton = tempButton2;

            }
        }
        buttonScripts[movedIndex] = tempSwapButton;
        for (int i = 0; i < buttonScripts.Length;i++)
        {
            buttonScripts[i].index = i;
            PlayerParty.Instance.minions[buttonScripts[i].startIndex].slotIndex = i;
        }

        foreach (SwapButtonPlayerUI button in buttonScripts)
        {
            if (button.index == movedIndex)
            {
                button.transform.position = new Vector2 (xCoordsButtons[button.index], button.transform.position.y);
                continue;
            }
            button.MoveTo(xCoordsButtons[button.index], 50);
        }
    }

    // Player Open inv
    [Header("Player Inventory")]
    [SerializeField] GameObject inv;

    [SerializeField] float pictureSize2; // button size
    [SerializeField] RectTransform buttonHolder2; // where the button are placed
    [SerializeField] GameObject buttonPrefab2;
    [SerializeField] InvStatsObjects invStats;
    float[] yCoordsButtons;
    GameObject[] minonButtonsInv;
    GameObject tempButton;

    [System.Serializable]
    class InvStatsObjects
    {
        public GameObject holder;
        public TMPro.TextMeshProUGUI minionName;
        public TMPro.TextMeshProUGUI statsText;
        public TMPro.TextMeshProUGUI abilityText;
    }
    void CreateButtonsInv()
    {
        Vector2 startEndPosX = new Vector2(buttonHolder2.position.y - buttonHolder2.sizeDelta.y / 2, buttonHolder2.position.y + buttonHolder2.sizeDelta.y / 2);
        float sizeBetweenStartAndEndPoints = startEndPosX.y - startEndPosX.x;
        // if button is lager then placeHolder make the picture size smaller
        if (pictureSize2 * (PlayerParty.Instance.minions.Count + 1) > sizeBetweenStartAndEndPoints)
            pictureSize2 = sizeBetweenStartAndEndPoints / (PlayerParty.Instance.minions.Count + 2);

        float distanceBetweenPic = (sizeBetweenStartAndEndPoints) / PlayerParty.Instance.minions.Count;
        float pictureSize22222222e22e2e2e2e2e2e22e2e2ev2ee2e2e2e2e2e2e2e2e2e2e2e2e222222222222222eeeeeeeeeeeeeeeeeeeeeeeeeeeeeee2e2e2eee2222222222222222222eeeee = pictureSize2;
        float distance = startEndPosX.x + pictureSize2 / 2 - distanceBetweenPic + (sizeBetweenStartAndEndPoints - pictureSize22222222e22e2e2e2e2e2e22e2e2ev2ee2e2e2e2e2e2e2e2e2e2e2e2e222222222222222eeeeeeeeeeeeeeeeeeeeeeeeeeeeeee2e2e2eee2222222222222222222eeeee * PlayerParty.Instance.minions.Count) / PlayerParty.Instance.minions.Count / 2;

        // creates buttons
        tempButton = null;
        yCoordsButtons = new float[PlayerParty.Instance.minions.Count];
        minonButtonsInv = new GameObject[PlayerParty.Instance.minions.Count];
        for (int i = 0; i < PlayerParty.Instance.minions.Count; i++)
        {
            distance += distanceBetweenPic;
            yCoordsButtons[i] = distance;
        }
        for (int i = 0; i < PlayerParty.Instance.minions.Count; i++)
        {
            MinionClass.MinionSave minData = PlayerParty.Instance.minions[i];

            tempButton = Instantiate(buttonPrefab2, new Vector2(buttonHolder2.position.x-buttonHolder2.sizeDelta.x/2+pictureSize2/2+50, yCoordsButtons[minData.slotIndex]), Quaternion.identity);
            tempButton.transform.SetParent(buttonHolder2);
            tempButton.GetComponent<RectTransform>().sizeDelta = Vector2.one * pictureSize2;
            tempButton.GetComponent<Image>().sprite = minData.icon;
            minonButtonsInv[i] = tempButton;

            InvButtonUI buttonScriptPlayerUI = tempButton.GetComponent<InvButtonUI>();

            buttonScriptPlayerUI.minionStats = minData.stats;
            buttonScriptPlayerUI.minionName = minData.minionName;
            buttonScriptPlayerUI.index = minData.slotIndex;
            buttonScriptPlayerUI.playerUI = this;

            MinionBattleSpecial minDataBattleSpecial = minData.minion.GetComponent<MinionBattleSpecial>();
            if (minDataBattleSpecial != null && minDataBattleSpecial.specals != null)
                buttonScriptPlayerUI.abilityType = minDataBattleSpecial.specals[0];
        }
    }

    public void OpenInv()
    {
        inv.SetActive (true);
        Time.timeScale = 0;
        CreateButtonsInv();
        if (tempButton != null)
            tempButton.GetComponent<InvButtonUI>().ButtonDown();
    }
    public void CloseInv()
    {
        inv.SetActive(false);
        Time.timeScale = 1;
        for (int i = 0; i < minonButtonsInv.Length; i++)
        { Destroy(minonButtonsInv[i]); }
    
    }

    public void ShowStats(MinionClass.MinionStats stats,string name)
    {
        invStats.holder.SetActive(true);
        invStats.statsText.text = UppdateStatsText(stats);
        invStats.minionName.text = name;
    }
    public void HideStats()
    {
        invStats.holder.SetActive(false);
    }
    string UppdateStatsText(MinionClass.MinionStats stats)
    {
        string statsText =
            "ATK: "+ stats.ATK*10 +
            "\nATK Speed: "+ Mathf.Round(1 /stats.AttackSpeed*100)/100 +
            "\nHP: "+stats.HP*10 + 
            "\nRange: "+ Mathf.Round(stats.Range * 10) +
            "\nSpeed: "+Mathf.Round(stats.Speed * 10) +
            "\nCost: "+stats.Cost;

        return statsText;
    }
    public void ShowAbility(Spacials ability)
    {
        invStats.abilityText.text = "";
        switch (ability)
        {
            case Spacials.Poison:
                //string[] hej = { "Ability: Poison", "Poisoned minons take damage over time" };
                ChangeAbilityText(new string[]{ "ABILITY: Poison", "Poisoned enemys take damage over time" });
                break;
                case Spacials.Thorns:
                ChangeAbilityText(new string[] { "ABILITY: Thorns", "if enemys hits a creature with thorns","they also take a samll amount of damage" });
                    break;
            case Spacials.AreaOfAttack:
                ChangeAbilityText(new string[] { "ABILITY: Area Of Attack", "Can hit several enemys per attack" });
                break;
            default:
                break;

        }
    }
    void ChangeAbilityText(string[] abilityText)
    {
        foreach (string s in abilityText)
        {
            invStats.abilityText.text += s+"\n";
        }
    }
}

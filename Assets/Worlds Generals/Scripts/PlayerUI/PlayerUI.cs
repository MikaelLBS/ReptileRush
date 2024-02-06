using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Header("Summon Buttons")]
    //[SerializeField] PlayerParty playerDeck;
    //[SerializeField] MinionDeck minionDeck;
    [SerializeField] GameObject[] minions; // minions in the player party
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
        if (pictureSize * (minions.Length + 1) > sizeBetweenStartAndEndPoints)
            pictureSize = sizeBetweenStartAndEndPoints / (minions.Length + 2);

        float distanceBetweenPic = (sizeBetweenStartAndEndPoints) / minions.Length;

        float distance = startEndPosX.x + pictureSize / 2 - distanceBetweenPic + (sizeBetweenStartAndEndPoints - pictureSize * minions.Length) / minions.Length / 2;

        // creates buttons
        buttonScripts = new SwapButtonPlayerUI[minions.Length];
        xCoordsButtons = new float[minions.Length];
        xCoordsButtonsSplitLines = new float[minions.Length-1];
        for (int i = 0; i < minions.Length;i++)
        {
            distance += distanceBetweenPic;
            xCoordsButtons[i] = distance;
            if (i != minions.Length - 1)
                xCoordsButtonsSplitLines[i] = distance + distanceBetweenPic / 2;
        }
        for (int i = 0; i < minions.Length; i++)
        {
            MinionBattleBasic minData = minions[i].GetComponent<MinionBattleBasic>();

            GameObject button = Instantiate(buttonPrefab, new Vector2(xCoordsButtons[minData.partyIndex], buttonHolder.position.y), Quaternion.identity);
            button.transform.SetParent(buttonHolder);
            button.GetComponent<RectTransform>().sizeDelta = Vector2.one * pictureSize;
            button.GetComponent<Image>().sprite = minData.icon;

            SwapButtonPlayerUI buttonScriptPlayerUI = button.GetComponent<SwapButtonPlayerUI>();
            buttonScriptPlayerUI.minion = minions[i];
            buttonScriptPlayerUI.index = minData.partyIndex;
            buttonScriptPlayerUI.xSplitLines = xCoordsButtonsSplitLines;
            buttonScriptPlayerUI.playerUI = this;

            if (buttonScripts[minData.partyIndex] == null)
                buttonScripts[minData.partyIndex] = buttonScriptPlayerUI;
            else
            {
                for (int j = 0; j < buttonScripts.Length;j++)
                    if (buttonScripts[j] == null)
                    {
                        buttonScripts[j] = buttonScriptPlayerUI;
                        minData.partyIndex = j;
                        button.transform.position = new Vector2(xCoordsButtons[j], buttonHolder.position.y);
                        break;
                    }
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        PlayerParty.Instance.Test = gameObject;
        string[] guids = PlayerParty.Instance.GetPrefabPathsForLoad();
        minions = new GameObject[guids.Length];
        for (int i = 0;i < guids.Length;i++)
        {
            minions[i] = Resources.Load(guids[i]) as GameObject;
        }
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
            minions[buttonScripts[i].startIndex].GetComponent<MinionBattleBasic>().partyIndex = i;
        }

        foreach (SwapButtonPlayerUI button in buttonScripts)
        {
            if (button.index == movedIndex)
            {
                button.transform.position = new Vector2 (xCoordsButtons[button.index], button.transform.position.y);
                continue;
            }
            button.MoveTo(xCoordsButtons[button.index], 5f);
        }
    }
}

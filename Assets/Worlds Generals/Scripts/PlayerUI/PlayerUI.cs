using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Header("Summon Buttons")]
    //[SerializeField] PlayerParty playerDeck;
    //[SerializeField] MinionDeck minionDeck;
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
            pictureSize = sizeBetweenStartAndEndPoints / (PlayerParty.Instance.minions.Count + 2);

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
            button.MoveTo(xCoordsButtons[button.index], 5f);
        }
    }
}

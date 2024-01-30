using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AfterBattleGame : MonoBehaviour
{
    [SerializeField] short maxMinions;
    [SerializeField] Transform spawnPos;
    [SerializeField] RectTransform buttonHolder;
    [SerializeField] GameObject buttonPrefab;
    [SerializeField] float pictureSize;
    [SerializeField] GameObject[] minions;
    Button[] summonButtons;
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
        summonButtons = new Button[minions.Length];
        for (int i = 0; i < minions.Length; i++)
        {
            MinionBattleBasic minData = minions[i].GetComponent<MinionBattleBasic>();

            distance += distanceBetweenPic;
            GameObject button = Instantiate(buttonPrefab, new Vector2(distance, buttonHolder.position.y), Quaternion.identity);
            button.transform.SetParent(transform);
            button.GetComponent<RectTransform>().sizeDelta = Vector2.one * pictureSize;
            button.GetComponent<Image>().sprite = minData.icon;
            button.GetComponent<BattleSummonButton>().minion = minions[i];
            button.GetComponent<BattleSummonButton>().spawnPos = spawnPos;
            button.GetComponent<BattleSummonButton>().index = i;

            summonButtons[i] = button.GetComponent<Button>();
        }
    }
    // Start is called before the first frame update
    void Start()
    {

        CreateButtons();
    }
}

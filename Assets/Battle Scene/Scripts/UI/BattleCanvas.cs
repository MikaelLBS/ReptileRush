using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleCanvas : MonoBehaviour
{
    [SerializeField] GameObject[] minions; // minions in the player party

    [SerializeField] float pictureSize; // button size
    [SerializeField] RectTransform buttonHolder; // where the button are placed
    [SerializeField] GameObject buttonPrefab; // minion summon button
    [SerializeField] Transform spawnPos; // where the minons going to spawn

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
        foreach (GameObject minion in minions)
        {
            MinionBattleBasic minData = minion.GetComponent<MinionBattleBasic>();

            distance += distanceBetweenPic;
            GameObject button = Instantiate(buttonPrefab, new Vector2(distance, buttonHolder.position.y), Quaternion.identity);
            button.transform.SetParent(transform);
            button.GetComponent<RectTransform>().sizeDelta = Vector2.one * pictureSize;
            button.GetComponent<Image>().sprite = minData.icon;
            button.GetComponent<BattleSummonButton>().minion = minion;
            button.GetComponent<BattleSummonButton>().spawnPos = spawnPos;
        }
    }



    [SerializeField] Slider manaFillBar;
    [SerializeField] TMPro.TextMeshProUGUI manaText;
    [SerializeField] float sekPerMana;
    [SerializeField] uint maxMana;

    float timer;
    [NonSerialized] public int mana;
    public void ChangeMana(int changeValue)
    {
        mana += changeValue;
        manaText.text = mana.ToString();
        manaFillBar.interactable = true;
        manaFillBar.value = mana;
        manaFillBar.interactable = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        CreateButtons();
        timer = sekPerMana;
        manaFillBar.maxValue = maxMana;
    }

    // Update is called once per frame
    void Update()
    {
        if ( mana < maxMana)
        {
            if (timer <= 0)
            {
                timer += sekPerMana;
                ChangeMana(1);
            }
            else
                timer -= Time.deltaTime;
        }
    }
}

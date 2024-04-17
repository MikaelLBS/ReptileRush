using System;
using UnityEngine;
using UnityEngine.UI;

class VectorInt2
{
    public VectorInt2(int cost)
    {
        x = cost;
        y = 1;
    }

    public int x;
    public int y;
}

public class BattleCanvas : MonoBehaviour
{
    [Header("Summon Buttons")]
    [SerializeField] GameObject[] minions; // minions in the player party
    Button[] summonButtons;
    VectorInt2[]minionsCost; // Variable used to check minion cost

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

        minionsCost = new VectorInt2[minions.Length];
        summonButtons = new Button[minions.Length];
        float[] xCoordsForButtons = new float[minions.Length];
        for (int i = 0; i < minions.Length; i++)
        {
            distance += distanceBetweenPic;
            xCoordsForButtons[i] = distance;
        }
        for (int i = 0; i < minions.Length;i++)
        {
            MinionBattleBasic minData = minions[i].GetComponent<MinionBattleBasic>();
            GameObject button = Instantiate(buttonPrefab, new Vector2(xCoordsForButtons[minData.partyIndex], buttonHolder.position.y), Quaternion.identity);
            button.transform.SetParent(buttonHolder);
            button.GetComponent<RectTransform>().sizeDelta = Vector2.one * pictureSize;
            button.GetComponent<Image>().sprite = minData.icon;

            BattleSummonButton battleButton = button.GetComponent<BattleSummonButton>();
            battleButton.minion = minions[i];
            battleButton.spawnPos = spawnPos;
            battleButton.index = i;

            minionsCost[i] = new VectorInt2(minData.stats.Cost);
            summonButtons[i] = button.GetComponent<Button>();
        }
    }


    [Header("Mana Regen")]
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

        CheckCosts();
    }
    public void disableCheckCost(int index) // disables checkin cost for a specific summon button
    { minionsCost[index].y -= 2; }
    public void enableCheckCost(int index) // enabeles checkin cost for a specific summon button. OBS only works if have been disabeld firt
    { minionsCost[index].y += 2; }
    void CheckCosts()
    {
        for (int i = 0; i < minions.Length; i++)
        {
            if (minionsCost[i].x <= mana && minionsCost[i].y == 0)
            {
                minionsCost[i].y++;
                summonButtons[i].interactable = true;
            } else if (minionsCost[i].x > mana && minionsCost[i].y == 1)
            {
                minionsCost[i].y--;
                summonButtons[i].interactable = false;
            }
        }
    }
    private void Awake()
    {
        PlayerParty.SetInstance();
        MinionDeck.SetInstance();
    }
    // Start is called before the first frame update
    void Start()
    {
        PlayerParty.Instance.battleHasEnded = false;
        minions = PlayerParty.Instance.LoadMinions();
        CreateButtons();
        timer = sekPerMana;
        manaFillBar.maxValue = maxMana;
        CheckCosts();
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
                CheckCosts();
            }
            else
                timer -= Time.deltaTime;
        }
        if (Input.anyKey)
        {
            for (int i = 0; i < 10; i++)
            {
                if (Input.GetButton("Hot " + (i + 1)))
                {
                    if (i < summonButtons.Length && summonButtons[i].IsInteractable())
                        summonButtons[i].GetComponent<BattleSummonButton>().ButtonDown();
                    break;
                }
            }
        }
    }
}

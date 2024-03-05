using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static MinionBattleSpecial;

public class InvButtonUI : MonoBehaviour
{
    public PlayerUI playerUI;
    public float[] xSplitLines;
    public int index;
    public string minionName;
    public MinionClass.MinionStats minionStats;
    public MinionClass.BattleMinion battleScript;
    public Spacials abilityType;
    [SerializeField] TMPro.TextMeshProUGUI nameText;
    private void Start()
    {
        nameText.text = minionName;
    }
    public void ButtonDown()
    {
        playerUI.ShowStats(minionStats, minionName);
        playerUI.ShowAbility(abilityType);
    }
}

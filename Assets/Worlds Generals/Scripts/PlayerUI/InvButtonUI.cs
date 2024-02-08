using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvButtonUI : MonoBehaviour
{
    public PlayerUI playerUI;
    public float[] xSplitLines;
    public int index;
    public string minionName;
    public MinionClass.MinionStats minionStats;
    public MinionClass.BattleMinion battleScript;
    [SerializeField] TMPro.TextMeshProUGUI nameText;
    private void Start()
    {
        nameText.text = minionName;
    }
    public void ButtonDown()
    { playerUI.ShowStats(minionStats, minionName); }
}

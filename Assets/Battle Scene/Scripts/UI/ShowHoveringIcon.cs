using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowHoveringIcon : MonoBehaviour
{
    int index = -1;
    public AfterBattleGame afterBattleGameScript;
    private void Start()
    {
        if (GetComponent<AfterGameMinionButton>() != null)
        {
            index = GetComponent<AfterGameMinionButton>().indexInPlayerParty;
        }
    }
    public GameObject hoveringIcon;
    public void HoverEnter()
    {
        hoveringIcon.SetActive(true);
        afterBattleGameScript.ShowMinionStats(index,transform.position);
    }
    public void HoverExit()
    {
        hoveringIcon.SetActive(false);
        afterBattleGameScript.HideMinionStats();
    }
}

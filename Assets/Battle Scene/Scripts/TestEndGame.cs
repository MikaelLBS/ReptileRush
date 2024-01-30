using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEndGame : MonoBehaviour
{
    [SerializeField] MinionDeck minionDeck;
    [SerializeField] PlayerParty playerDeck;
    public void BotWin()
    {
        Debug.Log("BotWin");

    }
    public void PlayerWin()
    {
        Debug.Log("PlayerWin");
    }
}

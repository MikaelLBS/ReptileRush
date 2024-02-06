using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestEndGame : MonoBehaviour
{

    [SerializeField] GameObject battleUI;
    [SerializeField] GameObject afterGameUI;
    [SerializeField] EnemyBot bot;
    public void BotWin()
    {
        Debug.Log("BotWin");
        SceneManager.LoadScene(PlayerParty.Instance.sceneIndex);
    }
    public void PlayerWin()
    {
        Debug.Log("PlayerWin");
        battleUI.SetActive(false);
        afterGameUI.SetActive(true);
        bot.enabled = false;
    }
}

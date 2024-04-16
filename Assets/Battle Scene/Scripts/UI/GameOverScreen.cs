using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    private void OnEnable()
    {
        PlayerParty.Instance.isGameOver = true;
        PlayerParty.Instance.isExitingBattle = false;
    }
    [SerializeField] float delay;
    private void Update()
    {
        if (Input.anyKeyDown)
        {
            Debug.Log("skip");
            delay = 0;
        }
        if (delay <= 0)
        {
            SceneManager.LoadScene(PlayerParty.Instance.sceneIndex);
        }
        else
            delay -= Time.deltaTime;
    }
}

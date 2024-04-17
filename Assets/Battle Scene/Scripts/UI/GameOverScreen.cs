using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] float delay;
    private void Update()
    {
        if (Input.anyKeyDown)
        {
            delay = 0;
        }
        if (delay <= 0)
        {
            PlayerParty.Instance.isGameOver = true;
            PlayerParty.Instance.isExitingBattle = false;
            SceneManager.LoadScene(PlayerParty.Instance.sceneIndex);
        }
        else
            delay -= Time.deltaTime;
    }
}

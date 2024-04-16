using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleEndScreen : MonoBehaviour
{
    [SerializeField] GameObject battleUI;
    [SerializeField] GameObject text;
    [SerializeField] float delay;
    [SerializeField] GameObject nextScreen;
    private void OnEnable()
    {
        text.SetActive(true);
    }
    void Update() {
        if (Input.anyKeyDown)
        {
            Debug.Log("skip");
            delay = 0;
        }

        if (delay <= 0)
        {
            PlayerParty.Instance.isExitingBattle = true;
            if (nextScreen == null)
            {
                PlayerParty.Instance.wonBattle = false;
                if (PlayerParty.Instance.minions.Count == 0)
                    nextScreen.SetActive(true);
                else
                    SceneManager.LoadScene(PlayerParty.Instance.sceneIndex);
            }
            else
            {
                PlayerParty.Instance.wonBattle = true;
                nextScreen.SetActive(true);
            }

            battleUI.SetActive(false);
            gameObject.SetActive(false);
        }
        else
            delay -= Time.deltaTime;
    }
}

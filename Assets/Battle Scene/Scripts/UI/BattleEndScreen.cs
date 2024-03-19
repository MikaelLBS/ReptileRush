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
        if (Input.GetButtonDown("Submit"))
        {
            delay = 0;
        }

        if (delay <= 0)
        {
            if (nextScreen == null)
                SceneManager.LoadScene(PlayerParty.Instance.sceneIndex);
            else
                nextScreen.SetActive(true);
            battleUI.SetActive(false);
            gameObject.SetActive(false);
        }
        else
            delay -= Time.deltaTime;
    }
}

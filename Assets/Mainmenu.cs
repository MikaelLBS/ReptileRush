using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mainmenu : MonoBehaviour, IDataPersitiens
{
    int sceneIndex;
    public void PlayGame()
    {
        DataPersistenceManager.Instance.SaveGame();
        SceneManager.LoadScene(sceneIndex);
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }

    public void NewRun()
    {
        DataPersistenceManager.Instance.NewGameData();
        PlayerParty.Instance.minions.Clear();
        DataPersistenceManager.Instance.SaveGame();
        DataPersistenceManager.Instance.SavePartyData();
        SceneManager.LoadScene(1);
    }

    public void LoadData(GameData data)
    {
        sceneIndex = data.sceneIndex;
    }
    public void SaveData(ref GameData data)
    {

    }
}

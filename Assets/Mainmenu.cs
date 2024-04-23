using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mainmenu : MonoBehaviour, IDataPersitiens
{
    int sceneIndex;
    public void PlayGame()
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }

    public void LoadData(GameData data)
    {
        sceneIndex = data.sceneIndex;
    }
    public void SaveData(ref GameData data)
    {

    }
}

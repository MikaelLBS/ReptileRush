using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScoreUI : MonoBehaviour, IDataPersitiens
{
    [SerializeField] TMPro.TextMeshProUGUI scoreText;

    public void LoadData(GameData data)
    {
        if (data.amountOfLevels == -1)
            scoreText.text = "Score: " + GameData.difficultyMultiplayer;
        else
            scoreText.gameObject.SetActive(false);
    }
    public void SaveData(ref GameData data)
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MinionClass;

public class devTools : MonoBehaviour
{
    [SerializeField] PlayerParty playerParty;
    [SerializeField] string fileName;
    [Header("Tools")]
    [SerializeField] bool resetFile;
    [SerializeField] bool getFilePath;
    private void OnValidate()
    {
        if (resetFile)
        {
            resetFile = false;
            //FileDataHandler handeler = new FileDataHandler(Application.persistentDataPath, saveFileName);
            new FileDataHandler(Application.persistentDataPath, fileName).Save(new());
            playerParty.minions.Clear();
            Debug.Log("Deleted SaveData File!");
        }
        if (getFilePath)
        {
            getFilePath = false;
            Debug.Log(Application.persistentDataPath);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MinionClass;

public class devTools : MonoBehaviour
{
    [SerializeField] string fileName;
    [Header("Tools")]
    [SerializeField] bool resetFile;
    private void OnValidate()
    {
        if (resetFile)
        {
            resetFile = false;
            //FileDataHandler handeler = new FileDataHandler(Application.persistentDataPath, saveFileName);
            new FileDataHandler(Application.persistentDataPath, fileName).Save(new());
            Debug.Log("Deleted SaveData File!");
        }
    }
}

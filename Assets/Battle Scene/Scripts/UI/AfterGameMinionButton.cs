using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AfterGameMinionButton : MonoBehaviour
{
    //[NonSerialized]
    public int indexInPlayerParty;
    public string minionnName;
    public MinionBattleBasic minionBattleScript;

    [SerializeField] TMPro.TextMeshProUGUI costText;
    [SerializeField] TMPro.TextMeshProUGUI nameText;

    void Start()
    {

        costText.text = minionBattleScript.stats.Cost.ToString();
        nameText.text = minionnName;
    }
    public void ButtonDown()
    {
        PlayerParty.Instance.minions.RemoveAt(indexInPlayerParty);
        // creating prefabs
        /*string[] guids = PlayerParty.Instance.GetPrefabPaths();
        AssetDatabase.DeleteAsset(AssetDatabase.GUIDToAssetPath(path));*/
        SceneManager.LoadScene(PlayerParty.Instance.sceneIndex);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AfterGameMinionButton : MonoBehaviour
{
    //[NonSerialized]
    public string path;
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
        string[] guids = PlayerParty.Instance.GetPrefabPaths();
        AssetDatabase.DeleteAsset(AssetDatabase.GUIDToAssetPath(path));
        SceneManager.LoadScene(PlayerParty.Instance.sceneIndex);
    }
}

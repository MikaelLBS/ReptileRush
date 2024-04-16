using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage")]
    [SerializeField] string fileName;

    private GameData gameData;
    private List<IDataPersitiens> dataPersistenceObjcets;

    public FileDataHandler DataHandler {  get; private set; }
    public static DataPersistenceManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        this.DataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        this.dataPersistenceObjcets = FindAllDataPersistenceObjects();
        LoadGame();
    }

    public void NewGameData()
    {
        gameData = new GameData();
    }
    public void LoadGame()
    {
        this.gameData = DataHandler.Load();
        //Debug.Log(Application.persistentDataPath);
        if (gameData == null)
        {
            NewGameData();
            Debug.Log("created savefile at: "+ Application.persistentDataPath+"/"+fileName);
        }

        foreach (IDataPersitiens dataPer in dataPersistenceObjcets)
            dataPer.LoadData(gameData);

    }
    public void SaveGame()
    {
        SavePartyData();
        foreach (IDataPersitiens dataPer in dataPersistenceObjcets)
            dataPer.SaveData(ref gameData);

        DataHandler.Save(gameData);
    }
    public void WriteSaveFile()
    { DataHandler.Save(gameData); }
    public void NewLevelDataReset()
    { gameData.NewLevelDataReset(); }
    private void OnApplicationQuit()
    {
        SaveGame();
    }
    private List<IDataPersitiens> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersitiens> dataPersistenceObjcets = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersitiens>();

        return new List<IDataPersitiens>(dataPersistenceObjcets);
    }

    // Player Party
    public void LoadPartyData()
    {
        if (PlayerParty.Instance == null || gameData.PartyMinions == null || PlayerParty.Instance.isExitingBattle == true)
            return;

        PlayerParty.Instance.sceneIndex = gameData.sceneIndex;

        PlayerParty.Instance.minions.Clear();
        for (int i = 0; i < gameData.PartyMinions.Length; i++)
        {
            if (i == PlayerParty.Instance.minions.Count)
                PlayerParty.Instance.minions.Add(new());
            PlayerParty.Instance.minions[i].LoadFromGeneric(gameData.PartyMinions[i].minionSave);

            PlayerParty.Instance.minions[i].minion = Resources.Load<GameObject>(gameData.PartyMinions[i].minionPrefabPath);
            PlayerParty.Instance.minions[i].icon = Resources.Load<Sprite>(gameData.PartyMinions[i].iconAssetPath);
            PlayerParty.Instance.minions[i].animator = Resources.Load<RuntimeAnimatorController>(gameData.PartyMinions[i].animatorAssetPath);
        }
    }
     void SavePartyData()
    {
        if (PlayerParty.Instance == null)
            return;

        gameData.sceneIndex = PlayerParty.Instance.sceneIndex;

        gameData.PartyMinions = new MinionClass.MinionFileSave[PlayerParty.Instance.minions.Count];
        for (int i = 0; i < PlayerParty.Instance.minions.Count; i++)
        {
            gameData.PartyMinions[i] = new MinionClass.MinionFileSave();

            gameData.PartyMinions[i].minionSave = new MinionClass.GenericMinionSave(PlayerParty.Instance.minions[i]);
            if (PlayerParty.Instance.minions[i].minion != null)
                gameData.PartyMinions[i].minionPrefabPath = "Battles/" + PlayerParty.Instance.minions[i].minion.name;
            if (PlayerParty.Instance.minions[i].animator != null)
                gameData.PartyMinions[i].animatorAssetPath = "Anime/" + PlayerParty.Instance.minions[i].animator.name;
            if (PlayerParty.Instance.minions[i].icon != null)
                gameData.PartyMinions[i].iconAssetPath = "Icons/" + PlayerParty.Instance.minions[i].icon.name;
        }
    }
}

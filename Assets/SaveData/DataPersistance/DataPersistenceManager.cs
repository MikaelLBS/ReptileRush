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

    FileDataHandler DataHandler;
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
        foreach (IDataPersitiens dataPer in dataPersistenceObjcets)
            dataPer.SaveData(ref gameData);

        DataHandler.Save(gameData);
    }
    private void OnApplicationQuit()
    {
        SaveGame();
    }
    private List<IDataPersitiens> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersitiens> dataPersistenceObjcets = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersitiens>();

        return new List<IDataPersitiens>(dataPersistenceObjcets);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using Newtonsoft.Json;

public class FileDataHandler
{
    string dataDirPath = string.Empty;
    string dataFileName = string.Empty;

    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }

    public GameData Load()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath,FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                //loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
                loadedData = JsonConvert.DeserializeObject<GameData>(dataToLoad);
            }
            catch(Exception e)
            { Debug.LogError("Error Loding " + fullPath + "\n" + e); }
        }
        return loadedData;

    }
    public void Save(GameData data)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        //Debug.Log(fullPath);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            //string dataToStore = JsonUtility.ToJson(data,true);
            //JsonConvert.SerializeObject(data, Formatting.Indented);
            string dataToStore = JsonConvert.SerializeObject(data);

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }

        } catch (Exception e)
        {
            Debug.LogError("Error Saving "+fullPath+"\n"+e);
        }
    }
}

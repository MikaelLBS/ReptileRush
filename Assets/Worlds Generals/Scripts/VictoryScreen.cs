using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryScreen : MonoBehaviour, IDataPersitiens
{
    // Start is called before the first frame update
    [SerializeField] float delay;
    [SerializeField] AudioClip VictoryMusic;
    float[] volumes;
    private void Start()
    {
        GameObject.Find("Music").GetComponent<AudioSource>().clip = VictoryMusic;
    }
    void Update()
    {
        if (Input.anyKeyDown)
        {
            delay = 0;
        }

        if (delay <= 0)
        {

            Time.timeScale = 1;
            new FileDataHandler(Application.persistentDataPath, "ReptileRushSave").Save(new(volumes));
            PlayerParty.Instance.minions.Clear();
            SceneManager.LoadScene(0);
        }
        else
            delay -= Time.deltaTime;
    }
    public void LoadData(GameData data)
    {
        volumes = new float[data.soundsVolume.Length];
        for (int i = 0; i < data.soundsVolume.Length; i++)
        {
            volumes[i] = data.soundsVolume[i];
        }
    }
    public void SaveData(ref GameData data)
    {

    }
}

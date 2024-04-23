using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PauseManager : MonoBehaviour, IDataPersitiens
{

    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider[] volumeSliders;
    [SerializeField] GameObject doubleCheckResetRunWindo;
    public Canvas canvas;
    public Canvas canvas2;
    public Canvas canvas3;
    private bool funFactor = false;
    private bool light = false;
    private bool isPaused = false;
    [SerializeField] bool fynny_WARNING;
    void Start () {
        canvas.enabled = isPaused;
        canvas2.enabled = isPaused;
    }
    void Update() {
        if (Input.GetButtonDown("Pause")) {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
        if (isPaused && funFactor && fynny_WARNING)
        {
            canvas2.enabled = light;
            canvas3.enabled = !light;
            if (light)            
                light = false;            
            else 
                light = true;
        }
    }

    void PauseGame() {
        Time.timeScale = 0;
        isPaused = true;
        canvas.enabled = true;
        if (Random.Range(0, 20) == 1) 
            funFactor = true;
    }
    public void ResumeGame() {
        Time.timeScale = 1;
        isPaused = false;
        canvas.enabled = false;
        canvas2.enabled = false;
        canvas3.enabled = false;
        funFactor = false;
    }
    public void MainMenu()
    {
        DataPersistenceManager.Instance.SaveGame();
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
    public void ResetRun()
    {
        float[] volumes = new float[volumeSliders.Length];
        for (int i = 0; i < volumeSliders.Length; i++)
            volumes[i] = volumeSliders[i].value;

        Time.timeScale = 1;
        new FileDataHandler(Application.persistentDataPath, "ReptileRushSave").Save(new(volumes));
        PlayerParty.Instance.minions.Clear();
        SceneManager.LoadScene(1);
    }
    public void SaveGameData()
    {
        DataPersistenceManager.Instance.SaveGame();
    }

    void ChangeVolume(string mixerName, float volume)
    { audioMixer.SetFloat(mixerName, MathF.Log10(volume) * 25); }
    public void ChangeMainVolume(float value)
    { ChangeVolume("Volume", value); }

    public void LoadData(GameData data)
    {
        for (int i = 0; i < data.soundsVolume.Length; i++)
        {
            volumeSliders[i].value = data.soundsVolume[i];
        }
    }
    public void SaveData(ref GameData data)
    {
        data.soundsVolume = new float[volumeSliders.Length];
        for (int i = 0; i < volumeSliders.Length; i++)
        {
            data.soundsVolume[i] = volumeSliders[i].value;
        }
    }
}

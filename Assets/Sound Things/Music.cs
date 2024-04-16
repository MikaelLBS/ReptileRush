using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Music : MonoBehaviour, IDataPersitiens
{
    [SerializeField] AudioClip[] music;
    [SerializeField] AudioClip[] musicLow;
    AudioClip[] currentMusic;
    [SerializeField] AudioSource audioSource;

    [SerializeField] Slider[] soundSliders;
    float timer;
    public static Music Instance { get; private set; }
    private void Start()
    {
        if (Instance == null)
            Instance = this;

        currentMusic = music;
    }
    public void ChangeMusicType(bool toLow)
    {
        if (!toLow)
            currentMusic = music;
        else
            currentMusic = musicLow;
        timer = SoundFunctions.PlaySound(audioSource, currentMusic);
    }
    void Update()
    {
        if (timer <= 0)
        {
            timer = SoundFunctions.PlaySound(audioSource,currentMusic);
        }
        else
            timer -= Time.deltaTime;
    }
    public void SaveData(ref GameData data)
    {
        data.soundsVolume = new float[soundSliders.Length];
        for (int i = 0; i < soundSliders.Length; i++)
            data.soundsVolume[i] = soundSliders[i].value;
    }
    public void LoadData(GameData data)
    {
        if (data.soundsVolume == null)
            return;
        for (int i = 0; i < soundSliders.Length; i++)
            soundSliders[i].value = data.soundsVolume[i];
    }
}

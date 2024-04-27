using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettinsMenu : MonoBehaviour, IDataPersitiens
{
    public AudioMixer audioMixer;
    [SerializeField] Slider[] volumeSliders;
    [SerializeField] Slider levels;
    [SerializeField] TMPro.TextMeshProUGUI levelText;
    [SerializeField] TMPro.TextMeshProUGUI hiscoreText;
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", MathF.Log10(volume) * 25);
    }
    public void SetLevels(float value)
    {
        if ((int)value == levels.maxValue)
        {
            levelText.text = "Levels: inf";
            hiscoreText.gameObject.SetActive(true);
        }
        else
        {
            levelText.text = "Levels: " + value;
            hiscoreText.gameObject.SetActive(false);
        }
    }
    public void LoadData(GameData data)
    {
        for (int i = 0; i < data.soundsVolume.Length; i++)
            volumeSliders[i].value = data.soundsVolume[i];
        if (data.amountOfLevels == -1)
            levels.value = levels.maxValue;
        else
            levels.value = data.amountOfLevels;

        if (levels.value == levels.maxValue)
            hiscoreText.gameObject.SetActive(true);
        hiscoreText.text = "Hiscore: "+data.hiscore;
    }
    public void SaveData(ref GameData data)
    {

        data.soundsVolume = new float[volumeSliders.Length];
        for (int i = 0; i < volumeSliders.Length; i++)
        {
            data.soundsVolume[i] = volumeSliders[i].value;
        }

        if (data.playerPosWasSaved)
            return;

        if (levels.value == levels.maxValue)
            data.amountOfLevels = -1;
        else
            data.amountOfLevels = (int)levels.value-1;
    }
}

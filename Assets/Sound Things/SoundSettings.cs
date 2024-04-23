using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour, IDataPersitiens
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider[] volumeSliders;

    void ChangeVolume(string mixerName,float volume)
    { audioMixer.SetFloat(mixerName, MathF.Log10(volume)*25); }
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

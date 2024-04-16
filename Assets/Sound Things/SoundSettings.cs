using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    Music musicSettings;

    void ChangeVolume(string mixerName,float volume)
    { audioMixer.SetFloat(mixerName, MathF.Log10(volume)*25); }
    public void ChangeEnemyVolume(float value)
    { ChangeVolume("Enemys",value); }
    public void ChangePlayerVolume(float value)
    { ChangeVolume("Player", value); }
    public void ChangeitemVolume(float value)
    { ChangeVolume("Items", value); }
    public void ChangeMusicVolume(float value)
    { ChangeVolume("Music", value); }
    public void ChangeMainVolume(float value)
    { ChangeVolume("volume", value); }

}

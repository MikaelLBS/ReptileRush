using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticSoundFunctions : MonoBehaviour // Can be called from all scripts like this "StaticSoundFunctions.PlaySound(PARAMS)"
{
    ///<summary>Picks One Random clip from inputed clips and plays it.</summary>
    ///<returns>Length of clip playing</returns>
    public static float PlaySound(AudioSource audioSource, AudioClip[] audioClips)
    {
        if (audioClips.Length <= 0)
            return 0;

        audioSource.clip = audioClips[Random.Range(0, audioClips.Length)];
        audioSource.Play();
        return audioSource.clip.samples / audioSource.clip.frequency;
    }
    ///<returns>Length of clip playing</returns>
    public static float PlaySound(AudioSource audioSource, AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        audioSource.Play();
        return audioSource.clip.samples / audioSource.clip.frequency;
    }
    public static float PlaySoundDontOverrite(AudioSource audioSource, AudioClip[] audioClips)
    {
        if (audioSource.isPlaying)
            return 0;
        return PlaySound(audioSource,audioClips);
    }
    public static float PlaySoundDontOverrite(AudioSource audioSource, AudioClip audioClip)
    {
        if (audioSource.isPlaying)
            return 0;
        return PlaySound(audioSource, audioClip);
    }
    // Player from Camera. NEED TO HAVE ADDED COMPONENT AUDIO SOURCE ON MAIN CAMERA!!!
    public static float PlaySound(AudioClip[] audioClips)
    {
        return PlaySound(Camera.main.GetComponent<AudioSource>(),audioClips);
    }
    public static float PlaySound(AudioClip audioClip)
    {
        return PlaySound(Camera.main.GetComponent<AudioSource>(), audioClip);
    }
}

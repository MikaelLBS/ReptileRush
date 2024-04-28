using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFunctions : MonoBehaviour
{
    public static float PlaySound(AudioSource audioSource, AudioClip[] audioClips)
    {
        if (audioClips.Length <= 0)
            return 0;

        audioSource.clip = audioClips[Random.Range(0, audioClips.Length)];
        audioSource.Play();
        return audioSource.clip.samples / audioSource.clip.frequency;
    }
    public static float PlaySoundDontOverrite(AudioSource audioSource, AudioClip[] audioClips)
    {
        if (audioSource.isPlaying)
            return 0;

        return PlaySound(audioSource,audioClips);
    }
    // Player from Camera
    public static float PlaySound(AudioClip[] audioClips)
    {
        return PlaySound(Camera.main.GetComponent<AudioSource>(),audioClips);
    }
}

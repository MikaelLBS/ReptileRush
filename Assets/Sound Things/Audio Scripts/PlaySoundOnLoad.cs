using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnLoad : MonoBehaviour
{
    enum AudioPlayOptions
    {
        playAllAudioClipsInOrder,
        playAllAudioClipsInRandomOrder,
        PlayOneRandomAudioClip
    }
    [SerializeField] AudioPlayOptions audioPlayOptions;
    [SerializeField] float extraDelayBetweenAudioClips;
    [SerializeField] List<AudioClip> audioClips;
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("GameObject with script PlaySoundOnLoad Needs Audio Source component!!!");
            return;
        }

        if (audioClips.Count == 1)
        {
            audioSource.clip = audioClips[0];
            audioSource.Play();
        }
        else
        {
            switch (audioPlayOptions)
            {
                case AudioPlayOptions.playAllAudioClipsInRandomOrder:
                    int[] indexs = new int[audioClips.Count];
                    List<AudioClip> tempClips = new List<AudioClip>();
                    tempClips.AddRange(audioClips);
                    for (int i = 0; i < indexs.Length;i++)
                    {
                        indexs[i] = Random.Range(0, tempClips.Count);
                        tempClips.RemoveAt(indexs[i]);
                    }
                    StartCoroutine(PlaySounds(indexs));
                    break;
                case AudioPlayOptions.PlayOneRandomAudioClip:
                    audioSource.clip = audioClips[Random.Range(0, audioClips.Count)];
                    audioSource.Play();
                    break;
                case AudioPlayOptions.playAllAudioClipsInOrder:
                    indexs = new int[audioClips.Count];
                    for (int i = 0; i < audioClips.Count;i++)
                    {
                        indexs[i] = i;
                    }
                    StartCoroutine(PlaySounds(indexs));
                    break;
            }
        }
    }
    IEnumerator PlaySounds(int[] indexs)
    {
        for (int i = 0; i < indexs.Length;i++)
        {
            audioSource.clip = audioClips[indexs[i]];
            audioSource.Play();
            yield return new WaitForSeconds(audioSource.clip.samples / audioSource.clip.frequency+extraDelayBetweenAudioClips);
        }
    }
}

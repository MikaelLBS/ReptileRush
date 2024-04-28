using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopSounds : MonoBehaviour
{
    enum AudioPlayOptions
    {
        playAudioClipsInOrder,
        playAudioClipsInRandomOrder,
        playAudioClipsInRandomOrderDontRepeatUntilAllClipsIsPlayed
    }
    [SerializeField] AudioPlayOptions audioPlayOptions;
    [SerializeField] float extraDelayBetweenAudioClips;
    [SerializeField] List<AudioClip> audioClips;
    AudioSource audioSource;
    bool isPlaying;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("GameObject with script PlaySoundOnLoad Needs Audio Source component!!!");
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlaying)
            return;
        isPlaying = true;

        switch (audioPlayOptions)
        {
            case AudioPlayOptions.playAudioClipsInRandomOrderDontRepeatUntilAllClipsIsPlayed:
                int[] indexs = new int[audioClips.Count];
                List<AudioClip> tempClips = new List<AudioClip>();
                tempClips.AddRange(audioClips);
                for (int i = 0; i < indexs.Length; i++)
                {
                    indexs[i] = Random.Range(0, tempClips.Count);
                    tempClips.RemoveAt(indexs[i]);
                }
                StartCoroutine(PlaySounds(indexs));
                break;
            case AudioPlayOptions.playAudioClipsInRandomOrder:
                StartCoroutine(PlaySounds());
                break;
            case AudioPlayOptions.playAudioClipsInOrder:
                indexs = new int[audioClips.Count];
                for (int i = 0; i < audioClips.Count; i++)
                {
                    indexs[i] = i;
                }
                StartCoroutine(PlaySounds(indexs));
                break;
        }
    }
    IEnumerator PlaySounds(int[] indexs)
    {
        for (int i = 0; i < indexs.Length; i++)
        {
            audioSource.clip = audioClips[indexs[i]];
            audioSource.Play();
            yield return new WaitForSeconds(audioSource.clip.samples / audioSource.clip.frequency + extraDelayBetweenAudioClips);
        }
        isPlaying = false;
    }
    IEnumerator PlaySounds()
    {

        audioSource.clip = audioClips[0];
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.samples / audioSource.clip.frequency + extraDelayBetweenAudioClips);
        isPlaying = false;
    }
}

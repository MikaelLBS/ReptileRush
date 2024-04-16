using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExamplePlaySounds : MonoBehaviour
{
    [SerializeField] AudioClip[] auClips;
    // Start is called before the first frame update
    void Start()
    {
        SoundFunctions.PlaySound(auClips);
    }
}

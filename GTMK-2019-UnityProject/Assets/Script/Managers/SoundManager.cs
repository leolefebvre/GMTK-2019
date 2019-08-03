using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    public List<AudioClip> selectionSounds;

    public AudioSource selectionAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySelectionSound(int soundIndex)
    {
        soundIndex = soundIndex % (selectionSounds.Count);

        selectionAudioSource.PlayOneShot(selectionSounds[soundIndex]);
    }
}

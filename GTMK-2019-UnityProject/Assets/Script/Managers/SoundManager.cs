using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    public List<AudioClip> selectionSounds;

    public AudioSource resetAudioSource;
    public AudioSource selectionAudioSource;
    public int repeatLastNumber = 2;

    [Header("Queue parameters")]

    public double timeBetweenQueueSounds = 0.05f;
    public int queueLimit = 10;
    public List<AudioClip> soundQueue;

    public bool isSourceOpen = true;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlaySelectionSound(int soundIndex, bool playInQueue = false)
    {
        //soundIndex = soundIndex % (selectionSounds.Count);
        

        if(soundIndex >= selectionSounds.Count)
        {
            soundIndex = selectionSounds.Count - (repeatLastNumber - (soundIndex - selectionSounds.Count) % repeatLastNumber) - 2;
        }

        if(playInQueue)
        {
            PlaySoundInQueue(selectionSounds[soundIndex]);
        }
        else
        {
            selectionAudioSource.PlayOneShot(selectionSounds[soundIndex]);
        }
    }

    public void PlaySoundInQueue(AudioClip sound)
    {
        if(soundQueue.Count >= queueLimit)
        {
            return;
        }

        if(isSourceOpen)
        {
            selectionAudioSource.PlayOneShot(sound);
            isSourceOpen = false;

            StartCoroutine(WaitForNextQueueOpening());
        }
        else
        {
            soundQueue.Add(sound);
        }
    }

    IEnumerator WaitForNextQueueOpening()
    {
        yield return new WaitForSeconds((float) timeBetweenQueueSounds);
        PlayNextSoundInQueueOrReleaseIt();
    }

    public void PlayNextSoundInQueueOrReleaseIt()
    {
        if(soundQueue.Count > 0)
        {
            // play next sound in queue
            selectionAudioSource.PlayOneShot(soundQueue[0]);

            // remove the sound from the queue
            soundQueue.RemoveAt(0);

            StartCoroutine(WaitForNextQueueOpening());
        }
        else
        {
            isSourceOpen = true;
        }
    }
}

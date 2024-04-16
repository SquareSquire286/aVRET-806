using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioButtonEvent : AbstractButtonEvent
{
    public AudioSource audioSource;
    public bool hasToggleableAudio;
    public AudioClip[] soundBites;

    // Start is called before the first frame update
    public override void Start()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    public override void ExecuteEvent()
    {
        if (hasToggleableAudio)
            this.SwitchClip_ThenPlay();

        else audioSource.Play();
    }

    public override void StopEvent()
    {
        audioSource.Stop();
    }

    private void SwitchClip_ThenPlay()
    {
        // Prevent NullReferenceExceptions by stopping playback before replacing the AudioClip
        if (audioSource.isPlaying)
            audioSource.Stop();

        audioSource.clip = soundBites[UnityEngine.Random.Range(0, soundBites.Length)];
        audioSource.Play(); // Replace the clip to be played by the audio source with a new one in the array,
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] audioClips, musicClips;
    public static AudioManager instance;
    private AudioSource audioSource;
    private AudioClip selectedClip;

    public float soundVolume = 1.0f;
    public float musicVolume = 1.0f;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        DontDestroyOnLoad(this);
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void PlaySound(string soundName)
    {
        selectedClip = null;
        soundName = soundName.ToLower();
        foreach (AudioClip clip in audioClips)
        {
            if (clip.name.ToLower() == soundName)
            {
                selectedClip = clip;
            }
        }


        float currentVolume;
        if (selectedClip != null)
        {
            currentVolume = soundVolume;
            audioSource.PlayOneShot(selectedClip, currentVolume);
        }
        else
        {
            foreach (AudioClip clip in musicClips)
            {
                if (clip.name.ToLower() == soundName)
                {
                    selectedClip = clip;
                }
            }
            currentVolume = musicVolume;
            audioSource.PlayOneShot(selectedClip, currentVolume);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

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


    public void PlaySound(string soundName, float delay = 0)
    {
        StartCoroutine(soundFunction(soundName, delay));
    }
    private IEnumerator soundFunction(string name, float delay)
    {
        yield return new WaitForSeconds(delay);
        selectedClip = null;
        name = name.ToLower();
        foreach (AudioClip clip in audioClips)
        {
            if (clip.name.ToLower() == name)
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
                if (clip.name.ToLower() == name)
                {
                    selectedClip = clip;
                }
            }
            currentVolume = musicVolume;
            audioSource.PlayOneShot(selectedClip, currentVolume);
        }
    }
}

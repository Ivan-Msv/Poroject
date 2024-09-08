using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] audioClips;
    public static AudioManager instance;
    private AudioSource audioSource;

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
    }


    public void PlaySound()
    {
        audioSource.PlayOneShot(audioClips[0]);
    }
}

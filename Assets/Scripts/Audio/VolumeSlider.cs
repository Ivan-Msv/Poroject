using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    private Slider slider;
    private TextMeshProUGUI tmp;
    [SerializeField] private bool sound = false;
    private void Awake()
    {
        slider = GetComponentInChildren<Slider>();
        tmp = GetComponentInChildren<TextMeshProUGUI>();
    }
    private void OnEnable()
    {
        slider.value = sound ? AudioManager.instance.soundVolume : AudioManager.instance.musicVolume;
    }


    public void ChangeVolume()
    {
        switch (sound)
        {
            case true:
                AudioManager.instance.soundVolume = slider.value;   
                tmp.text = $"Sound Volume ({Math.Round(slider.value, 2)})";
                break;
            case false:
                AudioManager.instance.musicVolume = slider.value;
                tmp.text = $"Music Volume ({Math.Round(slider.value, 2)})";
                break;
        }
    }
}

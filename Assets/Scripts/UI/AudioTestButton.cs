using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTestButton : MonoBehaviour
{
    public void PlayTestSound(string soundName)
    {
        AudioManager.instance.PlaySound(soundName);
    }
}

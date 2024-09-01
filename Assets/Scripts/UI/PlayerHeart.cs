using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum HeartState
{
    Empty = 0,
    Full = 1
}

public class PlayerHeart : MonoBehaviour
{
    public Sprite emptyHeart, fullHeart;
    private Image heartImage;

    void Awake()
    {
        heartImage = GetComponent<Image>();
    }
    public void SetHeartImage(HeartState status)
    {
        switch (status)
        {
            case HeartState.Full:
                heartImage.sprite = fullHeart;
                break;
            case HeartState.Empty:
                heartImage.sprite = emptyHeart;
                break;
        }
    }
}

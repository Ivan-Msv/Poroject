using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    [SerializeField] private float fadeSpeed;
    public bool fadeOut;
    public bool fadeIn;
    private Component currentComponent;
    // Start is called before the first frame update
    void Start()
    {
        GetObjectComponent();
    }

    // Update is called once per frame
    void Update()
    {
        FadeOutSystem();
        FadeInSystem();
    }

    public void StartFadeOut()
    {
        fadeIn = false;
        fadeOut = true;
    }

    public void StartFadeIn()
    {
        fadeOut = false;
        fadeIn = true;
    }

    private void FadeOutSystem()
    {
        if (fadeOut)
        {
            switch (currentComponent)
            {
                case Image:
                    Color imageColor = this.gameObject.GetComponent<Image>().color;
                    imageColor.a -= fadeSpeed * Time.deltaTime;
                    this.gameObject.GetComponent<Image>().color = imageColor;

                    if (imageColor.a <= 0)
                    {
                        fadeOut = false;
                    }
                    break;
                case CanvasGroup:
                    currentComponent.GetComponent<CanvasGroup>().alpha -= fadeSpeed * Time.deltaTime;
                    if (currentComponent.GetComponent<CanvasGroup>().alpha <= 0)
                    {
                        fadeOut = false;
                    }
                    break;
                case SpriteRenderer:
                    Color spriteRendererColor = this.gameObject.GetComponent<SpriteRenderer>().color;
                    spriteRendererColor.a -= fadeSpeed * Time.deltaTime;
                    this.gameObject.GetComponent<SpriteRenderer>().color = spriteRendererColor;

                    if (spriteRendererColor.a <= 0)
                    {
                        fadeOut = false;
                    }
                    break;
            }
        }
    }
    private void FadeInSystem()
    {
        if (fadeIn)
        {
            switch (currentComponent)
            {
                case Image:
                    Color imageColor = this.gameObject.GetComponent<Image>().color;
                    imageColor.a += fadeSpeed * Time.deltaTime;
                    this.gameObject.GetComponent<Image>().color = imageColor;

                    if (imageColor.a >= 1)
                    {
                        fadeIn = false;
                    }
                    break;
                case CanvasGroup:
                    currentComponent.GetComponent<CanvasGroup>().alpha += fadeSpeed * Time.deltaTime;
                    if (currentComponent.GetComponent<CanvasGroup>().alpha >= 1)
                    {
                        fadeIn = false;
                    }
                    break;
                case SpriteRenderer:
                    Color spriteRendererColor = this.gameObject.GetComponent<SpriteRenderer>().color;
                    spriteRendererColor.a += fadeSpeed * Time.deltaTime;
                    this.gameObject.GetComponent<SpriteRenderer>().color = spriteRendererColor;

                    if (spriteRendererColor.a <= 0)
                    {
                        fadeIn = false;
                    }
                    break;
            }
        }
    }
    private void GetObjectComponent()
    {
        Type[] typeList = { typeof(CanvasGroup), typeof(Image), typeof(SpriteRenderer)};

        foreach (Type type in typeList)
        {
            currentComponent = GetComponent(type);
            if (currentComponent != null)
            {
                break;
            }
        }
    }
}

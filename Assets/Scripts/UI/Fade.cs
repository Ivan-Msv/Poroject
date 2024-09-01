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
                    Color newColor = this.gameObject.GetComponent<Image>().color;
                    newColor.a -= fadeSpeed * Time.deltaTime;
                    this.gameObject.GetComponent<Image>().color = newColor;

                    if (newColor.a <= 0)
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
                    Color newColor = this.gameObject.GetComponent<Image>().color;
                    newColor.a += fadeSpeed * Time.deltaTime;
                    this.gameObject.GetComponent<Image>().color = newColor;

                    if (newColor.a >= 1)
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
            }
        }
    }
    private void GetObjectComponent()
    {
        currentComponent = this.GetComponent<Image>();
        if (this.GetComponent<Image>() == null)
        {
            currentComponent = this.GetComponent<CanvasGroup>();
        }
    }
}

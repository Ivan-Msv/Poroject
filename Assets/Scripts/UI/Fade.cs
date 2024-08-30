using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    [SerializeField] private float fadeSpeed;
    private bool fadeOut;
    // Start is called before the first frame update
    void Start()
    {
        fadeOut = true;
    }

    // Update is called once per frame
    void Update()
    {
        StartFadeOut();
    }

    private void StartFadeOut()
    {
        if (fadeOut)
        {
            Color newColor = this.gameObject.GetComponent<Image>().color;
            newColor.a -= fadeSpeed * Time.deltaTime;
            this.gameObject.GetComponent<Image>().color = newColor;
            
            if (newColor.a <= 0)
            {
                fadeOut = false;
            }
        }
    }
}

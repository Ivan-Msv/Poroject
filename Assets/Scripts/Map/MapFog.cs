using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapFog : MonoBehaviour
{
    public bool breakableObjectTrigger;
    private bool isFading;
    public GameObject breakableObject;
    [SerializeField] private float fadeoutSpeed;
    // Update is called once per frame
    void Update()
    {
        if (breakableObjectTrigger && !breakableObject.activeSelf)
        {
            isFading = true;
        }

        if (isFading)
        {
            FadeOut();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!breakableObjectTrigger && collision.CompareTag("Player"))
        {
            isFading = true;
        }
    }

    private void FadeOut()
    {
        Color newColor = this.GetComponent<SpriteRenderer>().color;
        if (newColor.a <= 0.1f)
        {
            gameObject.SetActive(false);
        }
        else
        {
            newColor.a = Mathf.Lerp(newColor.a, 0, fadeoutSpeed * Time.deltaTime);
            this.GetComponent<SpriteRenderer>().color = newColor;
        }

        Debug.Log(newColor.a);
    }
}

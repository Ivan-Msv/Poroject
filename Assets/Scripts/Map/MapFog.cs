using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

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
        Color newColor = this.GetComponent<SpriteShapeRenderer>().color;
        if (newColor.a <= 0.1f)
        {
            gameObject.SetActive(false);
        }
        else
        {
            newColor.a -= fadeoutSpeed * Time.deltaTime;
            this.GetComponent<SpriteShapeRenderer>().color = newColor;
        }
    }
}

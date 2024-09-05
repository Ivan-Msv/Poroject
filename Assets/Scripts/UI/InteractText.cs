using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InteractText : MonoBehaviour
{
    private float playerDistance;
    private Fade textFade;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        textFade = GetComponent<Fade>();
        player = FindAnyObjectByType<PlayerHealth>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        playerDistance = Vector3.Distance(this.transform.position, player.transform.position);
        if (playerDistance <= 8)
        {
            InteractTextAppearance();
        }
    }

    private void InteractTextAppearance()
    {
        if (playerDistance <= 3)
        {
            textFade.StartFadeIn();
        }
        else
        {
            textFade.StartFadeOut();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField] private ShortcutDoor door;
    [SerializeField] private Boss_Arena bossArea;
    private Animator anim;
    public bool canBePressed = false;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("canBePressed", canBePressed);

        if (canBePressed)
        {
            this.gameObject.GetComponent<BoxCollider2D>().enabled = true;
        }
        else
        {
            this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        door.shouldOpen = true;
        canBePressed = false;
    }
}

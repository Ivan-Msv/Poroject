using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDialogue : MonoBehaviour
{
    private DialogueData data;
    private bool canBeInteracted;
    // Start is called before the first frame update
    void Start()
    {
        data = GetComponent<DialogueData>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canBeInteracted && Input.GetKeyDown(KeyCode.E))
        {
            data.TriggerDialogue();
            this.gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canBeInteracted = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canBeInteracted = false;
        }
    }
}

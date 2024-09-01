using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    private DialogueData data;
    private bool canBeInteracted;
    private HealthUpgradeItem hpUpgrade;
    // Start is called before the first frame update
    void Start()
    {
        data = GetComponent<DialogueData>();
        hpUpgrade = GetComponent<HealthUpgradeItem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canBeInteracted && Input.GetKeyDown(KeyCode.E) && !data.dialogueActive)
        {
            data.TriggerDialogue();
            hpUpgrade.UpgradeHealth();
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

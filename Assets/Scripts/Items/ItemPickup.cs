using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] private ItemEffect itemFeature;
    private DialogueData data;
    private PlayerHealth player;
    private Fade objectFade;
    private bool canBeInteracted;
    // Start is called before the first frame update
    void Start()
    {
        data = GetComponent<DialogueData>();
        player = FindAnyObjectByType<PlayerHealth>();
        objectFade = this.gameObject.GetComponent<Fade>();
    }

    // Update is called once per frame
    void Update()
    {
        DialogueTrigger();
    }

    private void DialogueTrigger()
    {
        if (canBeInteracted && Input.GetKeyDown(KeyCode.E) && !DialogueManager.instance.dialogueActive)
        {
            data.TriggerDialogue();
            itemFeature.ApplyEffect(player.gameObject);
            StartCoroutine(RemoveObject());
        }
    }
    private IEnumerator RemoveObject()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        objectFade.StartFadeOut();
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
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

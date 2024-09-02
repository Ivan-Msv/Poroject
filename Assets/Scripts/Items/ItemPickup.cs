using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] private ItemEffect itemFeature;
    private DialogueData data;
    private PlayerHealth playerHP;
    private Fade objectFade;
    private Fade textFade;
    private bool canBeInteracted;
    private float playerDistance;
    // Start is called before the first frame update
    void Start()
    {
        data = GetComponent<DialogueData>();
        playerHP = FindAnyObjectByType<PlayerHealth>();
        objectFade = this.gameObject.GetComponent<Fade>();
        textFade = transform.GetChild(0).GetComponent<Fade>();
    }

    // Update is called once per frame
    void Update()
    {
        playerDistance = Vector3.Distance(this.transform.position, playerHP.transform.position);
        DialogueTrigger();
        if (playerDistance <= 8)
        {
            InteractTextAppearance();
        }
    }

    private void DialogueTrigger()
    {
        if (canBeInteracted && Input.GetKeyDown(KeyCode.E) && !data.dialogueActive)
        {
            data.TriggerDialogue();
            itemFeature.ApplyEffect(playerHP.gameObject);
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

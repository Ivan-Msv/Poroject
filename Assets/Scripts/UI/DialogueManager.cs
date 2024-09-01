using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textVisual;
    [SerializeField] private TextMeshProUGUI nameVisual;
    private CanvasGroup thisCanvas;
    private Fade fade;
    private Queue<string> sentences;

    private void Start()
    {
        thisCanvas = GetComponent<CanvasGroup>();
        fade = GetComponent<Fade>();
        sentences = new Queue<string>();
    }

    private void Update()
    {
        if (thisCanvas.alpha == 1)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                DisplayNextSentence();
            }
        }
    }

    public void StartDialogue(DialogueData data)
    {
        fade.fadeIn = true;
        sentences.Clear();
        foreach (string newSentence in data.sentences)
        {
            sentences.Enqueue(newSentence);
        }
        nameVisual.text = data.nickname;
        DisplayNextSentence();
    }
    private void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        else
        {
            string currentSentence = sentences.Dequeue();

            textVisual.text = currentSentence;
        }
    }
    private void EndDialogue()
    {
        fade.fadeOut = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    [SerializeField] private TextMeshProUGUI textVisual;
    [SerializeField] private TextMeshProUGUI nameVisual;
    private CanvasGroup thisCanvas;
    private Fade fade;
    private Queue<string> sentences;
    private DialogueData data;
    public bool dialogueActive;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        thisCanvas = GetComponent<CanvasGroup>();
        fade = GetComponent<Fade>();
        sentences = new Queue<string>();
    }

    private void Update()
    {
        dialogueActive = thisCanvas.alpha == 1;

        if (dialogueActive)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                DisplayNextSentence();
            }
        }
    }

    public void StartDialogue(DialogueData getData)
    {
        data = getData;

        fade.StartFadeIn();
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
        data.dialogueActive = false;
        fade.StartFadeOut();
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    [SerializeField] private TextMeshProUGUI textVisual, nameVisual, continueText, choice1Visual, choice2Visual;
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
        dialogueActive = thisCanvas.alpha > 0;

        DialogueKeys();
    }

    private void DialogueKeys()
    {
        if (dialogueActive)
        {
            if (data.hasChoice)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    data.selectedChoice = 1;
                    EndDialogue();
                }
                if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    data.selectedChoice = 2;
                    EndDialogue();
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    DisplayNextSentence();
                }
            }
        }
    }

    public void StartDialogue(DialogueData getData)
    {
        data = getData;
        nameVisual.text = data.nickname;

        fade.StartFadeIn();
        sentences.Clear();
        foreach (string newSentence in data.sentences)
        {
            sentences.Enqueue(newSentence);
        }
        DisplayNextSentence();
    }
    public void StartChoiceDialogue(DialogueData getData)
    {
        data = getData;
        nameVisual.text = data.nickname;

        fade.StartFadeIn();
        sentences.Clear();

        sentences.Enqueue(data.choiceSentence);
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

            if (data.hasChoice)
            {
                choice1Visual.text = $"[1] {data.choice1}";
                choice2Visual.text = $"{data.choice2} [2]";
                choice1Visual.gameObject.SetActive(true);
                choice2Visual.gameObject.SetActive(true);
            }
            else
            {
                continueText.gameObject.SetActive(true);
            }
        }
    }
    private void EndDialogue()
    {
        data.dialogueActive = false;
        fade.StartFadeOut();
        choice1Visual.gameObject.SetActive(false);
        choice2Visual.gameObject.SetActive(false);
        continueText.gameObject.SetActive(false);
    }
}

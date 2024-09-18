using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndingCutscenes : MonoBehaviour
{
    [SerializeField] private Fade endScreen;
    [TextArea()]
    [SerializeField] private string endText;
    private float minutes;
    private float seconds;
    private DialogueData data;
    void Start()
    {
        data = GetComponent<DialogueData>();
        data.ending = true;
    }
    private void Update()
    {
        if (data.ending == false)
        {
            SetTimer();
            // for some reason it didn't find it without specifically getting the child
            endScreen.transform.GetChild(2).GetComponent<Button>().gameObject.SetActive(true);
            endScreen.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = string.Format(endText, minutes, seconds, RespawnManager.instance.deathCount, RespawnManager.instance.healsTaken);
            endScreen.StartFadeIn();
            if (endScreen.gameObject.GetComponent<CanvasGroup>().alpha == 1)
            {
                Time.timeScale = 0;
            }
        }
    }
    public void FirstEnding()
    {
        data.TriggerDialogue();
    }
    private void SetTimer()
    {
        float timer = Time.time;
        minutes = Mathf.Floor(timer / 60);
        seconds = Mathf.Floor(timer - minutes * 60);
    }
}

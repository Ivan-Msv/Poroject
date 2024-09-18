using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutScene : MonoBehaviour
{
    [SerializeField] private Boss_Arena bossArea;
    [SerializeField] private Door entranceDoor;
    [SerializeField] private float sceneDuration;
    private SwitchCamera cameraSwitch;
    private bool firstCutSceneActivated = false;
    void Start()
    {
        cameraSwitch = GetComponent<SwitchCamera>();
    }

    private IEnumerator CutSceneStart()
    {
        this.gameObject.GetComponent<Collider2D>().enabled = false;
        this.gameObject.transform.position += Vector3.up * 3;
        entranceDoor.shouldOpen = false;
        entranceDoor.shouldClose = true;
        cameraSwitch.SetCamera();
        yield return new WaitForSeconds(sceneDuration);
        cameraSwitch.SetCamera();
        yield return new WaitForSeconds(sceneDuration);
        firstCutSceneActivated = true;
        bossArea.fightActive = true;
    }

    private void FightTrigger()
    {
        entranceDoor.shouldClose = true;
        entranceDoor.shouldOpen = false;
        bossArea.fightActive = true;
        this.gameObject.GetComponent<Collider2D>().enabled = false;
    }

    private void CutSceneSystem()
    {
        if (firstCutSceneActivated)
        {
            FightTrigger();
        }
        else
        {
            StartCoroutine(CutSceneStart());
        }
    }
    public void ResetCutScene()
    {
        this.gameObject.GetComponent<Collider2D>().enabled = true;
        entranceDoor.shouldClose = false;
        entranceDoor.shouldOpen = true;
        bossArea.fightActive = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CutSceneSystem();
        }
    }
}

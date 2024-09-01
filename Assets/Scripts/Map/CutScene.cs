using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutScene : MonoBehaviour
{
    [SerializeField] private Boss_Arena bossArea;
    [SerializeField] private Door entranceDoor;
    [SerializeField] private float sceneDuration;
    private SwitchCamera cameraSwitch;
    // Start is called before the first frame update
    void Start()
    {
        cameraSwitch = GetComponent<SwitchCamera>();
    }

    private IEnumerator CutSceneStart()
    {
        Destroy(this.gameObject.GetComponent<Collider2D>());
        entranceDoor.shouldClose = true;
        cameraSwitch.SetCamera();
        yield return new WaitForSeconds(sceneDuration);
        cameraSwitch.SetCamera();
        yield return new WaitForSeconds(sceneDuration);
        bossArea.fightActive = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(CutSceneStart());
        }
    }
}

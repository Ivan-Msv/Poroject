using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Arena : MonoBehaviour
{
    [SerializeField] private GameObject entranceDoor;
    [SerializeField] private float speed;
    public bool fightActive;
    public float doorSwitch;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Door();
    }

    public void Door()
    {
        switch (doorSwitch)
        {
            case 1:
                OpenDoor();
                break;
            case 2:
                CloseDoor();
                break;
        }
    }
    private void OpenDoor()
    {
        if (entranceDoor.transform.localScale.x > 0.1f)
        {
            entranceDoor.transform.localScale = new Vector3(Mathf.Lerp(entranceDoor.transform.localScale.x, 0, speed * Time.deltaTime), 1, 1);
        }
        else
        {
            doorSwitch = 0;
        }
    }

    private void CloseDoor()
    {
        if (entranceDoor.transform.localScale.x < 1)
        {
            entranceDoor.transform.localScale = new Vector3(Mathf.Lerp(entranceDoor.transform.localScale.x, 1.5f, speed * Time.deltaTime), 1, 1);
        }
        else
        {
            doorSwitch = 0;
        }
    }
}

using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCamera : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera playerCamera;
    [SerializeField] private CinemachineVirtualCamera targetCamera;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCamera()
    {
        switch (playerCamera.Priority)
        {
            case 0:
                playerCamera.Priority = 1;
                targetCamera.Priority = 0;
                break;
            case 1:
                playerCamera.Priority = 0;
                targetCamera.Priority = 1;
                break;
        }
    }
}

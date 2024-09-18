using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCamera : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera playerCamera;
    [SerializeField] private CinemachineVirtualCamera targetCamera;
    private CinemachineBrain cameraBrain;
    void Start()
    {
        // since only one exists anyway
        cameraBrain = FindAnyObjectByType<CinemachineBrain>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCamera(float time = 2)
    {
        cameraBrain.m_DefaultBlend.m_Time = time;
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

using Cinemachine;
using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;
    private CinemachineBasicMultiChannelPerlin shakeComponent;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        shakeComponent = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }
    public void ShakeCamera(float intensity, float time)
    {
        StartCoroutine(CameraShaking(intensity, time));
    }

    private IEnumerator CameraShaking(float intensity, float time)
    {
        shakeComponent.m_AmplitudeGain = intensity;
        yield return new WaitForSeconds(time);
        shakeComponent.m_AmplitudeGain = 0;
    }
}

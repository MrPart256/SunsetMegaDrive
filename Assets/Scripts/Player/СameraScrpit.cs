using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Cinemachine;
public class СameraScrpit : MonoBehaviour
{
    private CinemachineVirtualCamera _camera;
    private float shakeTimer;
    public static СameraScrpit Instance { get; private set; }
    void Awake()
    {
        Instance = this;
        _camera = GetComponent<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    
    public void shake(float intensity,float time)
    {
        CinemachineBasicMultiChannelPerlin _shake = _camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _shake.m_AmplitudeGain = intensity;
        shakeTimer = time;
    }
    void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
        }
        if (shakeTimer <= 0f)
        {
            CinemachineBasicMultiChannelPerlin _shake = _camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            _shake.m_AmplitudeGain = 0f;
        }
    }
}

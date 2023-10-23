using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Cinemachine.PostFX;
using Unity.Mathematics;

public class CameraShakeEffect : MonoBehaviour {

    public static CameraShakeEffect Instance { get; private set; }

    private CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin;

    private float timer;
    private float timerMax;
    private float startingIntensity;

    private void Awake() {
        Instance = this;

        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        cinemachineBasicMultiChannelPerlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Start() {
        ShakeCamera(0, 0);
    }

    private void Update() {
        if (timer < timerMax) {
            timer += Time.deltaTime;
            float amplitude = Mathf.Lerp(startingIntensity, 0f, timer / timerMax);
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = amplitude;
        }
    }

    public void ShakeCamera(float intensity, float timerMax) {
        this.timerMax = timerMax;
        startingIntensity = intensity;
        timer = 0f;
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
    }

}

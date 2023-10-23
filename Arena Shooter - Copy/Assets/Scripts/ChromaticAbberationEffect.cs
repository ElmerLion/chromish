using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Cinemachine.PostFX;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Rendering;

public class ChromaticAbberationEffect : MonoBehaviour {

    public static ChromaticAbberationEffect Instance {  get; private set; }

    [SerializeField] CinemachinePostProcessing cinemachinePostProcessing;
    private ChromaticAberration chromaticAberration;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        chromaticAberration = cinemachinePostProcessing.m_Profile.GetSetting<ChromaticAberration>();
        chromaticAberration.intensity.value = 0f; 
    }

    private void Update() {
        if (chromaticAberration.intensity.value > 0f) {
            float decreaseSpeed = 1f;
            chromaticAberration.intensity.value -= Time.deltaTime * decreaseSpeed;
        }
    }

    public void SetIntensity(float intensity) {
        chromaticAberration.intensity.value += intensity;
    }

}

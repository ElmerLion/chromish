using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioSource playerAudioSource;

    private AudioClip hitmarkerClip;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        hitmarkerClip = Resources.Load<AudioClip>("hitmarker");
    }

    public void PlayHitMarkerSound() {
        playerAudioSource.PlayOneShot(hitmarkerClip);
    }
}

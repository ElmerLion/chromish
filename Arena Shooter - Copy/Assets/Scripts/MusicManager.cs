using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {

    public static MusicManager Instance { get; private set; }

    [SerializeField] private AudioSource ambiencePlayer;
    [SerializeField] private List<AudioClip> ambienceClips;

    private void Awake() {
        Instance = this;
        PlayRandomAmbience();
    }

    private void Update() {
        if (!ambiencePlayer.isPlaying) {
            PlayRandomAmbience();
        }
    }

    public void PlayRandomAmbience() {
        int randomInt = Random.Range(0, ambienceClips.Count);

        ambiencePlayer.clip = ambienceClips[randomInt];
        ambiencePlayer.Play();
        
    }

}

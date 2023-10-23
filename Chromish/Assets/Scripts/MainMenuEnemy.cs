using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuEnemy : MonoBehaviour {

    private bool running;

    private void Start() {
        running = false;
    }

    public void StartRunning() {
        running = true; 
        
    }

    private void Update() {
        if (running) {
            transform.LookAt(Camera.main.transform.position);
            transform.position = transform.position * 10 * Time.deltaTime;
        }
        
    }

}

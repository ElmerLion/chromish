using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour {

    [SerializeField] private Transform playButton;
    [SerializeField] private Transform quitButton;

    private void Awake() {
        playButton.GetComponent<Button>().onClick.AddListener(() => {
            
            StartCoroutine(StartGame());
            
        });

        quitButton.GetComponent<Button>().onClick.AddListener(() => {
            Application.Quit();
        });

    }

    private void Start() {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.None;
    }

    private IEnumerator StartGame() {
        
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene("GameScene");
    }

}

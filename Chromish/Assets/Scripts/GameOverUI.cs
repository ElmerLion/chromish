using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour{

    public static GameOverUI Instance { get; private set; }

    [SerializeField] Transform retryButton;
    [SerializeField] Transform mainMenuButton;
    [SerializeField] Transform roundScore;
    [SerializeField] Transform highScore;

    private void Awake() {
        Instance = this;

        retryButton.GetComponent<Button>().onClick.AddListener(() => {
            SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
        });

        mainMenuButton.GetComponent<Button>().onClick.AddListener(() => {
            SceneManager.LoadScene("MainMenuScene", LoadSceneMode.Single);
        });

        Hide();
    }



    public void Show() {
        gameObject.SetActive(true);

        roundScore.GetComponent<TextMeshProUGUI>().SetText("Score: " + ScoreManager.Instance.GetCurrentScore());
        highScore.GetComponent<TextMeshProUGUI>().SetText("Highscore: " + ScoreManager.Instance.GetCurrentHighScore());
    }

    public void Hide() {
        gameObject.SetActive(false);
    }

    // Calculate hours and minutes to minutes
    

}

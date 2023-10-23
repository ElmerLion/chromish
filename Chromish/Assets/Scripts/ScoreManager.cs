using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour {

    public static ScoreManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI scoreAmount;
    [SerializeField] private AudioSource scoreGainSource;

    private int currentScore;
    private int highScore;

    private void Awake() {
        Instance = this;
        
    }

    private void Start() {
        currentScore = 0;
        UpdateScoreUI();
        Debug.Log("Highscore: " + PlayerPrefs.GetInt("playerHighScore"));
    }

    public void AddScore(int amount) {
        currentScore += amount;
        UpdateScoreUI();
        SaveNewScore();
        scoreGainSource.PlayOneShot(scoreGainSource.clip);
        ScoreGainDisplay.Instance.ShowScore(amount);
    }

    private void UpdateScoreUI() {
        scoreAmount.text = currentScore.ToString();
    }

    private void SaveNewScore() {
        if (currentScore > PlayerPrefs.GetInt("playerHighScore", 0)) {
            PlayerPrefs.SetInt("playerHighScore", currentScore);
            highScore = GetCurrentHighScore();
            Debug.Log(highScore);
        }
        
    }

    public int GetCurrentScore() {
        return currentScore;
    }

    public int GetCurrentHighScore() {
        return PlayerPrefs.GetInt("playerHighScore");
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreGainDisplay : MonoBehaviour {

    public static ScoreGainDisplay Instance {  get; private set; }

    private TextMeshProUGUI scoreText;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        scoreText = transform.GetComponent<TextMeshProUGUI>();
        scoreText.enabled = false; 
    }

    public void ShowScore(int score) {
        StartCoroutine(AnimateScore(score));
    }

    private IEnumerator AnimateScore(int score) {
        scoreText.text = "+" + score;
        scoreText.enabled = true;

        float fadeDuration = 1.0f;
        float fadeDelay = 0.5f; 

        // Fade in
        float timer = 0f;
        while (timer < fadeDuration) {
            float alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            scoreText.color = new Color(scoreText.color.r, scoreText.color.g, scoreText.color.b, alpha);
            timer += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(fadeDelay);

        // Fade out
        timer = 0f;
        while (timer < fadeDuration) {
            float alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            scoreText.color = new Color(scoreText.color.r, scoreText.color.g, scoreText.color.b, alpha);
            timer += Time.deltaTime;
            yield return null;
        }

        scoreText.enabled = false; 
    }

}

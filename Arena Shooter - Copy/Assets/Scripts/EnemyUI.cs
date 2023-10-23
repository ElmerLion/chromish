using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyUI : MonoBehaviour {

    public static EnemyUI Instance { get; private set; }

    [SerializeField] TextMeshProUGUI enemyAmount;

    private int enemiesAlive;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        enemiesAlive = 0;
        UpdateEnemiesAliveText();
    }

    public void AddEnemy(int enemyAmount) {
        enemiesAlive += enemyAmount;
        UpdateEnemiesAliveText();
    }

    private void UpdateEnemiesAliveText() {
        enemyAmount.text = enemiesAlive.ToString();
    }

}

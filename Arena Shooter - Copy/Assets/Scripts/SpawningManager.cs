using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningManager : MonoBehaviour {

    public static SpawningManager Instance;

    private enum State {
        WaitingToSpawnNextWave,
        SpawningWave,
    }


    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private LayerMask[] obstacleMasks;

    private State state;

    private int enemiesToSpawn;
    private float nextWaveTimer;
    private float nextEnemySpawnTimer;
    private float nextWaveTimerMax = 25;

    private int waveNumber;
    private int startingWaveEnemies = 1;
    private int enemiesAddedPerWave = 2;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        nextWaveTimerMax = 25;
        waveNumber = 0;
        nextWaveTimer = 2;
        state = State.WaitingToSpawnNextWave;
    }

    private void Update() {
        switch (state) {
            case State.WaitingToSpawnNextWave:
                nextWaveTimer -= Time.deltaTime;
                if (nextWaveTimer < 0f) {
                    SpawnWave();
                }

                break;
            case State.SpawningWave:
                if (enemiesToSpawn > 0) {
                    nextEnemySpawnTimer -= Time.deltaTime;
                    if (nextEnemySpawnTimer < 0f) {
                        nextEnemySpawnTimer = Random.Range(.5f, 2f);
                        SpawnEnemy();
                        enemiesToSpawn--;

                        if (enemiesToSpawn <= 0) {
                            state = State.WaitingToSpawnNextWave;
                        }
                    }
                }
                break;
        }

        
    }

    private void SpawnEnemy() {
        Debug.Log("Spawning Enemy");
        int randomInt = Random.Range(0, spawnPoints.Length);
        Vector3 spawnPosition = spawnPoints[randomInt].position;

        Enemy.Create(spawnPosition);
    }

    private void SpawnWave() {
        nextWaveTimer = nextWaveTimerMax;
        enemiesToSpawn = startingWaveEnemies + enemiesAddedPerWave * waveNumber;
        waveNumber++;
        state = State.SpawningWave;
    }

    public LayerMask[] GetObstacleMasks() {
        return obstacleMasks;
    }
}

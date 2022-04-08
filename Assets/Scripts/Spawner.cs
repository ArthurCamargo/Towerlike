using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spawner : MonoBehaviour {
    public Wave[] waves;
    public List<Enemy> enemiesTypes;
    Wave currentWave;
    int currentWaveNumber;

    public WaveCounterUI waveCounterUI;

    int enemiesRemainingToSpawn;
    int enemiesRemainingAlive;
    float nextSpawnTime;


    void Start() {
        waveCounterUI = GameObject.Find("WaveCounter").GetComponent<WaveCounterUI>();
        NextWave();
    }

    void Update() {
        if(enemiesRemainingToSpawn > 0 && Time.time > nextSpawnTime) {
            enemiesRemainingToSpawn--;
            nextSpawnTime = Time.time + currentWave.timeBetweenSpawns;
            int enemyIdx = Random.Range(0, enemiesTypes.Count - 1);
            Enemy spawnedEnemy = Instantiate(enemiesTypes[enemyIdx], this.transform.position, Quaternion.identity) as Enemy;
            spawnedEnemy.OnDeath += OnEnemyDeath;
        }
    }

    void OnEnemyDeath() {
        enemiesRemainingAlive--;

        if(enemiesRemainingAlive == 0) {
            NextWave();
        }
    }

    void NextWave() {
        currentWaveNumber++;
        waveCounterUI.ChangeWaveNumber(currentWaveNumber);
        if(currentWaveNumber - 1 < waves.Length) {
            currentWave = waves[currentWaveNumber - 1];

            enemiesRemainingToSpawn = currentWave.enemyCount;
            enemiesRemainingAlive = enemiesRemainingToSpawn;

            if(currentWaveNumber > 1)
                Inventory.instance.AddRandom();
        }
        else {
            Debug.Log("Congratulations!!!!!!!!");
            SceneManager.LoadScene("Menu");
        }
    }

    [System.Serializable]
    public class Wave {
        public int enemyCount;
        public float timeBetweenSpawns;
    }
}

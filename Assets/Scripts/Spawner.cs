using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Wave[] waves;
    public Enemy enemy;

    Wave currentWave;
    int currentWaveNumber;

    int enemiesRemainingToSpawn;
    float nextSpawnTime;

    void Start() {
        NextWave();
    }

    void Update() {
        if(enemiesRemainingToSpawn > 0 && Time.time > nextSpawnTime) {
            enemiesRemainingToSpawn--;
            nextSpawnTime = Time.time + currentWave.timeBetweenSapawns;

            Enemy spawnedEnemy = Instantiate(enemy, GameObject.FindGameObjectWithTag("EnemySpawner").transform.position, Quaternion.identity) as Enemy;
        }
    }

    void NextWave() {
        currentWaveNumber++;
        currentWave = waves[currentWaveNumber - 1];

        enemiesRemainingToSpawn = currentWave.enemyCount;
    }

    [System.Serializable]
    public class Wave {
        public int enemyCount;
        public float timeBetweenSapawns;
    }
}

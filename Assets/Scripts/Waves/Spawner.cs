using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spawner : MonoBehaviour {
    public Enemy enemy;
    public Wave currentWave;
    int enemiesRemainingToSpawn;
    int enemiesRemainingAlive;
    float nextSpawnTime;
    public bool spawning = false;

    void Update() {
        if(enemiesRemainingToSpawn > 0 && Time.time > nextSpawnTime && spawning) {
            enemiesRemainingToSpawn--;
            nextSpawnTime = Time.time + currentWave.timeBetweenSpawns;
            Enemy spawnedEnemy = Enemy.GenerateEnemyFromWave(currentWave, this.transform.position);
            spawnedEnemy.OnDeath += OnEnemyDeath;
        }
    }

    void OnEnemyDeath() {
        enemiesRemainingAlive--;

        if(enemiesRemainingAlive == 0) {    
            spawning = false;
        }
    }

    public void CallWave(Wave wave) {
        spawning = true;
        currentWave = wave;
        enemiesRemainingToSpawn = currentWave.enemyCount;
        enemiesRemainingAlive = enemiesRemainingToSpawn;
    }
}

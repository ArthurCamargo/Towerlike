using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spawner : MonoBehaviour {
    public List<Wave> waves;
    public Enemy enemy;
    Wave currentWave;
    int currentWaveNumber;
    System.Random random = new System.Random();

    public WaveCounterUI waveCounterUI;

    int enemiesRemainingToSpawn;
    int enemiesRemainingAlive;
    float nextSpawnTime;
    System.Random rand = new System.Random();


    private void Awake() {
        for(int i = 0; i < 50; i++) {
            waves.Add(Wave.GenerateRandomWave(i, random));
        }
    }

    void Start() {
        waveCounterUI = GameObject.Find("WaveCounter").GetComponent<WaveCounterUI>();
        NextWave();
    }

    void Update() {
        if(enemiesRemainingToSpawn > 0 && Time.time > nextSpawnTime) {
            enemiesRemainingToSpawn--;
            nextSpawnTime = Time.time + currentWave.timeBetweenSpawns;
            Enemy spawnedEnemy = Instantiate(enemy, this.transform.position, Quaternion.identity) as Enemy;
            currentWave.GenerateEnemy(spawnedEnemy, random);
            spawnedEnemy.OnDeath += OnEnemyDeath;
        }
    }

    void OnEnemyDeath() {
        enemiesRemainingAlive--;

        if(enemiesRemainingAlive == 0) {
            NextWave();
            switch(currentWave.reward) {
                case Wave.Rewards.NONE:
                    break;
                case Wave.Rewards.ELEMENT_ITEM:
                    Inventory.instance.AddRandomElementItem();
                    break;
                case Wave.Rewards.EFFECT_ITEM:
                    Inventory.instance.AddRandomEffectItem();
                    break;
                case Wave.Rewards.STAT_ITEM:
                    Inventory.instance.AddRandomStatItem();
                    break;
                case Wave.Rewards.SOCKET_ITEM:
                    Inventory.instance.AddRandomSocketItem();
                    break;
                case Wave.Rewards.COMBAT_ITEM:
                    Inventory.instance.AddRandomCombatItem();
                    break;
                case Wave.Rewards.TOWER_ITEM:
                    Inventory.instance.AddRandomTowerItem();
                    break;
                case Wave.Rewards.BASE_HEAL:
                    GameObject playerBase = GameObject.FindGameObjectWithTag("Base");
                    playerBase.GetComponent<LivingEntity>().health += 10;
                    break;

            }
        }
    }

    void NextWave() {
        currentWaveNumber++;
        waveCounterUI.ChangeWaveNumber(currentWaveNumber);
        if(currentWaveNumber - 1 < waves.Count) {
            currentWave = waves[currentWaveNumber - 1];

            enemiesRemainingToSpawn = currentWave.enemyCount;
            enemiesRemainingAlive = enemiesRemainingToSpawn;
                
        }
        else {
            Debug.Log("Congratulations!!!!!!!!");
            SceneManager.LoadScene("Menu");
        }
    }
}

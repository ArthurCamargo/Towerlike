using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static Utility;

[System.Serializable]
public class Wave {
    public const float WAVE_DIFFICULTY_MULTIPLYER = 1.05f;
    public int waveNumber;
    public int enemyCount;
    public float timeBetweenSpawns;
    public List<EnemyType> enemyTypes;
    public WaveType waveType;
    public Rewards reward;
    public Difficulty timeBetweenSpawnsType, enemyCountType, enemyHealthType, enemySpeedType, enemyDamageType;

    public Wave(int waveNumber) {
        
        this.waveNumber = waveNumber;
        this.enemyTypes = new List<EnemyType>();
        this.reward = Rewards.NONE;
        this.waveType = new WaveType();
        this.timeBetweenSpawnsType = Difficulty.VERY_LOW;
        this.enemyCountType = Difficulty.VERY_LOW;
        this.enemyHealthType = Difficulty.VERY_LOW;
        this.enemySpeedType = Difficulty.VERY_LOW;
        this.enemyDamageType = Difficulty.VERY_LOW;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class Wave {
    public const float WAVE_DIFFICULTY_MULTIPLYER = 1.05f;
    public int enemyCount;
    public float timeBetweenSpawns;
    public float healthMin, healthMax;
    public float speedMin, speedMax;
    public float damageMin, damageMax;
    public List<Attributes.Elements> elements;
    public Rewards reward;
    public Difficulty enemyCountType, enemyHealthType, enemySpeedType, enemyDamageType;

    public enum Rewards {
        NONE,
        ELEMENT_ITEM,
        EFFECT_ITEM,
        STAT_ITEM,
        SOCKET_ITEM,
        COMBAT_ITEM,
        TOWER_ITEM,
        BASE_HEAL,
    }

    public enum Difficulty {
        VERY_LOW,
        LOW,
        MEDIUM,
        HIGH,
        VER_HIGH,
    }

    public Wave(Difficulty enemyCountType, Difficulty enemyHealthType, Difficulty enemySpeedType, Difficulty enemyDamageType) {
        this.enemyCountType = enemyCountType;
        this.enemyHealthType = enemyHealthType;
        this.enemySpeedType = enemySpeedType;
        this.enemyDamageType = enemyDamageType;
    }

    public Wave(int enemyCount, float timeBetweenSpawns) {
        this.enemyCount = enemyCount;
        this.timeBetweenSpawns = timeBetweenSpawns;
    }

    public Wave() {
        this.healthMin = 0;
        this.healthMax = 0;
        this.speedMin = 0;
        this.speedMax = 0;
        this.damageMin = 0;
        this.damageMax = 0;
        this.elements = new List<Attributes.Elements>();
        this.reward = Rewards.NONE;
        this.enemyCountType = Difficulty.VERY_LOW;
        this.enemyHealthType = Difficulty.VERY_LOW;
        this.enemySpeedType = Difficulty.VERY_LOW;
        this.enemyDamageType = Difficulty.VERY_LOW;
    }

    internal void GenerateEnemy(Enemy spawnedEnemy, System.Random random) {
        Color enemyColor = Color.gray;

        spawnedEnemy.startingHealth = random.Next((int)this.healthMin, (int)this.healthMax);
        spawnedEnemy.GetComponent<NavMeshAgent>().speed = random.Next((int)this.speedMin, (int)this.speedMax);
        spawnedEnemy.enemyDamage = random.Next((int)this.damageMin, (int)this.damageMax);
        spawnedEnemy.enemyElement = elements[random.Next(0, this.elements.Count - 1)];
        

        switch(spawnedEnemy.enemyElement) {
            case Attributes.Elements.NONE:
                enemyColor = Color.gray;
                break;
            case Attributes.Elements.FIRE:
                enemyColor = Color.red;
                break;
            case Attributes.Elements.WATER:
                enemyColor = Color.blue;
                break;
            case Attributes.Elements.PLANT:
                enemyColor = Color.green;
                break;
            case Attributes.Elements.LIGHT:
                enemyColor = Color.white;
                break;
            case Attributes.Elements.DARKNESS:
                enemyColor = Color.magenta;
                break;
        }

        spawnedEnemy.GetComponent<Renderer>().material.color = enemyColor;

    }

    public static Wave GenerateRandomWave(int waveNum, System.Random random) {

        WaveType[] waveTypes = Resources.LoadAll<WaveType>("Waves");
        int randomWaveTypeIdx = Random.Range(0, waveTypes.Length);
        Wave newWave = new Wave();

        switch(waveTypes[randomWaveTypeIdx].enemyCountType) {
            case Difficulty.VERY_LOW:
                newWave.enemyCount = 1;
                break;
            case Difficulty.LOW:
                newWave.enemyCount = 4;
                break;
            case Difficulty.MEDIUM:
                newWave.enemyCount = 10;
                break;
            case Difficulty.HIGH:
                newWave.enemyCount = 20;
                break;
            case Difficulty.VER_HIGH:
                newWave.enemyCount = 40;
                break;
        }

        switch(waveTypes[randomWaveTypeIdx].enemyHealthType) {
            case Difficulty.VERY_LOW:
                newWave.healthMin = 1;
                newWave.healthMax = 4;
                break;
            case Difficulty.LOW:
                newWave.healthMin = 4;
                newWave.healthMax = 10;
                break;
            case Difficulty.MEDIUM:
                newWave.healthMin = 10;
                newWave.healthMax = 20;
                break;
            case Difficulty.HIGH:
                newWave.healthMin = 20;
                newWave.healthMax = 40;
                break;
            case Difficulty.VER_HIGH:
                newWave.healthMin = 40;
                newWave.healthMax = 60;
                break;
        }

        switch(waveTypes[randomWaveTypeIdx].enemySpeedType) {
            case Difficulty.VERY_LOW:
                newWave.speedMin = 1;
                newWave.speedMax = 3;
                break;
            case Difficulty.LOW:
                newWave.speedMin = 3;
                newWave.speedMax = 5;
                break;
            case Difficulty.MEDIUM:
                newWave.speedMin = 8;
                newWave.speedMax = 12;
                break;
            case Difficulty.HIGH:
                newWave.speedMin = 15;
                newWave.speedMax = 20;
                break;
            case Difficulty.VER_HIGH:
                newWave.speedMin = 20;
                newWave.speedMax = 30;
                break;
        }

        switch(waveTypes[randomWaveTypeIdx].enemyDamageType) {
            case Difficulty.VERY_LOW:
                newWave.damageMin = 1;
                newWave.damageMax = 5;
                break;
            case Difficulty.LOW:
                newWave.damageMin = 5;
                newWave.damageMax = 10;
                break;
            case Difficulty.MEDIUM:
                newWave.damageMin = 10;
                newWave.damageMax = 15;
                break;
            case Difficulty.HIGH:
                newWave.damageMin = 15;
                newWave.damageMax = 20;
                break;
            case Difficulty.VER_HIGH:
                newWave.damageMin = 30;
                newWave.damageMax = 50;
                break;
        }

        newWave.Multiply(waveNum);

        newWave.elements.Add((Attributes.Elements)random.Next(0, System.Enum.GetNames(typeof(Attributes.Elements)).Length));
        newWave.reward = (Rewards)random.Next(0, System.Enum.GetNames(typeof(Rewards)).Length);
        newWave.enemyCountType = waveTypes[randomWaveTypeIdx].enemyCountType;
        newWave.enemyHealthType = waveTypes[randomWaveTypeIdx].enemyHealthType;
        newWave.enemySpeedType = waveTypes[randomWaveTypeIdx].enemySpeedType;
        newWave.enemyDamageType = waveTypes[randomWaveTypeIdx].enemyDamageType;

        return newWave;
    }

    private void Multiply(int waveNum) {
        this.enemyCount = (int)((float)this.enemyCount * Mathf.Pow(WAVE_DIFFICULTY_MULTIPLYER, waveNum));
        this.healthMin *= Mathf.Pow(WAVE_DIFFICULTY_MULTIPLYER, waveNum);
        this.healthMax *= Mathf.Pow(WAVE_DIFFICULTY_MULTIPLYER, waveNum);
        this.speedMin *= Mathf.Pow(WAVE_DIFFICULTY_MULTIPLYER, waveNum);
        this.speedMax *= Mathf.Pow(WAVE_DIFFICULTY_MULTIPLYER, waveNum);
        this.damageMin *= Mathf.Pow(WAVE_DIFFICULTY_MULTIPLYER, waveNum);
        this.damageMax *= Mathf.Pow(WAVE_DIFFICULTY_MULTIPLYER, waveNum);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static Utility;
public class WaveGenerator: MonoBehaviour {

    public const float WAVE_DIFFICULTY_MULTIPLYER = 1.05f;

    WaveType[] waveTypes;

    public void Awake() {
        waveTypes = Resources.LoadAll<WaveType>("Waves");
    }
    
    public List<Wave> GenerateRandomWaves(int waveNum, System.Random random, int waveQuantity)
    {
        List<Wave> waveOptions = new List<Wave>();

        for(int i = 0; i < waveQuantity; i ++)
        {
            waveOptions.Add(GenerateRandomWave(waveNum, random));
        }

        return waveOptions;
    }
    public Wave GenerateRandomWave(int waveNum, System.Random random) {

        //WaveType randomWaveType = waveTypes[Random.Range(0, waveTypes.Length)];
        WaveType randomWaveType = PickRandomWaveTypeByPriority(random);
        Wave newWave = new Wave(waveNum);

        newWave.enemyTypes = randomWaveType.enemyTypes;
        newWave.reward = randomWaveType.rewardType;
        while(newWave.reward == Rewards.RANDOM_ITEM)
            newWave.reward = (Rewards)random.Next(2, System.Enum.GetNames(typeof(Rewards)).Length);
        newWave.timeBetweenSpawnsType = randomWaveType.timeBetweenSpawnsType;
        newWave.enemyCountType = randomWaveType.enemyCountType;
        newWave.enemyHealthType = randomWaveType.enemyHealthType;
        newWave.enemySpeedType = randomWaveType.enemySpeedType;
        newWave.enemyDamageType = randomWaveType.enemyDamageType;
        newWave.waveType =  randomWaveType;

        switch(randomWaveType.timeBetweenSpawnsType) {
            case Difficulty.VERY_LOW:
                newWave.timeBetweenSpawns = 0.2f;
                break;
            case Difficulty.LOW:
                newWave.timeBetweenSpawns = 0.5f;
                break;
            case Difficulty.MEDIUM:
                newWave.timeBetweenSpawns = 1;
                break;
            case Difficulty.HIGH:
                newWave.timeBetweenSpawns = 1.5f;
                break;
            case Difficulty.VER_HIGH:
                newWave.timeBetweenSpawns = 2;
                break;
        }

        switch(randomWaveType.enemyCountType) {
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

        newWave.enemyCount = (int)Utility.DifficultyIncreaseFunction(waveNum, newWave.enemyCount, WAVE_DIFFICULTY_MULTIPLYER);

        return newWave;
    }

    private WaveType PickRandomWaveTypeByPriority(System.Random random) {
        int totalWeight = 0;
        foreach(WaveType waveType in waveTypes) {
            totalWeight += PriorityEnumToInt(waveType.priorityType);
        }

        int randomNumber = random.Next(0, totalWeight);
        int wavePriority;

        WaveType selectedWaveType = null;
        foreach(WaveType waveType in waveTypes) {
            wavePriority = PriorityEnumToInt(waveType.priorityType);
            if(randomNumber < wavePriority) {
                selectedWaveType = waveType;
                break;
            }
            randomNumber = randomNumber - wavePriority;
        }

        return selectedWaveType;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Utility;
public class WaveController : MonoBehaviour
{
    public const int WAVE_QUANTITY = 3;
    public int waveNumber = 0;
    public WaveGenerator generator;
    WaveCounterUI waveCounterUI;
    [SerializeField]
    NextWaveUI nextWaveUI;
    [SerializeField]
    Spawner spawner;
    System.Random random = new System.Random();

    public List<Wave> waveOptions;

    public Wave currentWave;

    [SerializeField]
    bool choosing = false;
    [SerializeField]

    bool readyToSpawn = false;

    void Start() {
        waveCounterUI = GameObject.Find("WaveCounter").GetComponent<WaveCounterUI>();
        nextWaveUI = GameObject.Find("NextWavePanel").GetComponent<NextWaveUI>();
        spawner = GameObject.FindWithTag("EnemySpawner").GetComponent<Spawner>();
        waveOptions = new List<Wave>();   
    }


    public void ChooseWave(int number) {
        Debug.Log(number);
        currentWave = waveOptions[number];
        readyToSpawn = true;
    }

    void Update () {

        if(spawner != null)
        {
            if(!spawner.spawning && !choosing) {
                if(currentWave != null) {
                    GiveWaveReward();
                    Debug.Log("Giving reward for wave end: " + currentWave.reward);
                }
                waveOptions = generator.GenerateRandomWaves(waveNumber + 1, random, WAVE_QUANTITY);
                nextWaveUI.ChooseNextWaveUI(this, waveOptions);
                choosing = true;
            }

            if(readyToSpawn)
            {
                waveNumber++;
                waveCounterUI.ChangeWaveNumber(waveNumber);
                spawner.CallWave(currentWave);
                nextWaveUI.StartWave();
                choosing = false;
                readyToSpawn = false;
            }
        }
        else
        {
            spawner = GameObject.FindWithTag("EnemySpawner").GetComponent<Spawner>();
        }
    }


    private void GiveWaveReward() {
        switch(currentWave.reward) {
            case Rewards.NONE:
                break;
            case Rewards.RANDOM_ITEM:
                Inventory.instance.AddRandom();
                break;
            case Rewards.ELEMENT_ITEM:
                Inventory.instance.AddRandomElementItem();
                break;
            case Rewards.EFFECT_ITEM:
                Inventory.instance.AddRandomEffectItem();
                break;
            case Rewards.STAT_ITEM:
                Inventory.instance.AddRandomStatItem();
                break;
            case Rewards.SOCKET_ITEM:
                Inventory.instance.AddRandomSocketItem();
                break;
            case Rewards.COMBAT_ITEM:
                Inventory.instance.AddRandomCombatItem();
                break;
            case Rewards.TOWER_ITEM:
                Inventory.instance.AddRandomTowerItem();
                break;
            case Rewards.BASE_HEAL:
                GameObject playerBase = GameObject.FindGameObjectWithTag("Base");
                playerBase.GetComponent<LivingEntity>().health += 10;
                break;
        }
    }
}
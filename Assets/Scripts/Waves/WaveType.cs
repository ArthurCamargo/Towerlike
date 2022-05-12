using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "New Wave Type", menuName = "Waves/Wave Type")]
public class WaveType : ScriptableObject {
    public new string name = "New Wave Type";
    public Sprite icon = null;
    public string description = "";
    public Utility.Difficulty timeBetweenSpawnsType, enemyCountType, enemyHealthType, enemySpeedType, enemyDamageType;
    public Utility.Difficulty enemyDropRateType;
    public List<EnemyType> enemyTypes;
}
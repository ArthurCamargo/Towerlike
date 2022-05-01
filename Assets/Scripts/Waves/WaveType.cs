using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "New Wave Type", menuName = "Waves/Wave Type")]
public class WaveType : ScriptableObject {
    public new string name = "New Wave Type";
    public Image icon;
    public string description = "";
    public Utility.Difficulty timeBetweenSpawnsType, enemyCountType, enemyHealthType, enemySpeedType, enemyDamageType;
    public List<EnemyType> enemyTypes;
}
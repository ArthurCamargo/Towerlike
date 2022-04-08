using UnityEngine;
using UnityEngine.EventSystems;


[CreateAssetMenu(fileName = "New Wave Type", menuName = "Waves/Wave Type")]
public class WaveType : ScriptableObject {
    new public string name = "New Wave Type";
    public string description = "";
    public Wave.Difficulty enemyCountType, enemyHealthType, enemySpeedType, enemyDamageType;
}
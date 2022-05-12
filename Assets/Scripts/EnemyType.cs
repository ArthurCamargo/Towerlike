using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


[CreateAssetMenu(fileName = "New Enemy Type", menuName = "Enemies/Enemy Type")]
public class EnemyType : ScriptableObject {
    public Enemy prefab;
    public new string name = "New Enemy Type";
    public string description = "";
    public Attributes.Elements element;
    public Utility.Difficulty healthType, speedType, damageType;
    public List<Item> dropList;
}
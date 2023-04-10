using Enums;
using ScriptableObjects;
using System;
using UnityEngine;

[Serializable]
public class EnemySetting
{
    public GameObject prefab;
    public EnemyType type;
    public EnemyModelData enemyModel;
}

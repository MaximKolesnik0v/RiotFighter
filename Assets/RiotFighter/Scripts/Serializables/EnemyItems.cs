using Enums;
using System;
using UnityEngine;

namespace Serializables
{
    [Serializable]
    public class EnemyItems
    {
        public GameObject prefab;
        public EnemyItemType itemType;
    }
}
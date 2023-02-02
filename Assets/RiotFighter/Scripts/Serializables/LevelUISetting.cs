using Enums;
using System;
using UnityEngine;

namespace Serializables
{
    [Serializable]
    public class LevelUISetting
    {
        public LevelUIItem LevelUIItem;
        public GameObject Prefab;
    }
}
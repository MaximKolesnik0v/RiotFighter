using Enums;
using System;
using UnityEngine;

namespace Serializables
{
    [Serializable]
    public class EffectSetting
    {
        public EffectType Type;
        public GameObject Prefab;
    }
}
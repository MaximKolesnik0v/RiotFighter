using Enums;
using System;
using UnityEngine;

namespace Serializables
{
    [Serializable]
    public class SoDataSetting
    {
        public ScriptableObject data;
        public SoDataType type;
    }
}


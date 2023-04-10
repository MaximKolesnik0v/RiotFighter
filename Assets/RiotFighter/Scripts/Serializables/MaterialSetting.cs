using Enums;
using System;
using UnityEngine;

namespace Serializables
{
    [Serializable]
    public class MaterialSetting
    {
        public MaterialType Type;
        public Material Material;
    }
}
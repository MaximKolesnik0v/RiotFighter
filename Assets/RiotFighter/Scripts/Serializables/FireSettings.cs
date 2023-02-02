using Enums.Fire;
using ScriptableObjects;
using System;
using UnityEngine;

namespace Serializables
{
    [Serializable]
    public class FireSettings
    {
        public GameObject prefab;
        public FireType type;
        public FireModelData fireModel;
    }
}

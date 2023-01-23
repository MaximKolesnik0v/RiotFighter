using Enums;
using Others;
using System.Collections.Generic;
using UnityEngine;

namespace Controllers
{
    public static class DataController
    {
        public static List<SoDataSetting> SoDataItems { get; set; }
        public static Dictionary<GameObjectRoot, Transform> GameObjectRoots { get; set; } = 
            new Dictionary<GameObjectRoot, Transform>();
        public static GameState GameState { get; set; } = GameState.START_PRESETS;
    }
}
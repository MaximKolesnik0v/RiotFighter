using Enums;
using System;
using System.Collections.Generic;
using UnityEngine;
using Views.Audio;

namespace Serializables
{
    [Serializable]
    public class Sound
    {
        [Header("Тип звука")]
        public AudioClipType Type;

        [Header("Список звуков этого типа")]
        public List<AudioView> Data;
    }
}
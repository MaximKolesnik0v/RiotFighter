using Serializables;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "AudioData", menuName = "ScriptableObjects/Audio/AudioData")]
    public class AudioData : ScriptableObject
    {
        [Header("Список аудио-префабов разных типов")]
        [SerializeField] private List<Sound> _sound;

        public List<Sound> Sound
        {
            get => _sound;
            set => _sound = value;
        }
    }
}
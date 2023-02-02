using Enums;
using UnityEngine;

namespace Views.Audio
{
    public class AudioView : MonoBehaviour
    {
        [SerializeField]
        private AudioClipType _audioClipType;

        [SerializeField]
        private AudioSource _audioSource;

        public AudioClipType AudioClipType
        {
            get => _audioClipType;
            set => _audioClipType = value;
        }
        public AudioSource AudioSource
        {
            get => _audioSource;
            set => _audioSource = value;
        }
    }
}
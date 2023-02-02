using Enums;
using ScriptableObjects;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Views.Audio;

namespace Controllers
{
    public class AudioController
    {
        private AudioData _audioData;
        private GameSettingsData _gameSettingsData;
        private Dictionary<int, AudioSource> _audioSources;

        public AudioController()
        {
            _audioData = DataController.SoDataItems
                .Where(_ => _.type == SoDataType.AUDIO_DATA)
                .Select(_ => _.data)
                .FirstOrDefault() as AudioData;
            _gameSettingsData = DataController.SoDataItems
                .Where(_ => _.type == SoDataType.GAME_SETTINGS_DATA)
                .Select(_ => _.data)
                .FirstOrDefault() as GameSettingsData;

            _audioSources = new Dictionary<int, AudioSource>();
        }

        public int PlaySound(AudioClipType type, bool isLoop = false, float volume = 1f, float delay = 0f)
        {
            var sounds = _audioData.Sound
                .Where(_ => _.Type == type)
                .Select(_ => _.Data)
                .FirstOrDefault();

            if (sounds == null)
            {
                throw new System.Exception($"Audio clip with type: {type} not found");
            }

            var sound = sounds[Random.Range(0, sounds.Count)];

            if (sound == null)
            {
                throw new System.Exception($"Audio clip with type: {type} not found");
            }
            return Play(sound, type, isLoop, volume, delay);
        }

        public void StopSound(int audioClipId)
        {
            AudioSource audioSource;
            var isExists = _audioSources.TryGetValue(audioClipId, out audioSource);
            if (isExists)
            {
                audioSource.Stop();
            }
        }

        public void Clear()
        {
            foreach (var source in _audioSources.Values)
            {
                if (source != null)
                {
                    Object.Destroy(source.gameObject);
                }
            }
        }

        public void SetVolume(float volume)
        {
            foreach (var source in _audioSources.Values)
            {
                source.volume = volume;
            }
        }

        private int Play(AudioView audioView, AudioClipType type, bool isLoop, float volume, float delay)
        {
            var audioGO = Object.Instantiate(audioView, DataController.GameObjectRoots[GameObjectRoot.AUDIO_SOUND_ROOT]);
            _audioSources.Add(audioGO.GetInstanceID(), audioGO.AudioSource);
            audioGO.AudioSource.volume *= _gameSettingsData.Volume;
            if (delay == 0f)
            {
                audioGO.AudioSource.Play();
            } else
            {
                audioGO.AudioSource.PlayDelayed(delay);
            }

            if (!audioGO.AudioSource.loop)
            {
                _audioSources.Remove(audioGO.GetInstanceID());
                Object.Destroy(audioGO.gameObject, audioGO.AudioSource.clip.length);
            }

            return audioGO.GetInstanceID();
        }
    }
}
using Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Views.Audio;

namespace Others
{
    public class AudioObjectsPool
    {
        private static readonly Lazy<AudioObjectsPool> _lazy =
            new Lazy<AudioObjectsPool>(() => new AudioObjectsPool());

        private Dictionary<int, AudioView> _gameObjectsPool;

        private AudioObjectsPool()
        {
            _gameObjectsPool = new Dictionary<int, AudioView>();
        }

        public static AudioObjectsPool GetInstance()
        {
            return _lazy.Value;
        }

        public AudioView GetIsExistOrNull(AudioClipType audioClipType)
        {
            if (_gameObjectsPool.Count > 0)
            {
                var freeObject = _gameObjectsPool.Values
                    .Where(_ => _.AudioClipType == audioClipType)
                    .FirstOrDefault();

                return freeObject;
            }
            return null;
        }

        public void Add(AudioView audioView)
        {
            if (!_gameObjectsPool.ContainsKey(audioView.gameObject.GetInstanceID()))
            {
                _gameObjectsPool.Add(audioView.gameObject.GetInstanceID(), audioView);
            }
        }

        public void Remove(int instanceId)
        {
            if (_gameObjectsPool.ContainsKey(instanceId))
            {
                _gameObjectsPool.Remove(instanceId);
            }
        }

        public void Clear()
        {
            _gameObjectsPool.Clear();
        }
    }
}
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Pool;

namespace AudioSystem {
    public class SoundManager : PersistentSingleton<SoundManager> {
        IObjectPool<SoundEmitter> soundEmitterPool;
        readonly List<SoundEmitter> activeSoundEmitters = new();
        public readonly LinkedList<SoundEmitter> FrequentSoundEmitters = new();

        [SerializeField] SoundEmitter soundEmitterPrefab;
        [SerializeField] bool collectionCheck = true;
        [SerializeField] int defaultCapacity = 10;
        [SerializeField] int maxPoolSize = 100;
        [SerializeField] int maxSoundInstances = 30;

        [SerializeField] private AudioMixerGroup m_EnvGroup;
        [SerializeField] private AudioMixerGroup m_MusicGroup;
        [SerializeField] private AudioMixerGroup m_SFXGroup;

        void Start() {
            InitializePool();
        }

        public SoundBuilder CreateSoundBuilder() => new SoundBuilder(this);

        public bool CanPlaySound(SoundData data) {
            if (!data.frequentSound) return true;

            if (FrequentSoundEmitters.Count >= maxSoundInstances) {
                try {
                    FrequentSoundEmitters.First.Value.Stop();
                    return true;
                } catch {
                    Debug.Log("SoundEmitter is already released");
                }
                return false;
            }
            return true;
        }

        public SoundEmitter Get() {
            return soundEmitterPool.Get();
        }

        public void ReturnToPool(SoundEmitter soundEmitter) {
            soundEmitterPool.Release(soundEmitter);
        }

        public void StopAll() {
            foreach (var soundEmitter in activeSoundEmitters) {
                soundEmitter.Stop();
            }

            FrequentSoundEmitters.Clear();
        }

        void InitializePool() {
            soundEmitterPool = new ObjectPool<SoundEmitter>(
                CreateSoundEmitter,
                OnTakeFromPool,
                OnReturnedToPool,
                OnDestroyPoolObject,
                collectionCheck,
                defaultCapacity,
                maxPoolSize);
        }

        SoundEmitter CreateSoundEmitter() {
            var soundEmitter = Instantiate(soundEmitterPrefab);
            soundEmitter.gameObject.SetActive(false);
            return soundEmitter;
        }

        void OnTakeFromPool(SoundEmitter soundEmitter) {
            soundEmitter.gameObject.SetActive(true);
            activeSoundEmitters.Add(soundEmitter);
        }

        void OnReturnedToPool(SoundEmitter soundEmitter) {
            if (soundEmitter.Node != null) {
                FrequentSoundEmitters.Remove(soundEmitter.Node);
                soundEmitter.Node = null;
            }
            soundEmitter.gameObject.SetActive(false);
            soundEmitter.transform.SetParent(transform);
            activeSoundEmitters.Remove(soundEmitter);
        }

        void OnDestroyPoolObject(SoundEmitter soundEmitter) {
            Destroy(soundEmitter.gameObject);
        }
        
        public static SoundEmitter PlayOneShot2DMusic(AudioClip audioClip, float volume = 1)
        {
            return PlayOneShot2D(audioClip, volume, instance.m_MusicGroup);
        }
        
        public static SoundEmitter PlayOneShot2DSFX(AudioClip audioClip, float volume = 1)
        {
            return PlayOneShot2D(audioClip, volume, instance.m_SFXGroup);
        }
        
        public static SoundEmitter PlayOneShot2DEnvironment(AudioClip audioClip, float volume = 1)
        {
            return PlayOneShot2D(audioClip, volume, instance.m_EnvGroup);
        }

        private static SoundEmitter PlayOneShot2D(AudioClip audioClip, float volume = 1, AudioMixerGroup audioMixerGroup = null)
        {
            var soundData = new SoundData
            {
                clip = audioClip,
                volume = volume
            };
            if (audioMixerGroup != null) soundData.mixerGroup = audioMixerGroup;
            
            SoundBuilder soundBuilder = SoundManager.Instance.CreateSoundBuilder();
            return soundBuilder.Play(soundData);
        }
    }
}
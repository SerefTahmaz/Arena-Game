using UnityEngine;

namespace AudioSystem {
    public class SoundBuilder {
        readonly SoundManager soundManager;
        Vector3 position = Vector3.zero;
        bool randomPitch;
        private Transform parent;

        public SoundBuilder(SoundManager soundManager) {
            this.soundManager = soundManager;
            parent = this.soundManager.transform;
        }

        public SoundBuilder WithPosition(Vector3 position) {
            this.position = position;
            return this;
        }

        public SoundBuilder WithRandomPitch() {
            this.randomPitch = true;
            return this;
        }

        public SoundBuilder WithParent(Transform parent)
        {
            this.parent = parent;
            return this;
        }

        public SoundEmitter Play(SoundData soundData) {
            if (soundData == null) {
                Debug.LogError("SoundData is null");
                return null;
            }
            
            if (!soundManager.CanPlaySound(soundData)) return null;
            
            SoundEmitter soundEmitter = soundManager.Get();
            soundEmitter.Initialize(soundData);
            soundEmitter.transform.position = position;
            soundEmitter.transform.parent = parent;

            if (randomPitch) {
                soundEmitter.WithRandomPitch();
            }

            if (soundData.frequentSound) {
                soundEmitter.Node = soundManager.FrequentSoundEmitters.AddLast(soundEmitter);
            }
            
            soundEmitter.Play();
            return soundEmitter;
        }
    }
}
using System;
using System.Collections.Generic;
using ArenaGame.Utils;
using AudioSystem;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Freeroam.Scripts.Animals
{
    public class AnimalSoundController : MonoBehaviour
    {
        [SerializeField] private List<AudioClip> m_AudioClips;
        [SerializeField] private SoundData m_SoundData;
        [SerializeField] private Vector2 m_PickDelay;
    
        // Start is called before the first frame update
        void Start()
        {
            RandomPatrol();
        }

        private async UniTask RandomPatrol()
        {
            while (true)
            {
                await UniTask.WaitForSeconds(Random.Range(m_PickDelay.x, m_PickDelay.y), cancellationToken: this.GetCancellationTokenOnDestroy());
                PlaySound();
            }
        }

        private void PlaySound()
        {
            m_SoundData.clip = m_AudioClips.RandomItem();
            SoundBuilder soundBuilder = SoundManager.Instance.CreateSoundBuilder();
                
            soundBuilder
                .WithRandomPitch()
                .WithPosition(transform.position)
                .WithParent(transform)
                .Play(m_SoundData);
        }
    }
}
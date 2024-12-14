using System.Collections.Generic;
using ArenaGame.Utils;
using UnityEngine;

namespace SoundManagers.VOs
{
    [CreateAssetMenu(fileName = "RandomVoiceClips", menuName = "Game/Audio/RandomVoiceClips", order = 0)]
    public class RandomVoiceClips : ScriptableObject
    {
        [SerializeField] private List<AudioClip> m_AudioClips;

        public List<AudioClip> AudioClips => m_AudioClips;

        public AudioClip GetClip()
        {
            return m_AudioClips.RandomItem();
        }
    }
}
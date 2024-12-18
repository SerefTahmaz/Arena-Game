using System.Collections.Generic;
using AudioSystem;
using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "SoundsHolder", menuName = "Game/SoundsHolder", order = 0)]
    public class SoundsHolder : ScriptableObject
    {
        [SerializeField] private List<AudioClip> m_AudioClips;

        public List<AudioClip> AudioClips => m_AudioClips;
    }
}
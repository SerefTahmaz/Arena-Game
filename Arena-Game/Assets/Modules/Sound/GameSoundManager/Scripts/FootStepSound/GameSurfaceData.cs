using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "GameSurfaceData", menuName = "Game/GameSurfaceData", order = 0)]
    public class GameSurfaceData : ScriptableObject
    {
        [SerializeField] private List<AudioClip> m_AudioClips;

        public List<AudioClip> AudioClips => m_AudioClips;
    }
}
using System.Collections.Generic;
using ArenaGame.Utils;
using UnityEngine;

namespace DefaultNamespace
{
    public class PathingHelper : cSingleton<PathingHelper>
    {
        [SerializeField] private List<Transform> m_Paths;

        public List<Transform> Paths => m_Paths;
    }
}
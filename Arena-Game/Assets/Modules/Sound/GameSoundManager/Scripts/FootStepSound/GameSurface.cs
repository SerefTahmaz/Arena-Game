using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{ 
    public class GameSurface : MonoBehaviour
    {
        [SerializeField] private GameSurfaceData m_GameSurfaceData;

        public GameSurfaceData GameSurfaceData => m_GameSurfaceData;
    }
}
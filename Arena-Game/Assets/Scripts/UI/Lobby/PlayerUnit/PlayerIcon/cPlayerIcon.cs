﻿using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "PlayerIcon", menuName = "Game/Lobby/PlayerIcon", order = 0)]
    public class cPlayerIcon : ScriptableObject
    {
        [SerializeField] private Sprite m_Icon;

        public Sprite Icon => m_Icon;
    }
}
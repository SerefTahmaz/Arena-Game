using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "PlayerIconList", menuName = "PlayerIconList", order = 0)]
    public class cPlayerIconList : ScriptableObject
    {
        [SerializeField] private List<cPlayerIcon> m_PlayerIcons;

        public List<cPlayerIcon> PlayerIcons => m_PlayerIcons;
    }
}
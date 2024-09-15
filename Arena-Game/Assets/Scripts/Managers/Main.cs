using System.Collections;
using System.Collections.Generic;
using ArenaGame.Utils;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

namespace ArenaGame
{
    public class Main : cSingleton<Main>
    {
        [SerializeField] private UnityTransport m_UnityTransport;

        public UnityTransport UnityTransport => m_UnityTransport;
    }
}

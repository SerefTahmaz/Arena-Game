using Unity.Collections;
using Unity.Netcode;
using Unity.Services.Authentication;
using UnityEngine;

namespace DefaultNamespace
{
    public class cClientScoreController : NetworkBehaviour
    {
        private NetworkVariable<FixedString128Bytes> m_PlayerName = new NetworkVariable<FixedString128Bytes>("Player",NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);
        private NetworkVariable<int> m_KillCount = new NetworkVariable<int>(0,NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);
        private NetworkVariable<int> m_DeadCount = new NetworkVariable<int>(0,NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);
        private NetworkVariable<int> m_IconIndex = new NetworkVariable<int>(0,NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);
        
        public NetworkVariable<FixedString128Bytes> PlayerName => m_PlayerName;

        public NetworkVariable<int> KillCount => m_KillCount;

        public NetworkVariable<int> DeadCount => m_DeadCount;

        public NetworkVariable<int> IconIndex => m_IconIndex;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            cScoreClientHolder.Instance.ClientScoreUnits.Add(this);

            if (IsOwner)
            {
                PlayerName.Value = cLobbyManager.Instance.PlayerName;
                IconIndex.Value = int.Parse(cLobbyManager.Instance.IconIndex);
                cScoreClientHolder.Instance.ClientScoreUnit = this;
            }
        }
    }
}
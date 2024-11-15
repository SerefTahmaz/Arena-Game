using System.Collections;
using System.Collections.Generic;
using ArenaGame;
using DefaultNamespace;
using DG.Tweening;
using Gameplay.Character;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class StartDebugModeGame : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField] private UnityTransport m_UnityTransport;
    
    void Start()
    {
        DOVirtual.DelayedCall(2, () =>
        {
            m_UnityTransport.SetConnectionData("127.0.0.1", 7777);
            NetworkManager.Singleton.StartHost();
            MultiplayerLocalHelper.instance.NetworkHelper.ResetState();
        
            foreach (var VARIABLE in NetworkManager.Singleton.ConnectedClients)
            {
                var player = OnClientConnected(VARIABLE.Key).transform;
            }

            DOVirtual.DelayedCall(2, () =>
            {

                MultiplayerLocalHelper.instance.NetworkHelper.m_IsGameStarted.Value = true;
                var playerSM = GameplayStatics.OwnerPlayer.GetComponent<cPlayerStateMachineV2>();
            });
        });
    }
    
    private GameObject OnClientConnected(ulong obj)
    {
        Vector3 pos;
        GameObject go;
        pos = SceneView.lastActiveSceneView.camera.transform.position;
        var transformForward = SceneView.lastActiveSceneView.camera.transform.forward;
        transformForward.y = 0;
        Vector3 dir = transformForward;
        var lookRot = Quaternion.LookRotation(dir.normalized);
        go = cPlayerManager.Instance.SpawnPlayer(pos, lookRot, obj);
        go.GetComponent<HumanCharacter>().CharacterNetworkController.m_TeamId.Value = 10;
        return go;
    }
#endif
}

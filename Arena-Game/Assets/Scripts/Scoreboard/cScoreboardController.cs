using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using ArenaGame.Utils;
using Unity.Collections;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class cScoreboardController : NetworkBehaviour
{
    [SerializeField] private cClientScoreController m_ClientScoreController;
    [SerializeField] private cScoreboardUIUnitController m_ScoreboardUIUnitController;
    [SerializeField] private Transform m_UITransform;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        // cGameManager.Instance.m_GameStarted += OnRoundStart;
    }

    public void OnRoundStart()
    {
        foreach (var VARIABLE in NetworkManager.Singleton.ConnectedClients)
        {
            SpawnPlayerScoreData(VARIABLE.Key);
        }
        NetworkManager.Singleton.OnClientConnectedCallback += SpawnPlayerScoreData;
        cGameManager.Instance.m_OnMainMenuButton += OnMainMenuButton;
    }

    private void OnMainMenuButton()
    {
        NetworkManager.Singleton.OnClientConnectedCallback -= SpawnPlayerScoreData;
        cGameManager.Instance.m_OnMainMenuButton -= OnMainMenuButton;
        ClearScoreDataDictClientRpc();
    }

    [ClientRpc]
    public void ClearScoreDataDictClientRpc() 
    {
        cScoreClientHolder.Instance.ClearDict();
    }

    private void SpawnPlayerScoreData(ulong obj)
    {
        var go = Spawn(transform.position, transform.rotation, obj);
        go.PlayerName.OnValueChanged += UpdateUI;
        go.KillCount.OnValueChanged += UpdateUI;
        go.DeadCount.OnValueChanged += UpdateUI;
        go.IconIndex.OnValueChanged += UpdateUI;
        UpdateUIClientRpc();
    }

    private void UpdateUI(FixedString128Bytes previousvalue, FixedString128Bytes newvalue)
    {
        UpdateUIClientRpc();
    }

    private void UpdateUI(int previousvalue, int newvalue)
    {
        UpdateUIClientRpc();
    }

    private cClientScoreController Spawn(Vector3 pos, Quaternion rot, ulong id)
    {
        var go = Instantiate(m_ClientScoreController,pos, rot);
        go.GetComponent<NetworkObject>().SpawnWithOwnership(id);
        return go;
    }

    private List<cScoreboardUIUnitController> m_GeneratedUIUnits = new List<cScoreboardUIUnitController>();

    [ClientRpc]
    public void UpdateUIClientRpc()
    {
        foreach (var VARIABLE in m_GeneratedUIUnits)
        {
            if(VARIABLE) Destroy(VARIABLE.gameObject);
        }
        m_GeneratedUIUnits.Clear();

        var orderedUnits = cScoreClientHolder.Instance.m_ClientScoreUnitsDic.Values.OrderByDescending((controller => controller.KillCount.Value)).ThenBy((controller => controller.DeadCount.Value));
        foreach (var VARIABLE in orderedUnits)
        {
            var ins = Instantiate(m_ScoreboardUIUnitController, FindObjectOfType<cGameplayMenuUIController>().ScoreBoardUITransform);
            ins.Init(VARIABLE.PlayerName.Value.Value, VARIABLE.KillCount.Value, VARIABLE.DeadCount.Value, VARIABLE.IconIndex.Value);
            m_GeneratedUIUnits.Add(ins);
        }
    }
    
    [ServerRpc(RequireOwnership = false)]
    public void AddKillServerRpc(ulong ownerId)
    {
        AddKillClientRpc(ownerId);
    }

    [ClientRpc]
    public void AddKillClientRpc(ulong ownerId)
    {
        cScoreClientHolder.Instance.AddKillClientRpc(ownerId);
    }
}

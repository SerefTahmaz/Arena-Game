using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    public Action OnPlayerEntered { get; set; }
    public Action OnPlayerExit { get; set; }
    public bool IsDetectingPlayer { get; set; }

    public bool IsPlayerInside => m_PlayerEnteranceCount > 0;

    private IPlayerMarker m_PlayerIns;
    
    private int m_PlayerEnteranceCount;

    private void Awake()
    {
        IsDetectingPlayer = true;
    }

    private void Update()
    {
        if (IsPlayerInside && m_PlayerIns == null)
        {
            PlayerExited();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody && other.attachedRigidbody.TryGetComponent(out IPlayerMarker playerMarker) && IsDetectingPlayer)
        {
            Debug.Log("Player Entered");
            PlayerEntered(playerMarker);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.attachedRigidbody && other.attachedRigidbody.TryGetComponent(out IPlayerMarker playerMarker) && IsDetectingPlayer)
        {
            Debug.Log("Player Exited");
            PlayerExited();
        }
    }
    
    private void PlayerEntered(IPlayerMarker playerMarker)
    {
        OnPlayerEntered?.Invoke();
        m_PlayerIns = playerMarker;
        m_PlayerEnteranceCount++;
    }

    private void PlayerExited()
    {
        OnPlayerExit?.Invoke();
        m_PlayerIns = null;
        m_PlayerEnteranceCount--;
    }
}

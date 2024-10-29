using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    public Action OnPlayerEntered { get; set; }
    public bool IsDetectingPlayer { get; set; }

    private void Awake()
    {
        IsDetectingPlayer = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody && other.attachedRigidbody.TryGetComponent(out IPlayerMarker playerMarker) && IsDetectingPlayer)
        {
            Debug.Log("Player Detected");
            OnPlayerEntered?.Invoke();
        }
    }
}

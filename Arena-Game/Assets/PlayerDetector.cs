using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    public Action OnPlayerEntered { get; set; }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody && other.attachedRigidbody.TryGetComponent(out IPlayerMarker playerMarker))
        {
            Debug.Log("Player Detected");
            OnPlayerEntered?.Invoke();
        }
    }
}

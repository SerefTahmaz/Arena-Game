using System;
using System.Collections;
using System.Collections.Generic;
using ArenaGame.Utils;
using DefaultNamespace;
using Dialogue;
using UnityEngine;

public class NPCDialogSetter : MonoBehaviour
{
    [SerializeField] private InteractableNPC m_InteractableNpc;
    [SerializeField] private List<DialogueGraph> m_DialogueGraphs; 

    private void Awake()
    {
        m_InteractableNpc.DialogGraph = m_DialogueGraphs.RandomItem();
    }
}

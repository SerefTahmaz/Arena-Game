using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ArenaGame.Utils;
using DefaultNamespace;
using STNest.Utils;
using UnityEngine;

public class PlayerInteractionHelper : cSingleton<PlayerInteractionHelper>
{
    private ObservableList<InteractableNPC> m_InteractableNpcs = new ObservableList<InteractableNPC>();

    private InteractableNPC m_CurrentInteractableNpc;

    public ObservableList<InteractableNPC> InteractableNpcs => m_InteractableNpcs;

    public void AddInteractionList(InteractableNPC interactableNpc)
    {
        if (!InteractableNpcs.Contains(interactableNpc))
        {
            InteractableNpcs.Add(interactableNpc);
        }
    }

    public void RemoveInteractionList(InteractableNPC interactableNpc)
    {
        if (!InteractableNpcs.Contains(interactableNpc)) return;

        interactableNpc.SetInteraction(false);
        InteractableNpcs.Remove(interactableNpc);
    }

    private void Update()
    {
        if(InteractableNpcs.Count <= 0) return;
        
        var player = FindObjectOfType<PlayerMarker>();
        if (player)
        {
            var pos = player.transform;

            foreach (var VARIABLE in InteractableNpcs)
            {
                VARIABLE.SetInteraction(false);
            }

            var closest = InteractableNpcs.OrderBy((npc => Vector3.Distance(pos.position, npc.transform.position))).FirstOrDefault();
            
            if (closest != null)
            {
                closest.SetInteraction(true);
            }
        }
    }
    
    public void HandleDialogStarted(InteractableNPC interactableNpc)
    {
        cUIManager.Instance.HidePage(Page.Gameplay,this);
    }

    public void HandleDialogEnded(InteractableNPC interactableNpc)
    {
        cUIManager.Instance.ShowPage(Page.Gameplay,this);
    }
}

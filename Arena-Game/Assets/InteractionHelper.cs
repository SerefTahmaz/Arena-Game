using System.Linq;
using ArenaGame.Utils;
using DefaultNamespace;
using STNest.Utils;
using UnityEngine;

public class InteractionHelper : cSingleton<InteractionHelper>
{
    private ObservableList<BaseInteractable> m_InteractableItems = new ObservableList<BaseInteractable>();

    private BaseInteractable m_CurrentInteractableNpc;

    public ObservableList<BaseInteractable> InteractableNpcs => m_InteractableItems;

    public void AddInteractionList(BaseInteractable interactableNpc)
    {
        if (!InteractableNpcs.Contains(interactableNpc))
        {
            InteractableNpcs.Add(interactableNpc);
        }
    }

    public void RemoveInteractionList(BaseInteractable interactableNpc)
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
    
    public void HandleDialogStarted(BaseInteractable interactableNpc)
    {
        cUIManager.Instance.HidePage(Page.Gameplay,this);
    }

    public void HandleDialogEnded(BaseInteractable interactableNpc)
    {
        cUIManager.Instance.ShowPage(Page.Gameplay,this);
    }
}
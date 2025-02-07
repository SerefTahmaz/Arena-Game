using System;
using System.Linq;
using ArenaGame.Utils;
using DefaultNamespace;
using STNest.Utils;
using UnityEngine;

public class InteractionHelper<T> : cSingleton<T> where T : MonoBehaviour
{
    private ObservableList<BaseInteractable> m_Interactables = new ObservableList<BaseInteractable>();
    private BaseInteractable m_CurrentInteractable;

    public ObservableList<BaseInteractable> Interactables => m_Interactables;
    
    public Action OnCurrentInteractableChanged { get; set; }
    public BaseInteractable CurrentInteractable => m_CurrentInteractable;

    public void AddInteractionList(BaseInteractable interactableNpc)
    {
        if (!Interactables.Contains(interactableNpc))
        {
            Interactables.Add(interactableNpc);
        }
    }

    public void RemoveInteractionList(BaseInteractable interactableNpc)
    {
        if (!Interactables.Contains(interactableNpc)) return;

        interactableNpc.SetInteraction(false);
        Interactables.Remove(interactableNpc);
    }

    private void Update()
    {
        if (Interactables.Count <= 0)
        {
            SetCurrentInteractable(null);
            return;
        }
        
        var player = FindObjectOfType<PlayerMarker>();
        if (player)
        {
            var pos = player.transform;

            foreach (var VARIABLE in Interactables)
            {
                VARIABLE.SetInteraction(false);
            }

            var closest = Interactables.OrderBy((npc => Vector3.Distance(pos.position, npc.transform.position))).FirstOrDefault();
            
            if (closest != null)
            {
                closest.SetInteraction(true);
                SetCurrentInteractable(closest);
            }
        }
    }

    public void SetCurrentInteractable(BaseInteractable interactable)
    {
        if (interactable != null && CurrentInteractable != interactable)
        {
            m_CurrentInteractable = interactable;
            OnCurrentInteractableChanged?.Invoke();
        }
    }
    
    public void HandleInteractionStarted(BaseInteractable interactableNpc)
    {
        cUIManager.Instance.HidePage(Page.Gameplay,this);
    }

    public void HandleInteractionEnded(BaseInteractable interactableNpc)
    {
        cUIManager.Instance.ShowPage(Page.Gameplay,this);
    }
}
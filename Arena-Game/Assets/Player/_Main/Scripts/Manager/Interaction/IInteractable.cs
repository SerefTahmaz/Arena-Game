using UnityEngine;

public interface IInteractable
{
    void OnPreviewInteract(InteractionHelper interactionHelper);
    void OnExitInteract(InteractionHelper interactionHelper);
}
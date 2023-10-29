using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionTrigger : MonoBehaviour
{
    public InteractionHelper InteractionHelper
    {
        get => interactionHelper;
        set => interactionHelper = value;
    }
    private InteractionHelper interactionHelper;
}

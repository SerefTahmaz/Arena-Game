using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InteractionHelper
{
    public Actor Actor;
    public Transform Transform;
    public ObjectState ObjectState;
    public TextAsset Dialogue;
}

public enum ObjectState
{
    Active,
    Dead
}

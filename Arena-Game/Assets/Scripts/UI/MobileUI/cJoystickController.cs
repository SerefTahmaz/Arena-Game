using System;
using System.Collections;
using System.Collections.Generic;
using ArenaGame.Utils;
using Lean.Touch;
using RootMotion;
using UnityEngine;

public class cJoystickController : cSingleton<cJoystickController>
{
    [SerializeField] private VariableJoystick m_VariableJoystick;
    
    public Vector2 JoystickValue => m_VariableJoystick.Direction;
}

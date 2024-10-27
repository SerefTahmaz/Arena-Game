using System;
using System.Collections;
using System.Collections.Generic;
using ArenaGame;
using ArenaGame.Managers.SaveManager;
using TMPro;
using UnityEngine;

public class WinsInfoView : InfoView
{
    public override int TargetValue => ProfileGenerator.GetPlayerProfile().WinsCount;
    public override Action TargetChangeEvent
    {
        get => SaveGameHandler.OnChanged;
        set =>  SaveGameHandler.OnChanged = value;
    }
}

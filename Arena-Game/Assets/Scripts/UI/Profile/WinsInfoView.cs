using System.Collections;
using System.Collections.Generic;
using ArenaGame;
using TMPro;
using UnityEngine;

public class WinsInfoView : InfoView
{
    public override int TargetValue => ProfileGenerator.GetPlayerProfile().WinsCount;
}

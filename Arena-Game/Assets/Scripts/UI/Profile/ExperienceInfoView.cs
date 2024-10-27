using System;
using ArenaGame.Managers.SaveManager;
using UnityEngine;

namespace ArenaGame.UI.Profile
{
    public class ExperienceInfoView : InfoView
    {
        public override int TargetValue => ProfileGenerator.GetPlayerProfile().ExpPoint;
        public override Action TargetChangeEvent
        {
            get => SaveGameHandler.OnChanged;
            set =>  SaveGameHandler.OnChanged = value;
        }
    }
}
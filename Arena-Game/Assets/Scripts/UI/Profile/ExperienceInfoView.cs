using UnityEngine;

namespace ArenaGame.UI.Profile
{
    public class ExperienceInfoView : InfoView
    {
        public override int TargetValue => ProfileGenerator.GetPlayerProfile().ExpPoint;
    }
}
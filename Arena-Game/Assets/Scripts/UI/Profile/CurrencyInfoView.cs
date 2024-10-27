using System;
using DefaultNamespace;

namespace ArenaGame.UI.Profile
{
    public class CurrencyInfoView : InfoView
    {
        public override int TargetValue => GameplayStatics.GetPlayerCharacterSO().Currency;
        public override Action TargetChangeEvent
        {
            get => GameplayStatics.GetPlayerCharacterSO().OnChanged;
            set => GameplayStatics.GetPlayerCharacterSO().OnChanged = value;
        }
    }
}
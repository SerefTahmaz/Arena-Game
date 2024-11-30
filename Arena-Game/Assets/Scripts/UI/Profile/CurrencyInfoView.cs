using System;
using DefaultNamespace;

namespace ArenaGame.UI.Profile
{
    public class CurrencyInfoView : InfoView
    {
        public override int TargetValue => GameplayStatics.GetPlayerCharacterSO().GetCharacterSave().Currency;
        public override Action TargetChangeEvent
        {
            get => GameplayStatics.GetPlayerCharacterSO().GetCharacterSave().OnChanged;
            set => GameplayStatics.GetPlayerCharacterSO().GetCharacterSave().OnChanged = value;
        }
    }
}
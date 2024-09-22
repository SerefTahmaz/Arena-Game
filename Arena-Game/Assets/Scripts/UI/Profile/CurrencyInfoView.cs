namespace ArenaGame.UI.Profile
{
    public class CurrencyInfoView : InfoView
    {
        public override int TargetValue => ProfileGenerator.GetPlayerProfile().Currency;
    }
}
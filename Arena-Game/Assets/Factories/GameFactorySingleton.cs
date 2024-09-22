using _Main.Scripts;
using ArenaGame.Utils;

public class GameFactorySingleton : cSingleton<GameFactorySingleton>
{
    public IPurchasePopUpFactory PurchasePopUpFactory { get; private set; }
    public IInfoPopUpFactory InfoPopUpFactory { get; private set; }
    
    private void Awake()
    {
        PurchasePopUpFactory = new PurchasePopUpFactory();
        InfoPopUpFactory = new InfoPopUpFactory();
    }
}

public class GlobalFactory
{
    public static IPurchasePopUpFactory PurchasePopUpFactory => GameFactorySingleton.Instance.PurchasePopUpFactory;
    public static IInfoPopUpFactory InfoPopUpFactory => GameFactorySingleton.Instance.InfoPopUpFactory;
}
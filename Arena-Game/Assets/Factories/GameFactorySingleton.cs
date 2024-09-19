using _Main.Scripts;
using ArenaGame.Utils;

public class GameFactorySingleton : cSingleton<GameFactorySingleton>
{
    public IPurchasePopUpFactory PurchasePopUpFactory { get; private set; }
    
    private void Awake()
    {
        PurchasePopUpFactory = new PurchasePopUpFactory();
    }
}

public class GlobalFactory
{
    public static IPurchasePopUpFactory m_PurchasePopUpFactory = GameFactorySingleton.Instance.PurchasePopUpFactory;
}
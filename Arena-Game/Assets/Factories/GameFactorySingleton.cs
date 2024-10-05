using _Main.Scripts;
using ArenaGame.Utils;

public class GameFactorySingleton : cSingleton<GameFactorySingleton>
{
    public IPurchasePopUpFactory PurchasePopUpFactory { get; private set; }
    public IInfoPopUpFactory InfoPopUpFactory { get; private set; }
    public IDisconnectedPopUpFactory DisconnectedPopUpFactory { get; private set; }
    public IInfoPopUpFactory DisqualifyPopUpFactory { get; private set; }
    
    private void Awake()
    {
        PurchasePopUpFactory = new PurchasePopUpFactory();
        InfoPopUpFactory = new InfoPopUpFactory(PrefabList.Get().InfoPopUpPrefab);
        DisconnectedPopUpFactory = new DisconnectedPopUpFactory();
        DisqualifyPopUpFactory = new InfoPopUpFactory(PrefabList.Get().DisqualifyPopUpPrefab);
    }
}

public class GlobalFactory
{
    public static IPurchasePopUpFactory PurchasePopUpFactory => GameFactorySingleton.Instance.PurchasePopUpFactory;
    public static IInfoPopUpFactory InfoPopUpFactory => GameFactorySingleton.Instance.InfoPopUpFactory;
    public static IDisconnectedPopUpFactory DisconnectedPopUpFactory => GameFactorySingleton.Instance.DisconnectedPopUpFactory;
    public static IInfoPopUpFactory DisqualifyPopUpFactory => GameFactorySingleton.Instance.DisqualifyPopUpFactory;
}
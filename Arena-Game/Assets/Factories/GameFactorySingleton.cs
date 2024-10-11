using _Main.Scripts;
using ArenaGame.Utils;
using Factories;

public class GameFactorySingleton : cSingleton<GameFactorySingleton>
{
    public IPurchasePopUpFactory PurchasePopUpFactory { get; private set; }
    public IInfoPopUpFactory InfoPopUpFactory { get; private set; }
    public IDisconnectedPopUpFactory DisconnectedPopUpFactory { get; private set; }
    public IInfoPopUpFactory DisqualifyPopUpFactory { get; private set; }
    public IPopUpFactory NoWifiPopUpFactory { get; private set; }
    public ISeedSelectorPopUpFactory SeedSelectorPopUpFactory { get; private set; }
    public IPlantFieldCollectPopUpFactory PlantFieldCollectPopUpFactory { get; private set; }
    public ITransactionShopPopUpFactory TransactionShopPopUpFactory { get; private set; }
    
    private void Awake()
    {
        PurchasePopUpFactory = new PurchasePopUpFactory();
        InfoPopUpFactory = new InfoPopUpFactory(PrefabList.Get().InfoPopUpPrefab);
        DisconnectedPopUpFactory = new DisconnectedPopUpFactory();
        DisqualifyPopUpFactory = new InfoPopUpFactory(PrefabList.Get().DisqualifyPopUpPrefab);
        NoWifiPopUpFactory = new PopUpFactory(PrefabList.Get().NoWifiPopUpPrefab);
        SeedSelectorPopUpFactory = new SeedSelectorPopUpFactory(PrefabList.Get().SeedSelectorPopUpPrefab);
        PlantFieldCollectPopUpFactory = new PlantFieldCollectPopUpFactory();
        TransactionShopPopUpFactory = new TransactionShopPopUpFactory();
    }
}

public class GlobalFactory
{
    public static IPurchasePopUpFactory PurchasePopUpFactory => GameFactorySingleton.Instance.PurchasePopUpFactory;
    public static IInfoPopUpFactory InfoPopUpFactory => GameFactorySingleton.Instance.InfoPopUpFactory;
    public static IDisconnectedPopUpFactory DisconnectedPopUpFactory => GameFactorySingleton.Instance.DisconnectedPopUpFactory;
    public static IInfoPopUpFactory DisqualifyPopUpFactory => GameFactorySingleton.Instance.DisqualifyPopUpFactory;
    public static IPopUpFactory NoWifiPopUpFactory => GameFactorySingleton.Instance.NoWifiPopUpFactory;
    public static ISeedSelectorPopUpFactory SelectorPopUpFactory => GameFactorySingleton.Instance.SeedSelectorPopUpFactory;
    public static IPlantFieldCollectPopUpFactory PlantFieldCollectPopUpFactory => GameFactorySingleton.Instance.PlantFieldCollectPopUpFactory;

    public static ITransactionShopPopUpFactory TransactionShopPopUpFactory => GameFactorySingleton.Instance.TransactionShopPopUpFactory;
}
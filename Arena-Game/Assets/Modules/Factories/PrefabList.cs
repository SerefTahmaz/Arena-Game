using System.Collections.Generic;
using _Main.Scripts;
using ArenaGame.UI;
using ArenaGame.UI.MenuInventory;
using ArenaGame.UI.PopUps.DisconnectedPopUp;
using ArenaGame.UI.PopUps.InfoPopUp;
using Factories;
using UI.EndScreen;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Prefab List", menuName = "Game/Prefab List")]
public class PrefabList : ScriptableObject
{
    [SerializeField] private PurchasePopUpController m_PurchasePopUpPrefab;
    [SerializeField] private InfoPopUpController m_InfoPopUpPrefab;
    [SerializeField] private DisconnectedPopUpController m_DisconnectedPopUpPrefab;
    [SerializeField] private WinCurrencyRewardController m_WinCurrencyRewardPrefab;
    [SerializeField] private WinUpgradeRewardController m_WinUpgradeRewardPrefab; 
    [SerializeField] private InfoPopUpController m_DisqualifyPopUpPrefab; 
    [SerializeField] private NoWifiPopUpController m_NoWifiPopUpPrefab;
    [SerializeField] private SeedSelectorPopUpController m_SeedSelectorPopUpPrefab;
    [SerializeField] private PlantFieldCollectPopUpController m_PlantFieldCollectPopUpPrefab;
    [SerializeField] private TransactionShopPopUpController m_TransactionShopPopUpPrefab;
    [SerializeField] private DialogController m_DialogControllerPrefab;
    [SerializeField] private MatchMakingController m_MatchMakingController;
    [SerializeField] private ChoicePopUpUIController m_ChoicePopUpUIController;
  
    public PurchasePopUpController PurchasePopUpPrefab => m_PurchasePopUpPrefab;
    public InfoPopUpController InfoPopUpPrefab => m_InfoPopUpPrefab;
    public WinCurrencyRewardController CurrencyRewardPrefab => m_WinCurrencyRewardPrefab;
    public WinUpgradeRewardController WinUpgradeRewardPrefab => m_WinUpgradeRewardPrefab;
    public DisconnectedPopUpController DisconnectedPopUpPrefab => m_DisconnectedPopUpPrefab;
    public InfoPopUpController DisqualifyPopUpPrefab => m_DisqualifyPopUpPrefab;
    public NoWifiPopUpController NoWifiPopUpPrefab => m_NoWifiPopUpPrefab;
    public SeedSelectorPopUpController SeedSelectorPopUpPrefab => m_SeedSelectorPopUpPrefab;
    public PlantFieldCollectPopUpController PlantFieldCollectPopUpPrefab => m_PlantFieldCollectPopUpPrefab;
    public TransactionShopPopUpController TransactionShopPopUpPrefab => m_TransactionShopPopUpPrefab;
    public DialogController DialogControllerPrefab => m_DialogControllerPrefab;
    public MatchMakingController MatchMakingController => m_MatchMakingController;
    public ChoicePopUpUIController ChoicePopUpUIController => m_ChoicePopUpUIController;

    private static PrefabList m_Instance;
    
    public static PrefabList Get()
    {
        if (m_Instance == null)
        {
            m_Instance=Resources.Load<PrefabList>("Prefab List");
        }
            
        return m_Instance;
    }
}
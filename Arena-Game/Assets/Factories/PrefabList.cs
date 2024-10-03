using System.Collections.Generic;
using _Main.Scripts;
using ArenaGame.UI.PopUps.DisconnectedPopUp;
using ArenaGame.UI.PopUps.InfoPopUp;
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
  
    public PurchasePopUpController PurchasePopUpPrefab => m_PurchasePopUpPrefab;
    public InfoPopUpController InfoPopUpPrefab => m_InfoPopUpPrefab;
    public WinCurrencyRewardController CurrencyRewardPrefab => m_WinCurrencyRewardPrefab;
    public WinUpgradeRewardController WinUpgradeRewardPrefab => m_WinUpgradeRewardPrefab;
    public DisconnectedPopUpController DisconnectedPopUpPrefab => m_DisconnectedPopUpPrefab;

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
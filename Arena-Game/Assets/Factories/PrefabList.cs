using System.Collections.Generic;
using _Main.Scripts;
using ArenaGame.UI.PopUps.InfoPopUp;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Prefab List", menuName = "Game/Prefab List")]
public class PrefabList : ScriptableObject
{
    [SerializeField] private PurchasePopUpController m_PurchasePopUpPrefab;
    [SerializeField] private InfoPopUpController m_InfoPopUpPrefab;
  
    public PurchasePopUpController PurchasePopUpPrefab => m_PurchasePopUpPrefab;
    public InfoPopUpController InfoPopUpPrefab => m_InfoPopUpPrefab;

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
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using Gameplay;
using UI.Shop;
using UnityEngine;

public class FarmMerchantController : MonoBehaviour
{
    [SerializeField] private CharacterSO m_FarmerChar;
    [SerializeField] private List<BaseItemSO> m_FarmerMarketSOs;
    
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody && other.attachedRigidbody.TryGetComponent(out IPlayerMarker playerMarker))
        {
            m_FarmerChar.Load();
            m_FarmerChar.InventoryList = new List<BaseItemSO>();
            m_FarmerChar.Save();
            m_FarmerChar.GainCurrency(999999);
            var items = m_FarmerMarketSOs.Select((so => so.DuplicateUnique()));
            foreach (var VARIABLE in  items)
            {
                m_FarmerChar.AddInventory(VARIABLE);
                m_FarmerChar.Save();
            }
            var insPopUp = GlobalFactory.TransactionShopPopUpFactory.Create();
            insPopUp.Init(GameplayStatics.GetPlayerCharacterSO(), m_FarmerChar);
        }
    }
}

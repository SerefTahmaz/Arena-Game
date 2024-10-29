using System;
using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu]
    public class DialogHelper : ScriptableObject
    {
        public Action ShowMerchantInventory { get; set; }
        
        public void EndDialog()
        {
            
        }

        public void HandleShowMerchantInventory()
        {
            ShowMerchantInventory?.Invoke();
        }
    }
}
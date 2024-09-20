using System.Collections.Generic;
using System.Linq;
using Item;
using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "ItemList", menuName = "Item/Item List", order = 0)]
    public class ItemListSO : Registry<BaseItemSO>
    {
        public static T GetItemByGuid<T>(string id) where T : BaseItemSO
        {
            var itemList = Resources.Load<ItemListSO>("Item/ItemList");
            var itemSO = itemList._descriptors.Where((so => id == so.Guid.ToHexString())).FirstOrDefault();
        
            if (itemSO is T castedItem)
            {
                return castedItem;
            }
            else
            {
                return null;
            }
        }
    }
}
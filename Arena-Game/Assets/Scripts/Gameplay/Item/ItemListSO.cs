using System.Collections.Generic;
using System.Linq;
using Item;
using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "ItemList", menuName = "Game/Item/Item List", order = 0)]
    public class ItemListSO : Registry<BaseItemTemplateSO>
    {
        public static T GetItemByGuid<T>(string id) where T : BaseItemTemplateSO
        {
            var itemList = Resources.Load<ItemListSO>("Item/TemplateItemList");
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
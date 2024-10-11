using Item;
using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "Food Item Template", menuName = "Game/Item/Template/Template Food Item", order = 0)]
    public class FoodItemTemplateSO : BaseItemTemplateSO
    {
        [SerializeField] private int m_Price;

        public int Price => m_Price;
    }
}
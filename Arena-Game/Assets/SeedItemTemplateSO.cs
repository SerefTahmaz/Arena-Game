using Item;
using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "Seed Item Template", menuName = "Game/Item/Template/Template Seed Item", order = 0)]
    public class SeedItemTemplateSO : BaseItemTemplateSO
    {
        [SerializeField] private PlantItemSO m_PlantToBeBorn;
        [SerializeField] private int m_Price;

        public PlantItemSO PlantToBeBorn => m_PlantToBeBorn;
        public int Price => m_Price;
    }
}
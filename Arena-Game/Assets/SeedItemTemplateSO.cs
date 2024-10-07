using Item;
using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "Seed Item Template", menuName = "Game/Item/Template/Template Seed Item", order = 0)]
    public class SeedItemTemplateSO : BaseItemTemplateSO
    {
        [SerializeField] private PlantItemSO m_PlantToBeBorn;

        public PlantItemSO PlantToBeBorn => m_PlantToBeBorn;
    }
}
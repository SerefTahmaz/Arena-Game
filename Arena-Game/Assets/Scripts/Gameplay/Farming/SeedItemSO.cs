using DefaultNamespace;
using Item;
using UnityEngine;

namespace Gameplay.Farming
{
    [CreateAssetMenu(fileName = "Tomato Item", menuName = "Game/Item/Seed Item", order = 0)]
    public class SeedItemSO : BaseItemSO
    {
        [SerializeField] private BaseItemTemplateSO m_SeedItemTemplate;
        [SerializeField] private PlantItemSO m_PlantToBeBorn;

        public PlantItemSO GivePlantItemInsSO()
        {
            var insPlantToBeBorn = m_PlantToBeBorn.DuplicateUnique() as PlantItemSO;
            return insPlantToBeBorn;
        }
    }
}
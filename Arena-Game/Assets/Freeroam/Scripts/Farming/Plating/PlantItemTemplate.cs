using Gameplay.Farming;
using Item;
using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "Plant Item", menuName = "Game/Item/Template/Template Plant Item", order = 0)]
    public class PlantItemTemplate : BaseItemTemplateSO
    {
        [SerializeField] private PlantController m_PlantPrefab;
        [SerializeField] private FoodItemSO m_ProducedFoodItem;

        public PlantController PlantPrefab => m_PlantPrefab;
        public FoodItemSO ProducedFoodItem => m_ProducedFoodItem;
    }
}
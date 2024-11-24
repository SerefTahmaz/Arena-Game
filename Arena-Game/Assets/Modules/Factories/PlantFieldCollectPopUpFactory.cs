using ArenaGame.UI;
using UnityEngine;

namespace _Main.Scripts
{
    public interface IPlantFieldCollectPopUpFactory
    {
        public IPlantFieldCollectPopUpController Create();
    }
    
    public class PlantFieldCollectPopUpFactory : IPlantFieldCollectPopUpFactory
    {

        public PlantFieldCollectPopUpFactory()
        {
        }

        public IPlantFieldCollectPopUpController Create()
        {
            var ins = GameObject.Instantiate(PrefabList.Get().PlantFieldCollectPopUpPrefab,cUIManager.Instance.transform);
            // ins.Init();
            return ins;
        }
    }
}
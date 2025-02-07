using ArenaGame.Currency;
using ArenaGame.Utils;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ArenaGame.UI
{
    public class PlantFieldCollectPopUpController : YesNoPopUpController,IPlantFieldCollectPopUpController
    {
        public async UniTask<bool> Init()
        {
            var result = await base.Init($"The plant is {("fully grown").ColorHtmlString(Color.green)}. Collect the product");
            return result;
        }
    }

    public interface IPlantFieldCollectPopUpController
    {
        UniTask<bool> Init();
    }
}
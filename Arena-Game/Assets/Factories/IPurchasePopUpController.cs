using Cysharp.Threading.Tasks;

namespace _Main.Scripts
{
    public interface IPurchasePopUpController
    {
        public UniTask<bool> Init(string itemToPurchaseName, string value);
    }
}
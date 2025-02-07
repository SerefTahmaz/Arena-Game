using Cysharp.Threading.Tasks;
using Gameplay;

namespace _Main.Scripts
{
    public interface IPurchasePopUpController
    {
        public UniTask<bool> Init(CharacterSO sourceChar, CharacterSO targetChar, string itemToPurchaseName, int value, bool isPlayerSelling);
    }
}
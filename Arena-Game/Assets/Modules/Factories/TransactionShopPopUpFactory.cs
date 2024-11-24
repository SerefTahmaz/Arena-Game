using ArenaGame.UI;
using Factories;
using UnityEngine;

namespace _Main.Scripts
{
    public interface ITransactionShopPopUpFactory
    {
        public ITransactionShopPopUpController Create();
    }
    
    public class TransactionShopPopUpFactory : ITransactionShopPopUpFactory
    {

        public TransactionShopPopUpFactory()
        {
        }

        public ITransactionShopPopUpController Create()
        {
            var ins = GameObject.Instantiate(PrefabList.Get().TransactionShopPopUpPrefab,cUIManager.Instance.transform);
            // ins.Init();
            return ins;
        }
    }
}
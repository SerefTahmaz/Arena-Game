using UnityEngine;

namespace _Main.Scripts
{
    public interface IPurchasePopUpFactory
    {
        public IPurchasePopUpController Create();
    }
    
    public class PurchasePopUpFactory : IPurchasePopUpFactory
    {

        public PurchasePopUpFactory()
        {
        }

        public IPurchasePopUpController Create()
        {
            var ins = GameObject.Instantiate(PrefabList.Get().PurchasePopUpPrefab,cUIManager.Instance.transform);
            // ins.Init();
            return ins;
        }
    }
}
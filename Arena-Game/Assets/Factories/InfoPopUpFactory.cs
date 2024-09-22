using ArenaGame.UI.PopUps.InfoPopUp;
using UnityEngine;

namespace _Main.Scripts
{
    public interface IInfoPopUpFactory
    {
        public IInfoPopUpController Create();
    }
    
    public class InfoPopUpFactory : IInfoPopUpFactory
    {

        public InfoPopUpFactory()
        {
        }

        public IInfoPopUpController Create()
        {
            var ins = GameObject.Instantiate(PrefabList.Get().InfoPopUpPrefab,cUIManager.Instance.transform);
            // ins.Init();
            return ins;
        }
    }
}
using ArenaGame.UI.PopUps.DisconnectedPopUp;
using ArenaGame.UI.PopUps.InfoPopUp;
using UnityEngine;

namespace _Main.Scripts
{
    public interface IDisconnectedPopUpFactory
    {
        public IDisconnectedPopUpController Create();
    }
    
    public class DisconnectedPopUpFactory : IDisconnectedPopUpFactory
    {

        public DisconnectedPopUpFactory()
        {
        }

        public IDisconnectedPopUpController Create()
        {
            var ins = GameObject.Instantiate(PrefabList.Get().DisconnectedPopUpPrefab,cUIManager.Instance.transform);
            // ins.Init();
            return ins;
        }
    }
}
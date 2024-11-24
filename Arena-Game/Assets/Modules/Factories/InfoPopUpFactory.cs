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
        private InfoPopUpController m_Prefab;
        
        public InfoPopUpFactory(InfoPopUpController prefab)
        {
            m_Prefab = prefab;
        }

        public IInfoPopUpController Create()
        {
            var ins = GameObject.Instantiate(m_Prefab,cUIManager.Instance.transform);
            // ins.Init();
            return ins;
        }
    }
}
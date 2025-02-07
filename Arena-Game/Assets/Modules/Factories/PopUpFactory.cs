using UnityEngine;

namespace Factories
{
    public interface IPopUpFactory
    {
        public IPopUpController Create();
    }
    
    public class PopUpFactory : IPopUpFactory
    {
        private PopUpController m_Prefab;
        
        public PopUpFactory(PopUpController prefab)
        {
            m_Prefab = prefab;
        }

        public IPopUpController Create()
        {
            var ins = GameObject.Instantiate(m_Prefab,cUIManager.Instance.transform);
            // ins.Init();
            return ins;
        }
    }
}
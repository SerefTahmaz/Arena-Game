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
            //TODO: Fix this
            Transform parent = null;
            if (cUIManager.Instance)
            {
                parent = cUIManager.Instance.transform;
            }
            else
            {
                parent = SetupUIManager.instance.transform;
            }
            
            var ins = GameObject.Instantiate(m_Prefab,parent);
            // ins.Init();
            return ins;
        }
    }
}
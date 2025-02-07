using ArenaGame.UI.MenuInventory;
using ArenaGame.UI.PopUps.InfoPopUp;
using UnityEngine;

namespace _Main.Scripts
{
    public interface ISeedSelectorPopUpFactory
    {
        public ISeedSelectorPopUpController Create();
    }
    
    public class SeedSelectorPopUpFactory : ISeedSelectorPopUpFactory
    {
        private SeedSelectorPopUpController m_Prefab;
        
        public SeedSelectorPopUpFactory(SeedSelectorPopUpController prefab)
        {
            m_Prefab = prefab;
        }

        public ISeedSelectorPopUpController Create()
        {
            var ins = GameObject.Instantiate(m_Prefab,cUIManager.Instance.transform);
            // ins.Init();
            return ins;
        }
    }
}
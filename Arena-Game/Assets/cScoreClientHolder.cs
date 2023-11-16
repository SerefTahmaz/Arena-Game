using System.Collections.Generic;
using DemoBlast.Utils;

namespace DefaultNamespace
{
    public class cScoreClientHolder : cSingleton<cScoreClientHolder>
    {
        private List<cClientScoreController> m_ClientScoreUnits = new List<cClientScoreController>();

        public List<cClientScoreController> ClientScoreUnits => m_ClientScoreUnits;
        public cClientScoreController ClientScoreUnit { get; set; }

        public void AddDead()
        {
            ClientScoreUnit.DeadCount.Value++;
        }
        public void AddKill()
        {
            ClientScoreUnit.KillCount.Value++;
        }
    }
}
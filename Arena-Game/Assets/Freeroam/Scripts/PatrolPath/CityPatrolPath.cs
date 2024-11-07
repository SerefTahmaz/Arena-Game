using System.Linq;
using ArenaGame.Utils;
using UnityEngine;

namespace DefaultNamespace
{
    public class CityPatrolPath : PatrolPath
    {
        public override Transform NextPoint(Transform target, Transform npc)
        { 
            var targets = m_Points;
            var suitableTargets = targets.Where((transform1 => target != transform1 ));
            return suitableTargets.RandomItem();
        }
    }
}
using System.Linq;
using UnityEngine;

namespace DefaultNamespace
{
    public class CityPatrolPath : PatrolPath
    {
        public override Transform NextPoint(Transform target, Transform npc)
        { 
            var targets = m_Points;
            var suitableTargets = targets.Where((transform1 => IsSuitable(transform1, npc, target)));
            return suitableTargets
                .OrderBy((transform1 => Vector3.Distance(transform1.position, npc.position)))
                .FirstOrDefault();
        }

        private bool IsSuitable(Transform transform1, Transform npc, Transform oldTarget)
        {
            var distance = Vector3.Distance(npc.position, transform1.position);
            var direction = transform1.position - npc.position;
            direction.Normalize();
            var dot = Vector3.Dot(direction, npc.forward);
            return dot > -.86f && oldTarget != transform1;
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DefaultNamespace
{
    public class PatrolPath : MonoBehaviour
    {
        [SerializeField] protected List<Transform> m_Points;
        
        private Transform StartPoint(Vector3 pos)
        {
            return m_Points.OrderBy((point => Vector3.Distance(pos, point.position))).FirstOrDefault();
        }

        public virtual Transform NextPoint(Transform target, Transform npc)
        {
            if (target == null)
            {
                return StartPoint(npc.position);
            }

            var index = m_Points.IndexOf(target);
            index++;
            
            return m_Points[index % m_Points.Count];
        }
    }
}
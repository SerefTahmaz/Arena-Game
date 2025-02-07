using System;
using System.Collections.Generic;
using System.Linq;
using ArenaGame.Utils;
using FluffyUnderware.Curvy;
using UnityEngine;

namespace DefaultNamespace
{
    public class SplinePatrolPath : MonoBehaviour
    {
        [SerializeField] private List<CurvySpline> m_CurvySplines;
        
        private List<CurvySplineSegment> m_CurvySplineSegments = new List<CurvySplineSegment>();

        public CurvySplineSegment GetClosestSegment(Vector3 startPos)
        {
            if (m_CurvySplineSegments.Count == 0)
            {
                PopulateSegments();
            }
            
            return m_CurvySplineSegments.OrderBy((segment => Vector3.Distance(startPos, segment.transform.position)))
                .FirstOrDefault();
        }

        private void PopulateSegments()
        {
            foreach (var VARIABLE in m_CurvySplines)
            {
                m_CurvySplineSegments.AddRange(VARIABLE.ControlPointsList);
            }
        }
    }

    [Serializable]
    public class SplineNavigator
    {
        private CurvySplineSegment m_CurrentTarget;
        private CurvySplineSegment m_PreviousTarget;
        
        public SplinePatrolPath SplinePatrolPath { get; }
        public float StopDist {get;}
        public CurvySplineSegment CurrentTarget => m_CurrentTarget;
        public Vector3 CurrentTargetPos => CurrentTarget.transform.position;

        public SplineNavigator(SplinePatrolPath splinePatrolPath, Vector3 startPos, float stopDist)
        {
            SplinePatrolPath = splinePatrolPath;
            StopDist = stopDist;
            m_CurrentTarget = splinePatrolPath.GetClosestSegment(startPos);
        }

        public virtual void PickNextPoint()
        {
            // var splnei = new CurvySpline();
            // splnei.get
            var avaiablePoint = new List<CurvySplineSegment>()
            {
                CurrentTarget.Spline.GetNextControlPoint(CurrentTarget),
                CurrentTarget.Spline.GetPreviousControlPoint(CurrentTarget)
            };

            if (CurrentTarget.Connection)
            {
                foreach (var VARIABLE in CurrentTarget.Connection.ControlPointsList.Where((segment => segment!= CurrentTarget)))
                {
                    avaiablePoint.AddRange((new []
                    {
                        VARIABLE.Spline.GetNextControlPoint(VARIABLE),
                        VARIABLE.Spline.GetPreviousControlPoint(VARIABLE)
                    }));
                }
            }
            
            for (int i = 0; i < avaiablePoint.Count; i++)
            {
                if (avaiablePoint[i]&&avaiablePoint[i].Connection && avaiablePoint[i].Connection.ControlPointsList.Contains(m_PreviousTarget))
                {
                    avaiablePoint[i] = null;
                }
            }
            
            avaiablePoint.RemoveAll((segment => segment == null));
            avaiablePoint.Remove(m_PreviousTarget);
            
            m_PreviousTarget = CurrentTarget;
            m_CurrentTarget =  avaiablePoint.RandomItem();
        }

        public bool IsTargetReached(Vector3 pos)
        {
            return Vector3.Distance(pos, CurrentTarget.transform.position) < StopDist;
        }
    }
}
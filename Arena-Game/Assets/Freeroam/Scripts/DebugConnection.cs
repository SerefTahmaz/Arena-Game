using System;
using System.Collections.Generic;
using System.Linq;
using FluffyUnderware.Curvy;
using UnityEngine;

namespace Freeroam.Scripts
{
    public class DebugConnection : MonoBehaviour
    {
        public List<CurvySplineSegment> options;

        private void Update()
        {
            var CurrentTarget = GetComponent<CurvySplineSegment>();
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

            options = avaiablePoint;
        }
    }
}
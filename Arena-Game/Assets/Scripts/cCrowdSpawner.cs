using System;
using System.Collections;
using System.Collections.Generic;
using DemoBlast.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class cCrowdSpawner : MonoBehaviour
{
    [SerializeField] private Vector3 m_Interavals;
    [SerializeField] private Vector3 m_Max;
    [SerializeField] private float m_BendAmount;
    [SerializeField] private float m_BendFrequncy;
    [SerializeField] private float m_BendOffset;
    [SerializeField] private Vector3 m_Offset;
    [Range(0, 1)] [SerializeField] private float m_Random;
    [SerializeField] private List<GameObject> m_Model;
    [SerializeField] private bool m_Spawn;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (m_Spawn)
        {
            foreach (var VARIABLE in transform.gameObject.GetChilds())
            {
                EditorApplication.delayCall += () => { DestroyImmediate(VARIABLE.gameObject); };
            }

            int widthCount = (int)((m_Max.x / m_Interavals.x) / 2);

            int heightCount = (int)((m_Max.y / m_Interavals.y) / 2);

            for (int j = -heightCount; j < heightCount; j++)
            {
                var posH = (-Vector3.forward * j * m_Interavals.z + Vector3.up * j * m_Interavals.y);
                for (int i = -widthCount; i < widthCount; i++)
                {
                    var pos = posH + Vector3.right * i * m_Interavals.x + Vector3.forward *
                        Mathf.Sin(m_BendFrequncy * i * m_Interavals.x + m_BendOffset) * m_BendAmount;
                    pos = transform.TransformPoint(pos + m_Offset);
                    if (m_Random >= Random.value)
                    {
                        Quaternion lookRot = Quaternion.LookRotation(-pos);
                        if (Application.isPlaying)
                        {
                            var ins = Instantiate(m_Model.RandomItem(), pos, lookRot, transform);
                        }
                        else
                        {
                            var ins = PrefabUtility.InstantiatePrefab(m_Model.RandomItem(),transform) as GameObject;
                            ins.transform.position = pos;
                            ins.transform.rotation = lookRot;
                        }
                    }
                }
            }
        }
    }
#endif
    
}
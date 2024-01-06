using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

public class cScatter : MonoBehaviour
{
    [SerializeField] private List<GameObject> m_Trees;
    [SerializeField] private Texture2D m_SplatMap;
    [SerializeField] private int m_Xsize;
    [SerializeField] private int m_Zsize;
    [SerializeField] private Transform m_StartPos;
    [SerializeField] private MeshCollider m_Collider;
    [SerializeField] private Renderer m_Renderer;
    [SerializeField] private float m_Scale;
    [SerializeField] private float m_Threshold;
    [SerializeField] private bool m_Spawn;
    [SerializeField][Range(0,1)] private float m_Random;
    [SerializeField] private float m_RotationRandomScale;
    [SerializeField] private float m_RandomPositionScale;
    [SerializeField] private Vector2 m_RandomScaleRange;
    [SerializeField] private bool m_Instance;


    private GameObject m_Parent;
    [ContextMenu("Scatter")]
    public void ScatterTree()
    {
        if(m_Parent !=null) DestroyImmediate(m_Parent);
        m_Parent = new GameObject("Parent");
        for (int i = 0; i < m_Xsize*m_Scale; i++)
        {
            for (int j = 0; j < m_Zsize*m_Scale; j++)
            {
                Vector3 pos = m_StartPos.position + Vector3.up*500;
                pos += m_StartPos.right*i/m_Scale;
                pos += m_StartPos.forward*j/m_Scale;
                
                Ray ray = new Ray(pos, Vector3.down);
                RaycastHit hit;
                if (!m_Collider.Raycast(ray, out hit, 1000))
                    continue;
                
                Vector2 pixelUV = hit.textureCoord;
                pixelUV.x *= m_SplatMap.width;
                pixelUV.y *= m_SplatMap.height;

                var col =m_SplatMap.GetPixel((int)pixelUV.x, (int)pixelUV.y);

                var value = 1 - col.g - col.b - col.a;

                if (value > m_Threshold)
                {
                    Debug.Log(value);
                    if (m_Spawn && (Random.Range(0.0f, 1.0f) > m_Random))
                    {
                        GameObject tree = m_Trees.OrderBy((o => Random.Range(0, 1000))).FirstOrDefault();
                        var ins = m_Instance ? Instantiate(tree) : PrefabUtility.InstantiatePrefab(tree) as GameObject;
                        ins.transform.position = hit.point;
                        ins.transform.parent = m_Parent.transform;
                        var rot = ins.transform.eulerAngles;
                        rot += (Random.insideUnitSphere) * m_RotationRandomScale;
                        rot += Vector3.up * Random.Range(0,360);
                        ins.transform.eulerAngles = rot;
                        var insPos = ins.transform.position;

                        ray.origin += new Vector3(Random.Range(-m_RandomPositionScale, m_RandomPositionScale), 0, Random.Range(-m_RandomPositionScale, m_RandomPositionScale));

                        if (m_Collider.Raycast(ray, out hit, 1000))
                        {
                            ins.transform.position = hit.point;
                        }
                        ins.transform.localScale *=  Random.Range(m_RandomScaleRange.x, m_RandomScaleRange.y); 
                    }
                }
               
            }
        }
       
    }
}
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class StampToGround : MonoBehaviour
{
    [SerializeField] private List<Transform> m_Objects;

    public void Stamp()
    {
#if UNITY_EDITOR
        foreach (var VARIABLE in m_Objects)
        {
            if (Physics.Raycast(VARIABLE.position - Vector3.down, Vector3.down, out var hit))
            {
                var pos = VARIABLE.position;
                pos.y = hit.point.y;
                VARIABLE.position = pos;
                EditorUtility.SetDirty(VARIABLE);
            }
        }
#endif
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(StampToGround))]
public class StampToGroundEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Click"))
        {
            (target as StampToGround).Stamp();
        }
    }
}
#endif
using System.Collections;
using System.Collections.Generic;
using ArenaGame;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class EnemyCardController : MonoBehaviour
{
    [SerializeField] private ProfileCardController m_ProfileCardController;
    [SerializeField] private Transform m_AnimHolder;
    [SerializeField] private Transform m_AnimHolderStartPos;
    [SerializeField] private RawImage m_FirstOne;
    [SerializeField] private RawImage m_SecondOne;
    [SerializeField] private RawImage m_TargetOne;
    
    [SerializeField] private float m_Duration;
    [SerializeField] private float m_Delay;
    
    // Start is called before the first frame update
    void Start()
    {
        // PlaySearchingAnim();
    }

    public async UniTask PlaySearchingAnim()
    {
        m_ProfileCardController.SetSearchingProfile();

        m_FirstOne.texture = ProfileGenerator.GetRandomProfile().ProfilePicture;
        m_SecondOne.texture = ProfileGenerator.GetRandomProfile().ProfilePicture;

        var selectedProfile = ProfileGenerator.GetRandomProfile();
        m_TargetOne.texture = selectedProfile.ProfilePicture;
        
        m_AnimHolder.localPosition = m_AnimHolderStartPos.localPosition;

        await UniTask.WaitForSeconds(m_Delay);
        
        await m_AnimHolder.DOLocalMove(Vector3.zero, m_Duration);
        m_ProfileCardController.SetProfile(selectedProfile);
        
    }
}



#if UNITY_EDITOR
[CustomEditor(typeof(EnemyCardController))]
public class EnemyCardControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Click"))
        {
            (target as EnemyCardController).PlaySearchingAnim();
        }
    }
}
#endif
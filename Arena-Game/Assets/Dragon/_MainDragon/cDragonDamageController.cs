using System;
using System.Collections;
using System.Collections.Generic;
using DemoBlast.Utils;
using DG.Tweening;
using RootMotion.FinalIK;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Animations;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class cDragonDamageController : NetworkBehaviour, IDamagable
{

    [SerializeField] private float m_Strength =0.0002f;
    [SerializeField] private float m_InvincibleDuration =0.5f;
    [SerializeField] private float m_Duration =0.5f;
    [SerializeField] private LimbIK m_LimbIK;
    [SerializeField] private AnimationCurve m_AnimationCurve;
    [SerializeField] private AudioSource m_AudioSource;
    [SerializeField] private AudioClip m_DamageClip;
    [SerializeField] private ParticleSystem m_DamageParticle;
    [SerializeField] private cCharacter m_Dragon;
    [SerializeField] private Transform m_FocusTransform;

    public int TeamID => 10;
    public Transform FocusPoint => m_FocusTransform;

    private bool m_Punching = false;
    private Tween m_Tween;
    private Tween m_LimbTween;
    private Transform m_Target;
    private Transform m_FootPos;

    private void Awake()
    {
        m_FootPos = GetComponent<ParentConstraint>().GetSource(0).sourceTransform;
        m_Target = (new GameObject("Target")).transform;
        m_Target.SetParent(null);
        m_LimbIK.solver.target = m_Target;
    }


    [ContextMenu("Punch")]
    public void DamageLeg(Vector3 pos)
    {
        m_Dragon.HealthBar.OnDamage(10);
        TakeDamageServerRpc(pos);
    }
    
    

    [ServerRpc(RequireOwnership = false)]
    public void TakeDamageServerRpc(Vector3 pos)
    {
        TakeDamageClientRpc(pos);
    }

    [ClientRpc]
    public void TakeDamageClientRpc(Vector3 pos)
    {
        DamageAnim(pos);
    }

    public void DamageAnim(Vector3 pos)
    {
        m_Target.DOComplete();
        m_Tween.Complete();
        m_LimbTween.Complete();
        m_LimbTween=DOVirtual.Float(0, 1, m_Duration, value =>
        {
            m_LimbIK.solver.IKPositionWeight = m_AnimationCurve.Evaluate(value);
            // m_LimbIK.solver.IKRotationWeight= m_AnimationCurve.Evaluate(value);
        });

        m_Target.position = m_FootPos.position;
        
        m_Target.DOPunchPosition(Vector3.one * m_Strength, m_Duration).SetUpdate(UpdateType.Late).OnComplete((() =>
        {
            m_LimbIK.solver.IKPositionWeight =0;
            m_LimbIK.solver.IKRotationWeight =0;
        }));
        DOVirtual.DelayedCall(m_InvincibleDuration, () => m_Punching = false);
        m_Punching = true;
        
        m_AudioSource.PlayOneShot(m_DamageClip);
        

        var ins =Instantiate(m_DamageParticle, pos, Quaternion.identity);
        ins.PlayWithClear();

    }

    public void Damage(int amount, Vector3 pos, bool isHeavy)
    {
        if(m_Dragon.CharacterNetworkController.IsOwner == false) return;
        if(m_Punching == false) DamageLeg(pos);
    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(cDragonDamageController)), CanEditMultipleObjects]
public class cDragonDamageControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var script = target as cDragonDamageController;
        
        if (GUILayout.Button("DamageLeg"))
        {
            script.DamageLeg(Vector3.zero);
        }
        
    }
}
#endif

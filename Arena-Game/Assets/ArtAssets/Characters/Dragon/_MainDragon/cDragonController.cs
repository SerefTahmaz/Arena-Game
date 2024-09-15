using System;
using System.Collections;
using System.Collections.Generic;
using ArenaGame.Utils;
using DG.Tweening;
using RootMotion.FinalIK;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class cDragonController : MonoBehaviour
{
    [SerializeField] private ParticleSystem m_Flames;
    [SerializeField] private ParticleSystem m_GroundFlame;
    [SerializeField] private ParticleSystem m_FlameThrowerFlames;
    [SerializeField] private cDragonCharacter m_Character;
    [SerializeField] private ParticleSystem m_ImpactDust;
    [SerializeField] private ParticleSystem m_WingDust;
    [SerializeField] private ParticleSystem m_ShoutDust;

    private LookAtIK m_LookAtIK => m_Character.HeadLookAtIk;
    
    public Action m_ActionEnd = delegate {  };
    
    public void SnapToIdle()
    {
        DOVirtual.Float(transform.localPosition.y, 0, .1f,
            value => transform.localPosition =
                new Vector3(transform.localPosition.x, value, transform.localPosition.z));
    }
    
    
    public void OnAnimEnd()
    {
    }
    
    public void OnActionEnd()
    {
        m_ActionEnd.Invoke();
        Debug.Log("ANIM ENDED!!");
    }
    
    public void Breahting()
    {
        m_Flames.Clear(true);
        m_Flames.Play();
    }
    
    public void StopBreahting()
    {
        m_Flames.Stop();
    }
    
    public void FlameThrowerBreahting()
    {
        m_FlameThrowerFlames.Clear(true);
        m_FlameThrowerFlames.Play();
    }
    
    public void FlameThrowerStopBreahting()
    {
        m_FlameThrowerFlames.Stop();
    }
    
    public void GroundBreahting()
    {
        m_GroundFlame.Clear(true);
        m_GroundFlame.Play();
    }
    
    public void StopGroundBreahting()
    {
        m_GroundFlame.Stop();
    }

    public void EnableIK()
    {
        m_IKEnable++;
    }
    
    public void DisableIK()
    {
        m_IKEnable--;
    }

    private int m_IKEnable = 0;
    private void Update()
    {
        if (m_IKEnable > 0)
        {
            m_LookAtIK.solver.IKPositionWeight=Mathf.Lerp(m_LookAtIK.solver.IKPositionWeight, 1, Time.deltaTime*5);
        }
        else
        {
            m_LookAtIK.solver.IKPositionWeight=Mathf.Lerp(m_LookAtIK.solver.IKPositionWeight, 0, Time.deltaTime*5);
        }
    }

    public void ShakeCam(AnimationEvent animationEvent)
    {
        var camShakeParameter = animationEvent.objectReferenceParameter as cCameraShakeSO;
        FindObjectOfType<cCamShake>().ShakeCamera(5 * camShakeParameter.m_Intensity,
            8 * camShakeParameter.m_Frequency,
            .5f * camShakeParameter.m_Duration);
    }

    public void PlayImpactDust()
    {
        m_ImpactDust.PlayWithClear();
    }
    
    public void PlayWingDust()
    {
        m_WingDust.Play();
    }
    
    public void PlayShoutDust()
    {
        m_ShoutDust.Play();
    }
}

// [CustomEditor(typeof(cDragonController))]
// public class cDragonControllerEditor : Editor
// {
//     public override void OnInspectorGUI()
//     {
//         base.OnInspectorGUI();
//
//         var script = target as cDragonController;
//         
//         if (GUILayout.Button("LeftTurn"))
//         {
//             script.LeftTurn();
//         }
//         
//         if (GUILayout.Button("RightTurn"))
//         {
//             script.RightTurn();
//         }
//     }
// }

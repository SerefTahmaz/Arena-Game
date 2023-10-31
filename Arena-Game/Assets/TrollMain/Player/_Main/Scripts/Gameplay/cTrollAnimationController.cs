using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
// using DG.Tweening;
using UnityEngine;
using Object = System.Object;
using Random = UnityEngine.Random;

public class cTrollAnimationController : MonoBehaviour
{
    private Animator _animator;
    private Dictionary<TrollAnimationState, string> animatorStates = new Dictionary<TrollAnimationState, string>();
    
    public Action m_OnAttackEnd= delegate {  };

    [SerializeField] private AnimationCurve m_EyeballCurve;

    private void Awake()
    {
        foreach (TrollAnimationState VARIABLE in Enum.GetValues(typeof(TrollAnimationState)))
        {
            animatorStates.Add(VARIABLE, VARIABLE.ToString());
        }
        
        _animator = GetComponentInChildren<Animator>();
        
        BlinkEye();
        EyeBallMovement();
    }
    
    public void EyeBallMovement()
    {
        DOVirtual.DelayedCall(Random.Range(1.0f, 5.0f), () =>
        {
            DOVirtual.Float(0,  m_EyeballCurve.Evaluate(Random.Range(-1.0f, 1.0f)), .3f, value =>
            {
                _animator.SetFloat("EyeballHorizontal", value);
            });
        
            DOVirtual.Float(0, m_EyeballCurve.Evaluate(Random.Range(-1.0f, 1.0f)), .3f, value =>
            {
                _animator.SetFloat("EyeballVertical", value);
            });
        
            EyeBallMovement();
        });
    }

    public void BlinkEye()
    {
        DOVirtual.DelayedCall(Random.Range(0f, 10.0f), () =>
        {
            DOVirtual.Float(0, 1, .2f, value =>
            {
                _animator.SetFloat("Eyelid", value);
            }).SetLoops(2, LoopType.Yoyo);
            BlinkEye();
        });
    }

    public void SetTrigger(TrollAnimationState state, float? value = null)
    {
        if (value.HasValue)
        {
            _animator.SetFloat(animatorStates[state], value.Value);
        }
        else
        {
            _animator.SetTrigger(animatorStates[state]);
        }
    }
    
    public void ResetTrigger(TrollAnimationState state)
    {
        _animator.ResetTrigger(animatorStates[state]);
    }

    [SerializeField] private SwordFlameHelper m_LeftSword;
    [SerializeField] private SwordFlameHelper m_RightSword;

    [Serializable]
    private class SwordFlameHelper
    {
        public GameObject m_BackWeapon;
        public GameObject m_HandWeapon;
        public Renderer handWeaponRenderer => m_HandWeapon.GetComponent<Renderer>();
        public Material m_NormalMat;
        public Material m_BurntMat;
        public ParticleSystem m_FlameParticle;
    }

    public void SwitchLeftWeapon()
    {
        m_LeftSword.m_BackWeapon.SetActive(!m_LeftSword.m_BackWeapon.activeSelf);
        m_LeftSword.m_HandWeapon.SetActive(!m_LeftSword.m_HandWeapon.activeSelf);
        SetLeftFlame(0);
    }
    
    public void SwitchRightWeapon()
    {
        m_RightSword.m_BackWeapon.SetActive(!m_RightSword.m_BackWeapon.activeSelf);
        m_RightSword.m_HandWeapon.SetActive(!m_RightSword.m_HandWeapon.activeSelf);
        SetRightFlame(0);
    }
    
   
    
    public void SetLeftFlame(int state)
    {
        SetFlame(m_LeftSword, state == 1 ? true : false);
    }
    
    
    public void SetRightFlame(int state)
    {
        SetFlame(m_RightSword, state == 1 ? true : false);
    }

    private void SetFlame(SwordFlameHelper swordFlameHelper, bool state)
    {
        if (state)
        {
            swordFlameHelper.m_FlameParticle.gameObject.SetActive(true);
            swordFlameHelper.m_FlameParticle.Play();
            swordFlameHelper.handWeaponRenderer.sharedMaterial = swordFlameHelper.m_BurntMat;
        }
        else
        {
            swordFlameHelper.m_FlameParticle.gameObject.SetActive(false);
            swordFlameHelper.m_FlameParticle.Stop();
            swordFlameHelper.handWeaponRenderer.sharedMaterial = swordFlameHelper.m_NormalMat;
        }
    }

    public void CamShake()
    {
        // cCamShake.Instance.ShakeCamera(1.5f,1.5f,.5f);
    }

    [SerializeField] private List<ParticleSystem> m_WildFire;

    public void PlayFireChargeAttackParticle(int i)
    {
        // m_WildFire[i].PlayWithClear();
        // m_WildFire[i].transform.ResetTransform();
        // m_WildFire[i].transform.DOLocalMove(Vector3.forward * 2, 1).OnComplete((() =>
        // {
        //     m_WildFire[i].Stop();
        // }));
    }

    [Flags]
    public enum TrollAnimationState
    {
        Idle=0,
        Walk=1,
        Empty=2,
        Taunt=4,
        Attack1=8,
        Attack2=16,
        Attack3=32,
        Attack4=64,
        Dead=128
    }
}

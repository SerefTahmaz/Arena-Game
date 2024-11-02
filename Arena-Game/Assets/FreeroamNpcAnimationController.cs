using System;
using System.Collections;
using System.Collections.Generic;
using ArenaGame.Utils;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Main.Scripts.Gameplay
{
    public class FreeroamNpcAnimationController : MonoBehaviour
    {
        private Animator _animator;
        private Dictionary<AnimationState, string> animatorStates = new Dictionary<AnimationState, string>();
        private Dictionary<string, Action> animatorFinishEvents = new Dictionary<string, Action>();
        private Dictionary<string, Action> animatorStartEvents = new Dictionary<string, Action>();

        public AnimationState CurrentAnimation = AnimationState.Idle;
        
        public Action m_OnAttackEnd= delegate {  };

        public Dictionary<string, Action> AnimatorFinishEvents => animatorFinishEvents;
        public Dictionary<string, Action> AnimatorStartEvents => animatorStartEvents;

        public SwordFlameHelper LeftSword => m_LeftSword;

        public SwordFlameHelper RightSword => m_RightSword;

        [SerializeField] private AnimationCurve m_EyeballCurve;

        private void Awake()
        {
            animatorStates.Add(AnimationState.Idle, "Idle");
            animatorStates.Add(AnimationState.FightIdle, "FightIdle");
            animatorStates.Add(AnimationState.Slash, "Slash");
            animatorStates.Add(AnimationState.Empty, "Empty");
            animatorStates.Add(AnimationState.Jump, "Jump");
            animatorStates.Add(AnimationState.Block, "Block");
            animatorStates.Add(AnimationState.TakeDamage, "TakeDamage");
            animatorStates.Add(AnimationState.Combo, "Combo");
            animatorStates.Add(AnimationState.Dodge, "Dodge");
            animatorStates.Add(AnimationState.DrawRightSword, "DrawRightSword");
            animatorStates.Add(AnimationState.SheathRightSword, "SheathRightSword");
            animatorStates.Add(AnimationState.DrawLeftSword, "DrawLeftSword");
            animatorStates.Add(AnimationState.SheathLeftSword, "SheathLeftSword");
            animatorStates.Add(AnimationState.ChargeLeft, "ChargeLeft");
            animatorStates.Add(AnimationState.ChargeRight, "ChargeRight");
            animatorStates.Add(AnimationState.HeavySlash, "HeavySlash");
            animatorStates.Add(AnimationState.LeftSlash, "LeftSlash");
            animatorStates.Add(AnimationState.ChargeBoth, "ChargeBoth");
            animatorStates.Add(AnimationState.HelloEverybody, "HelloEverybody");
            animatorStates.Add(AnimationState.Stretching, "Stretching");
            animatorStates.Add(AnimationState.BackImpact, "BackImpact");
            animatorStates.Add(AnimationState.Damage, "Damage");
            animatorStates.Add(AnimationState.DamageAnimIndex, "DamageAnimIndex");
            animatorStates.Add(AnimationState.Dead, "Dead");

            foreach (var VARIABLE in animatorStates)
            {
                animatorFinishEvents.Add(VARIABLE.Value, new Action(delegate {  }));
            }
            foreach (var VARIABLE in animatorStates)
            {
                animatorStartEvents.Add(VARIABLE.Value, new Action(delegate {  }));
            }
            animatorStartEvents.Add("InputAvailable", new Action(delegate {  }));
            animatorFinishEvents.Add("InputAvailable", new Action(delegate {  }));
        
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
                    if (_animator == null)
                    {
                        return;    
                    }
                    _animator.SetFloat("EyeballHorizontal", value);
                });

                DOVirtual.Float(0, m_EyeballCurve.Evaluate(Random.Range(-1.0f, 1.0f)), .3f, value =>
                {
                    if (_animator == null)
                    {
                        return;    
                    }
                    _animator.SetFloat("EyeballVertical", value);
                });

                if (_animator == null)
                {
                    return;    
                }
                EyeBallMovement();
            });
        }

        public void BlinkEye()
        {
            DOVirtual.DelayedCall(Random.Range(0f, 10.0f), () =>
            {
                DOVirtual.Float(0, 1, .2f, value =>
                {
                    if (_animator == null)
                    {
                        return;    
                    }
                    _animator.SetFloat("Eyelid", value);
                }).SetLoops(2, LoopType.Yoyo);
                if (_animator == null)
                {
                    return;    
                }
                BlinkEye();
            });
        }

        public void SetTrigger(AnimationState state, float? value = null, bool resetable = false)
        {
            if (value.HasValue)
            {
                _animator.SetFloat(animatorStates[state], value.Value);
            }
            else
            {
                _animator.SetTrigger(animatorStates[state]);
            
                if (resetable)
                {
                    StartCoroutine(FrameDelay());
                    IEnumerator FrameDelay()
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            yield return null;
                        }
                        ResetTrigger(state);
                    }
                }
            }
        }
    
        public void ResetTrigger(AnimationState state)
        {
            _animator.ResetTrigger(animatorStates[state]);
        }

        public void AnimationHelperFinishInvoke(string animationName)
        {
            animatorFinishEvents[animationName].Invoke();
        }
    
        public void AnimationHelperStartInvoke(string animationName)
        {
            animatorStartEvents[animationName].Invoke();
        }

        [SerializeField] private SwordFlameHelper m_LeftSword;
        [SerializeField] private SwordFlameHelper m_RightSword;

        [Serializable]
        public class SwordFlameHelper
        {
            public GameObject m_BackWeapon;
            public GameObject m_HandWeapon;
            public Renderer handWeaponRenderer => m_HandWeapon.GetComponent<Renderer>();
            public Material m_NormalMat;
            public Material m_BurntMat;
            public ParticleSystem m_FlameParticle;
        }
    
   
    
        

       

        public void CamShake()
        {
            // cCamShake.Instance.ShakeCamera(1.5f,1.5f,.5f);
        }

        [SerializeField] private List<ParticleSystem> m_WildFire;

        public void PlayFireChargeAttackParticle(int i)
        {
            m_WildFire[i].PlayWithClear();
            m_WildFire[i].transform.ResetTransform();
            m_WildFire[i].transform.DOLocalMove(Vector3.forward * 2, 1).OnComplete((() =>
            {
                m_WildFire[i].Stop();
            }));
        }

        public enum AnimationState
        {
            Idle,
            FightIdle,
            Slash,
            DrawRightSword,
            SheathRightSword,
            Empty,
            Jump,
            Block,
            TakeDamage,
            Combo,
            Dodge,
            DrawLeftSword,
            SheathLeftSword,
            ChargeLeft,
            ChargeRight,
            HeavySlash,
            LeftSlash,
            ChargeBoth,
            HelloEverybody,
            Stretching,
            BackImpact,
            Damage,
            DamageAnimIndex,
            Dead
        }
    }
}

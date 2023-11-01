using System;
using System.Linq;
using DemoBlast.Utils;
using DG.Tweening;
using PlayerCharacter;
using RootMotion.FinalIK;
using UnityEngine;

namespace FiniteStateMachine
{
    public class cTrollStateMachine : cStateMachine
    {
        #region PritaveFields
        
        [SerializeField] private cTrollAnimationController.TrollAnimationState m_AvailableAttacks;
        [SerializeField] private Vector2 m_CooldownDurationRange;
        [SerializeField] private cTrollCharacter m_TrollCharacter;
        [SerializeField] private ParticleSystem m_BloodExpo;

        public Transform Target => FindObjectsOfType<cPlayerStateMachineV2>().OrderBy((v2 => Vector3.Distance(transform.position,v2.transform.position))).FirstOrDefault()?.transform;

        public cTrollAnimationController.TrollAnimationState AvailableAttacks => m_AvailableAttacks;

        public Vector2 CooldownDurationRange => m_CooldownDurationRange;

        public bool IsHost => TrollCharacter.IsHost;

        public cTrollCharacter TrollCharacter => m_TrollCharacter;

        public cStateBase m_Idle;
        public cStateBase m_Walk;
        public cStateBase m_Empty;
        public cStateBase m_Death;

        #endregion

        #region States

        #endregion

        #region Properties
        
        public override int TeamID => m_TrollCharacter.TeamID;

        #endregion

        protected override void Start()
        {
            // if(IsHost == false) return;
            
            m_Idle.InitializeState("Idle", this);
            m_Walk.InitializeState("Walk", this);
            m_Empty.InitializeState("Empty", this);
            m_Death.InitializeState("Death", this);

            TrollCharacter.HealthBar.m_OnDied += () =>
            {
            ChangeState(m_Death);
            };
            
            TrollCharacter.TrollNetworkController.OnStartFightServerRpc();
            
            base.Start();
        }

        protected override void Update()
        {
            // if(IsHost == false) return;
            base.Update();
        }
        
        protected override void FixedUpdate()
        {
            // if(IsHost == false) return;
            base.FixedUpdate();
        }

        protected override cStateBase GetInitialState()
        {
            return m_Idle;
        }
        
        private bool m_Damaged = false;

        public override void Damage(int amount, Vector3 pos,bool isHeavyDamage)
        {
            // if(!m_TrollCharacter.CharacterNetworkController.IsOwner) return;
            
            if(CurrentState == m_Death) return; 
            
            base.Damage(amount, pos,isHeavyDamage);

            if (m_Damaged == false)
            {
                TrollCharacter.HealthBar.OnDamage(10);
                TrollCharacter.CharacterNetworkController.TakeDamageServerRpc(pos);
                TrollCharacter.AnimationController.SetTrigger(cTrollAnimationController.TrollAnimationState.Damage);
                
                m_Damaged = true;
                DOVirtual.DelayedCall(.2f, () => m_Damaged = false);
            }
        }

        // private void OnTriggerEnter(Collider other)
        // {
        //     if (other.transform.TryGetComponent(out IDamageable damageable))
        //     {
        //         damageable.Damage();
        //     }
        // }
        public void OnDamageAnim(Vector3 pos)
        {
            m_BloodExpo.transform.position = pos;
            m_BloodExpo.PlayWithClear();
            // m_DustExpo.PlayWithClear();
        }
    }
    
    
}
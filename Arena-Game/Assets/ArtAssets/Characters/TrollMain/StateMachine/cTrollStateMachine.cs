using System;
using System.Linq;
using ArenaGame.Utils;
using DG.Tweening;
using PlayerCharacter;
using RootMotion.FinalIK;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FiniteStateMachine
{ 
    public class cTrollStateMachine : cCharacterStateMachine
    {
        #region PritaveFields
         
        [SerializeField] private cTrollAnimationController.TrollAnimationState m_AvailableAttacks;
        [SerializeField] private Vector2 m_CooldownDurationRange;
        [SerializeField] private cTrollCharacter m_TrollCharacter;
        [SerializeField] private ParticleSystem m_BloodExpo;
        [SerializeField] private string m_CharacterName;

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
            if(IsHost == false) return;
            
            m_Idle.InitializeState("Idle", this);
            m_Walk.InitializeState("Walk", this);
            m_Empty.InitializeState("Empty", this);
            m_Death.InitializeState("Death", this);

            TrollCharacter.OnDamage += OnDamage;
            
            TrollCharacter.HealthManager.m_OnDied += () =>
            {
                ChangeState(m_Death);
            };

            TrollCharacter.TrollNetworkController.PlayerName.Value = m_CharacterName;
            
            TrollCharacter.TrollNetworkController.OnStartFightServerRpc();
            
            TrollCharacter.MovementController.Init();
            
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

        private DamageWrapper m_LastDamager;

        public override void OnDamage(DamageWrapper damageWrapper)
        {
            Debug.Log("Damagingg trol!!!!!");
            
            if(CurrentState == m_Death) return;
            
            base.OnDamage(damageWrapper);
            TrollCharacter.HealthManager.OnDamage(damageWrapper.amount);
            m_LastDamager = damageWrapper;
            TrollCharacter.CharacterNetworkController.TakeDamageServerRpc(damageWrapper.pos);
            TrollCharacter.AnimationController.SetTrigger(cTrollAnimationController.TrollAnimationState.Damage);
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
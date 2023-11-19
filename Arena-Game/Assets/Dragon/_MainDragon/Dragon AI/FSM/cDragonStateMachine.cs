using System;
using System.Linq;
using DG.Tweening;
using RootMotion.FinalIK;
using UnityEngine;

namespace FiniteStateMachine
{
    public class cDragonStateMachine : cCharacterStateMachine
    {
        #region PritaveFields
        
        [SerializeField] private cAnimationController.eAttackType m_AvailableAttacks;
        [SerializeField] private Vector2 m_CooldownDurationRange;
        [SerializeField] private cDragonCharacter m_DragonCharacter;

        private bool m_ZKeyPressed=false;
        private bool m_XKeyPressed = false;
        private bool m_VKeyPressed = false;

        private LookAtIK m_LookAtIK => Character.HeadLookAtIk;

        public cDragonCharacter Character => DragonCharacter;

        public LookAtIK LookAtIK => m_LookAtIK;

        public cAnimationController.eAttackType AvailableAttacks => m_AvailableAttacks;

        public Vector2 CooldownDurationRange => m_CooldownDurationRange;

        public bool IsHost => Character.IsHost;

        public bool ZKeyPressed
        {
            get => m_ZKeyPressed;
            set => m_ZKeyPressed = value;
        }

        public bool XKeyPressed
        {
            get => m_XKeyPressed;
            set => m_XKeyPressed = value;
        }

        public bool VKeyPressed
        {
            get => m_VKeyPressed;
            set => m_VKeyPressed = value;
        }

        public cDragonCharacter DragonCharacter => m_DragonCharacter;

        public cStateBase m_DragonSleep;
        public cStateBase m_DragonIdle;
        public cStateBase m_DragonWalk;
        public cStateBase m_DragonEmpty;
        public cStateBase m_DragonFly;
        public cStateBase m_DragonDeath;

        #endregion

        #region States

        #endregion

        #region Properties

        #endregion

        public override int TeamID => Character.TeamID;

        protected override void Start()
        {
            if(IsHost == false) return;
            
            m_DragonSleep.InitializeState("DragonSleep", this);
            m_DragonIdle.InitializeState("DragonIdle", this);
            m_DragonWalk.InitializeState("DragonWalk", this);
            m_DragonEmpty.InitializeState("DragonEmpty", this);
            m_DragonFly.InitializeState("DragonFly", this);
            m_DragonDeath.InitializeState("DragonDeath", this);

            Character.HealthManager.m_OnDied += () =>
            {
                ChangeState(m_DragonDeath);
            };

            Character.HeadLookAtIk.solver.target = Target();
            
            base.Start();
        }

        protected override void Update()
        {
            if(IsHost == false) return;
            base.Update();
            
            
            if (Input.GetKeyDown(KeyCode.Z))
            {
                ZKeyPressed = true;
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                XKeyPressed = true;
            }
            if (Input.GetKeyDown(KeyCode.V))
            {
                m_VKeyPressed = true;
            }
        }
        
        protected override void FixedUpdate()
        {
            if(IsHost == false) return;
            base.FixedUpdate();
        }

        protected override cStateBase GetInitialState()
        {
            return m_DragonSleep;
        }

        public override void OnDamage(DamageWrapper damageWrapper)
        {
            if(m_DragonCharacter.CharacterNetworkController.IsOwner == false) return;
            base.OnDamage(damageWrapper);
            m_DragonCharacter.HealthManager.OnDamage(10);
        }

        // private void OnTriggerEnter(Collider other)
        // {
        //     if (other.transform.TryGetComponent(out IDamageable damageable))
        //     {
        //         damageable.Damage();
        //     }
        // }

    }
    
    
}
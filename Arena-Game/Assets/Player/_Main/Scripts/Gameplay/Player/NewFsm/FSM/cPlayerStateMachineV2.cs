using System;
using DemoBlast.Utils;
using DG.Tweening;
using FiniteStateMachine;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class cPlayerStateMachineV2 : cCharacterStateMachine
    {
        #region PritaveFields

        [SerializeField] private ParticleSystem m_DustExpo;
        [SerializeField] private ParticleSystem m_BloodExpo;
        [SerializeField] private cPlayerCharacter m_Character;
        [SerializeField] private cStatesBlackBoard m_BlackBoard;

        // #region Properties

        public cStateBase Empty => m_BlackBoard.m_Empty;

        public cStateBase FreeRoam => m_BlackBoard.m_FreeRoam;

        public cStateBase Fight => m_BlackBoard.m_Fight;

        public cStateBase Dead => m_BlackBoard.m_Dead;

        private MovementUserController m_MovementUserController;
        private IInputManager m_InputManager;

        [SerializeField] private float m_YDistance;
        [SerializeField] private LayerMask m_LayerMask;

        #endregion

        #region States

        #endregion

        #region Properties
        public AnimationController AnimationController => Character.AnimationController;
        public MovementUserController MovementUserController => m_MovementUserController;
        public IInputManager InputManager => m_InputManager;

        public cCharacterNetworkController CharacterNetworkController => Character.CharacterNetworkController;

        public cPlayerCharacter Character => m_Character;

        public override int TeamID => m_Character.TeamID;

        #endregion

        private void Awake()
        {
            m_InputManager = global::InputManager.Instance.GetComponent<IInputManager>();
            m_MovementUserController = GetComponentInChildren<MovementUserController>();
            m_MovementUserController.InputManager = m_InputManager;
            Character.InventoryManager.InitInventory(Character.TeamID);
        }

        protected override void Start()
        {
            cPlayerManager.Instance.m_Players.Add(gameObject);
            
            Debug.Log(CharacterNetworkController.IsOwner);
            if (!CharacterNetworkController.IsOwner)
            {
                MovementUserController.enabled = false;
                return;
            }
            
            Empty.InitializeState("Empty", this);
            FreeRoam.InitializeState("FreeRoam", this);
            Fight.InitializeState("Fight", this);
            Dead.InitializeState("Dead", this);
            base.Start();
            
            cPlayerManager.Instance.m_OwnerPlayerSpawn.Invoke(this.transform);
            
            Character.HealthManager.m_OnDied += () =>
            {
                cScoreClientHolder.Instance.AddDead(m_LastDamager);
                ChangeState(Dead);
            };
        }

        protected override void Update()
        {
            if(!CharacterNetworkController.IsOwner) return;
            base.Update();

            if (Physics.Raycast(transform.position + transform.up, -transform.up, out var hit,5, m_LayerMask))
            {
                if (Mathf.Abs(transform.position.y - hit.point.y) > m_YDistance)
                {
                    var pos = transform.position;
                    transform.position = new Vector3(pos.x, hit.point.y, pos.z);
                }
            }
        }

        protected override cStateBase GetInitialState()
        {
            return FreeRoam;
        }
        
        private DamageWrapper m_LastDamager;

        public override void OnDamage(DamageWrapper damageWrapper)
        {
            if(!CharacterNetworkController.IsOwner) return;
            
            if(CurrentState == Dead) return;
            
            base.OnDamage(damageWrapper);
            Character.HealthManager.OnDamage(10);
            m_LastDamager = damageWrapper;
            Character.PlayerCharacterNetworkController.TakeDamageServerRpc(damageWrapper.pos);

            AnimationController.SetTrigger(damageWrapper.isHeavyDamage ? AnimationController.AnimationState.BackImpact : AnimationController.AnimationState.Damage, 
                resetable: true);
            AnimationController.SetTrigger(AnimationController.AnimationState.DamageAnimIndex, Random.Range(0, 2));
        }

        public void OnDamageAnim()
        {
            m_BloodExpo.PlayWithClear();
            Character.SoundEffectController.PlayDamageGrunt();
            // m_DustExpo.PlayWithClear();
        }
    }
using System.Collections.Generic;
using System.Linq;
using _Main.Scripts.Gameplay;
using ArenaGame.Utils;
using FiniteStateMachine;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay.Character.NPCHuman
{
    public class NPCHumanStateMachine : cCharacterStateMachine
    {
        #region PritaveFields

        [SerializeField] private ParticleSystem m_DustExpo;
        [SerializeField] private ParticleSystem m_BloodExpo;
        [SerializeField] private HumanCharacter m_Character;
        [SerializeField] private NPCHumanSMBlackboard m_BlackBoard;
        [SerializeField] private List<ArmorItemSO> m_ArmorItemSos;

        // #region Properties

        public cStateBase Empty => m_BlackBoard.m_Empty;
        public cStateBase Fight => m_BlackBoard.m_Fight;
        public cStateBase Dead => m_BlackBoard.m_Dead;

        [SerializeField] private float m_YDistance;
        [SerializeField] private LayerMask m_LayerMask;
        [SerializeField] private bool m_DisableYCheck;

        #endregion

        #region States

        #endregion

        #region Properties
        public AnimationController AnimationController => Character.AnimationController;

        public cCharacterNetworkController CharacterNetworkController => Character.CharacterNetworkController;

        public HumanCharacter Character => m_Character;

        public override int TeamID => m_Character.TeamID;

        #endregion

        protected override void Awake()
        {
            base.Awake();
            Character.InventoryManager.InitInventory(Character.TeamID);
        }

        protected override void Start()
        {
            cPlayerManager.Instance.m_Players.Add(gameObject);
            
            Empty.InitializeState("Empty", this);
            Fight.InitializeState("Fight", this);
            Dead.InitializeState("Dead", this);
            base.Start();
            
            Character.OnDamage += OnDamage;
            Character.HealthManager.m_OnDied += () =>
            {
                cScoreClientHolder.Instance.AddDead(m_LastDamager);
                ChangeState(Dead);
            };
            
            Character.SkinManager.ClearAllEquipment();

            m_ArmorItemSos.Shuffle();
            var randomItems = m_ArmorItemSos.Take(Random.Range(1,5));
            foreach (var armorItemSo in randomItems)
            {
                Character.SkinManager.EquipItem(armorItemSo);
            }
        }

        protected override void Update()
        {
            base.Update();

            if (!m_DisableYCheck)
            {
                if (Physics.Raycast(transform.position + transform.up, -transform.up, out var hit,5, m_LayerMask))
                {
                    if (Mathf.Abs(transform.position.y - hit.point.y) > m_YDistance)
                    {
                        var pos = transform.position;
                        transform.position = new Vector3(pos.x, hit.point.y, pos.z);
                    }
                }
            }
        }

        protected override cStateBase GetInitialState()
        {
            return Fight;
        }
        
        private DamageWrapper m_LastDamager;

        public override void OnDamage(DamageWrapper damageWrapper)
        {
            if(CurrentState == Dead) return;
            
            m_LastDamager = damageWrapper;
            
            base.OnDamage(damageWrapper);
            Character.HealthManager.OnDamage(damageWrapper.amount);
            Character.PlayerCharacterNetworkController.TakeDamageServerRpc(damageWrapper.pos);
            Character.CharacterStateMachine.PlayTakeDamage(damageWrapper.isHeavyDamage);
        } 
    }
}

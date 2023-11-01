using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FiniteStateMachine
{
    public abstract class cStateMachine : MonoBehaviour, IDamagable
    {
        #region Fields

        [SerializeField] private string m_CurrentStateName;
        [SerializeField] private string m_CurrentSubState;
        [SerializeField] private Transform m_FocusTransform;
        
        public abstract int TeamID { get; }
        public Transform FocusPoint => m_FocusTransform;

        public bool m_ShowStates;

        #endregion
        
        #region States

        #endregion

        #region Properties

        public string CurrentStateName
        {
            get => m_CurrentStateName;
            set => m_CurrentStateName = value;
        }

        public string CurrentSubState
        {
            get => m_CurrentSubState;
            set => m_CurrentSubState = value;
        }

        public cStateBase CurrentState { get; set; }

        #endregion

        protected virtual void Start()
        {
            CurrentState = GetInitialState();
            if(CurrentState != null) CurrentState.Enter();
        }

        protected virtual void Update()
        {
            if(CurrentState != null) CurrentState.StateMachineUpdate();
        }
        
        protected virtual void FixedUpdate()
        {
            if(CurrentState != null) CurrentState.StateMachineFixedUpdate();
        }

        public void ChangeState(cStateBase cState)
        {
            if(m_ShowStates) Debug.Log(cState.m_StateName);
            this.CurrentState.Exit();
            this.CurrentState = cState;
            this.CurrentState.Enter();
        }

        protected virtual cStateBase GetInitialState()
        {
            return null;
        }


        public virtual void Damage(int amount, Vector3 pos,bool isHeavyDamage)
        {
            
        }
    }
}
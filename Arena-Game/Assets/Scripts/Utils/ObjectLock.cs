using System;
using System.Collections.Generic;
using System.Linq;
using ArenaGame.Utils;
using UnityEngine;
using Object = System.Object;

namespace STNest.Utils
{
    [Serializable]
    public class UIObjectLock
    {
        public bool IsActive => m_Activators.Count >= m_MinumumActivator && m_Deactivators.IsEmpty();

        [SerializeField] private List<Object> m_Activators = new List<object>();
        [SerializeField] private List<Object> m_Deactivators = new List<object>();
        [SerializeField] private int m_TokensCount;
        [SerializeField] private int m_MinumumActivator = 1;

        public void AddActivator(Object token)
        {
            if (m_Deactivators.Contains(token))
            {
                m_Deactivators.Remove(token);
            }
            else
            {
                if (m_Activators.Contains(token)) throw new Exception();
                m_Activators.Add(token);
            }
        }
        
        public void AddDeactivator(Object token)
        {
            if (m_Activators.Contains(token))
            {
                m_Activators.Remove(token);
            }
            else
            {
                if (m_Deactivators.Contains(token)) throw new Exception();
                m_Deactivators.Add(token);
            }
        }
    }
    
    [Serializable]
    public class ObjectLock
    {
        public bool LockState => m_Tokens.IsEmpty();

        [SerializeField] private List<Object> m_Tokens = new List<object>();
        [SerializeField] private int m_TokensCount;
        

        public void ActivateLock(Object token)
        {
            if (m_Tokens.Contains(token)) throw new Exception();
            
            m_Tokens.Add(token);
            m_TokensCount++;
        }
        
        public void DeactivateLock(Object token)
        {
            if (!m_Tokens.Contains(token)) throw new Exception();
            
            m_Tokens.Remove(token);
            m_TokensCount--;
        }

        public bool HasToken(Object token)
        {
            return m_Tokens.Contains(token);
        }
    }
}
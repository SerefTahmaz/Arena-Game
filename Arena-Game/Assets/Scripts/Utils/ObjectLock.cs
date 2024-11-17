using System;
using System.Collections.Generic;
using ArenaGame.Utils;
using UnityEngine;
using Object = System.Object;

namespace STNest.Utils
{
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
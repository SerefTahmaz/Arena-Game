using UnityEngine;

namespace Authentication
{
    public class BaseAuthProvider : MonoBehaviour
    {
        protected IAuthService m_AuthService;
        
        public virtual void Init(IAuthService authService)
        {
            m_AuthService = authService;
        }
    }
}
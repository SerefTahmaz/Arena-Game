using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Authentication
{
    public class BaseAuthProvider : MonoBehaviour
    {
        protected IAuthService m_AuthService;
        
        public virtual async UniTask Init(IAuthService authService)
        {
            m_AuthService = authService;
        }
    }
}
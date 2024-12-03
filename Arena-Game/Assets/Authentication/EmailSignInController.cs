using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Authentication
{
    public class EmailSignInController: BaseAuthProvider
    {
        [SerializeField] private LoginManager m_LoginManager;
        [SerializeField] private RegistrationManager m_RegistrationManager;
        [SerializeField] private cMenuNode m_MenuNode;
        
        public override async UniTask Init(IAuthService authService)
        {
            await base.Init(authService);
            m_LoginManager.Init(authService);
            m_RegistrationManager.Init(authService);
            m_LoginManager.OnLoggedInUser += HideUI;
            m_RegistrationManager.OnLoggedInUser += HideUI;
        }

        private void HideUI()
        {
            m_MenuNode.Deactivate();
        }
    }
}
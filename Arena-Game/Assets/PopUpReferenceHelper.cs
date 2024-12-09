using ArenaGame.UI;
using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class PopUpReferenceHelper : MonoBehaviour
    {
        [SerializeField] private cView m_View;
        [SerializeField] private TMP_Text m_Text;
        [SerializeField] private cButton m_NoButton;
        [SerializeField] private cButton m_YesButton;

        public cButton YesButton => m_YesButton;
        public cButton NoButton => m_NoButton;
        public TMP_Text Text => m_Text;
        public cView View => m_View;
    }
}
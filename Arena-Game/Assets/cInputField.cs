using ArenaGame;
using ArenaGame.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace DefaultNamespace 
{
    public class cInputField : MonoBehaviour
    {
        [SerializeField] private TMP_InputField m_InputField;
        [SerializeField] private UnityEvent<string> m_OnValueChanged;
        [SerializeField] private cView m_View;

        public UnityEvent<string> OnValueChanged => m_OnValueChanged;

        // Start is called before the first frame update
        void Start()
        {
            m_InputField.onValueChanged.AddListener(HandleOnValueChanged);
        }

        private void HandleOnValueChanged(string newName)
        {
            OnValueChanged.Invoke(newName);
        }

        public void SetText(string text)
        {
            m_InputField.text = text;
        }
        
        public void Activate()
        {
            m_View.Activate();
        }
    
        public void DeActivate()
        {
            m_View.Deactivate();
        }

    }
}
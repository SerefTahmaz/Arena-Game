using System;
using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class PlayerDialogOptionController : MonoBehaviour
    {
        [SerializeField] private TMP_Text m_Text;
        [SerializeField] private cButton m_Button;
        
        public int OptionIndex { get; set; }
        public IPlayerDialogOptionHandler PlayerDialogOptionHandler { get; private set; }

        private void Start()
        {
            m_Button.OnClickEvent.AddListener(OnClick);
        }

        public void Init(string text, int optionIndex, IPlayerDialogOptionHandler playerDialogOptionHandler)
        {
            m_Text.text = text;
            OptionIndex = optionIndex;
            PlayerDialogOptionHandler = playerDialogOptionHandler;
        }

        public void OnClick()
        {
            PlayerDialogOptionHandler?.HandleOptionSelection(this);
        }
    }

    public interface IPlayerDialogOptionHandler
    {
        public void HandleOptionSelection(PlayerDialogOptionController optionController);
    }
}
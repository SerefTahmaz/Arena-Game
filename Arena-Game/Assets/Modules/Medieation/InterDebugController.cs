using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.Medieation
{
    public class InterDebugController : MonoBehaviour
    {
        [SerializeField] private TMP_InputField m_InputField;
        [SerializeField] private Button m_Button;

        private void Awake()
        {
            m_Button.onClick.AddListener((() =>
            {
                int.TryParse(m_InputField.text.ToString(), out var result);
                FindObjectOfType<InterstitialManager>().InterInterval = result;
            }));
        }
    }
}
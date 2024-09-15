using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace ArenaGame.Utils
{
    public class cCellEventHandler : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField] private UnityEvent m_OnEnter;
        [SerializeField] private UnityEvent m_OnDown;
        [SerializeField] private UnityEvent m_OnClick;
        [SerializeField] private UnityEvent m_OnExit;

        public void OnPointerEnter(PointerEventData eventData)
        {
            m_OnEnter.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            m_OnExit.Invoke();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            m_OnDown.Invoke();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            m_OnClick.Invoke();
        }
    }
}
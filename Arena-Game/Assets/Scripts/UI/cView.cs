using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace DemoBlast.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class cView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup m_CanvasGroup;

        public bool m_IsActive;

        public virtual void Activate(bool instant = false)
        {
            m_CanvasGroup.DOComplete();
            m_CanvasGroup.blocksRaycasts = true;
            m_CanvasGroup.interactable = true;
            m_CanvasGroup.DOFade(1, instant? 0:.2f);
            m_IsActive = true;
        }
    
        public virtual void Deactivate(bool instant = false)
        {
            m_CanvasGroup.DOComplete();
            m_CanvasGroup.blocksRaycasts = false;
            m_CanvasGroup.interactable = false;
            m_CanvasGroup.DOFade(0,instant? 0:.2f);
            m_IsActive = false;
        }
    }
}
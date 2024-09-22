using System.Collections;
using System.Collections.Generic;
using ArenaGame.UI;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ArenaGame.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class cView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup m_CanvasGroup;
        
        [SerializeField] private UnityEvent m_OnActivateEvent;
        [SerializeField] private UnityEvent m_OnDeActivateEvent;

        public UnityEvent OnActivateEvent => m_OnActivateEvent;
        public UnityEvent OnDeActivateEvent => m_OnDeActivateEvent;

        public bool m_IsActive;

        public CanvasGroup CanvasGroup => m_CanvasGroup;

        public virtual void Activate(bool instant = false)
        {
            if (Application.isPlaying)
            {
                CanvasGroup.DOComplete();
                CanvasGroup.blocksRaycasts = true;
                CanvasGroup.interactable = true;
                CanvasGroup.DOFade(1, instant? 0:.2f);
                m_IsActive = true;
                OnActivateEvent.Invoke();
            }
            else
            {
                CanvasGroup.blocksRaycasts = true;
                CanvasGroup.interactable = true;
                CanvasGroup.alpha = 1;
                m_IsActive = true;
            }
        }
    
        public virtual void Deactivate(bool instant = false)
        {
            if (Application.isPlaying)
            {
                CanvasGroup.DOComplete();
                CanvasGroup.blocksRaycasts = false;
                CanvasGroup.interactable = false;
                CanvasGroup.DOFade(0,instant? 0:.2f);
                m_IsActive = false;
                m_OnDeActivateEvent.Invoke();
            }
            else
            {
                CanvasGroup.blocksRaycasts = false;
                CanvasGroup.interactable = false;
                CanvasGroup.alpha = 0;
                m_IsActive = false;
            }
        }
    }
    
#if UNITY_EDITOR
    [CustomEditor(typeof(cView))]
    public class ViewEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var viewTarget = (target as cView);
            
            if (GUILayout.Button(  viewTarget.m_IsActive ? "Deactivate" : "Activate"))
            {
                if (viewTarget.m_IsActive)
                {
                    viewTarget.Deactivate();
                }
                else
                {
                    viewTarget.Activate();
                }
                
                EditorUtility.SetDirty(viewTarget);
                EditorUtility.SetDirty(viewTarget.CanvasGroup);
            }
        }
    }
#endif
}
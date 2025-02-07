using System;
using System.Collections;
using System.Collections.Generic;
using ArenaGame.Utils;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ProgressBarController : MonoBehaviour
{
    [SerializeField] private Transform m_CircularUI;
    [SerializeField] private cMenuNode m_MenuNode;
    [SerializeField] private float m_CircularUISpeed;
    [SerializeField] private Image m_ProgressBar;
    [SerializeField] private List<AnimationCurve> m_AnimationCurves;
    [SerializeField] private Transform m_BG;
    [SerializeField] private Transform m_BGPivot;
    [SerializeField] private float m_Range;
    [SerializeField] private float m_Duration;
    [SerializeField] private float m_BGScaleRange;
    [SerializeField] private float m_BGScaleDuration;
    [SerializeField] private BGAnimType m_BgAnimType;

    private Tween m_LastProgressTween;
    private Tween m_BGAnimTween;
    private float m_DeactivationDuration;
    private bool m_IsDeactivated;
    
    public enum BGAnimType
    {
        Scale,
        Movement
    }
    
    // Start is called before the first frame update
    void Start() 
    {
        m_MenuNode.OnActivateEvent.AddListener(HandleActivated);
        m_MenuNode.OnDeActivateEvent.AddListener(HandleDeactivated);
        m_DeactivationDuration = 50;
    }

    private void HandleDeactivated()
    {
        m_IsDeactivated = true;
    }

    private void HandleActivated()
    {
        m_IsDeactivated = false;
        if (m_DeactivationDuration < 2)
        {
            m_DeactivationDuration = 0;
            return;
        }
        m_DeactivationDuration = 0;
        
        m_CircularUI.DOKill();
        m_CircularUI
            .DORotate(-Vector3.forward * 30, m_CircularUISpeed, RotateMode.FastBeyond360)
            .SetRelative(true)
            .SetLoops(-1, LoopType.Incremental)
            .SetEase(Ease.Linear)
            .SetSpeedBased();
        UpdateProgressBar();
        BGAnim();
    }

    private void BGAnim()
    {
        m_BG.DOKill();
        m_BGAnimTween.Kill();

        switch (m_BgAnimType)
        {
            case BGAnimType.Scale:
                m_BGAnimTween=DOVirtual.Float(0, 1, m_BGScaleDuration, value =>
                {
                    m_BG.localScale = Vector3.one + Vector3.one * value * m_BGScaleRange;
                }).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuad);
                break;
            case BGAnimType.Movement:
                m_BGAnimTween=DOVirtual.Float(0, 1, m_Duration, value =>
                {
                    m_BG.position = m_BGPivot.position + Vector3.right * value * m_Range;
                }).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuad);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void UpdateProgressBar()
    {
        m_LastProgressTween.Kill();
        m_ProgressBar.fillAmount = 0;
        var randomCurve = m_AnimationCurves.RandomItem();
        m_LastProgressTween=DOVirtual.Float(0, 1, 10* Random.Range(0.8f,1.2f), value =>
        {
            m_ProgressBar.fillAmount = randomCurve.Evaluate(value);
        });

    }

    private void Update()
    {
        if (m_IsDeactivated)
        {
            m_DeactivationDuration += Time.deltaTime;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using ArenaGame.UI;
using ArenaGame.UI.PopUps.InfoPopUp;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class NoWifiPopUpController : PopUpController
{
    [SerializeField] private cView m_View;

    private void Awake()
    {
        StartAnim();
    }

    private async UniTask StartAnim()
    {
        transform.localScale = Vector3.zero;
        await transform.DOScale(1, 0.35f).SetEase(Ease.OutBack);
        await UniTask.WaitForSeconds(2);
        await transform.DOScale(0, 0.35f).SetEase(Ease.OutBack);
        Destroy(gameObject);
    }
}

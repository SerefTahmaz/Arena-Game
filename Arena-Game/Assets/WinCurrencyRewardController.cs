using System.Collections;
using System.Collections.Generic;
using ArenaGame.Currency;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class WinCurrencyRewardController : MonoBehaviour,IWinReward
{
    [SerializeField] private TMP_Text m_Text;

    private int m_Amount;
    
    public void Init(int amount)
    {
        m_Amount = amount;
        m_Text.text = m_Amount.ToString();
    }

    public async UniTask GiveReward()
    {
        CurrencyManager.GainCurrency(m_Amount);
        await transform.DOScale(0.15f, 0.15f).SetLoops(2, LoopType.Yoyo).SetRelative(true);
        await UniTask.WaitForSeconds(0.5f);
    }
}

public interface IWinReward
{
    public Transform transform { get; }
    public UniTask GiveReward();
}

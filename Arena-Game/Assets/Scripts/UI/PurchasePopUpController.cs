using _Main.Scripts;
using ArenaGame.Currency;
using ArenaGame.Utils;
using Cysharp.Threading.Tasks;
using Gameplay;
using UnityEngine;

public class PurchasePopUpController : YesNoPopUpController, IPurchasePopUpController
{
    [SerializeField] private Color m_Color;
    private CharacterSO m_SourceChar;
    private CharacterSO m_TargetChar;

    private int m_Amount;

    public async UniTask<bool> Init(CharacterSO sourceChar, CharacterSO targetChar,string itemToPurchaseName, int value, bool isPlayerSelling)
    {
        m_SourceChar = sourceChar;
        m_TargetChar = targetChar;
        m_Amount = value;
        var purchaseString = $"Purchase {itemToPurchaseName.ColorHtmlString(m_Color)} for {m_Amount.ToString().ColorHtmlString(m_Color)}";
        var sellString =  $"Sell {itemToPurchaseName.ColorHtmlString(m_Color)} for {m_Amount.ToString().ColorHtmlString(m_Color)}";
        var result = await base.Init(isPlayerSelling ? sellString : purchaseString);
        return result;
    }

    public override void HandleYes()
    {
        if (m_TargetChar.HasEnoughCurrency(m_Amount))
        {
            m_TargetChar.SpendCurrency(m_Amount);
            if(m_SourceChar != null) m_SourceChar.GainCurrency(m_Amount);
            isSuccessfully = true;
        }
        else
        {
            var infoPopUp=GlobalFactory.InfoPopUpFactory.Create();
            infoPopUp.Init($"{m_TargetChar.name} has not <color=red>enough</color> currency!");
            isSuccessfully = false;
        }
        waitLock = false;
        gameObject.SetActive(false);
    }

    public override void HandleNo()
    {
        isSuccessfully = false;
        waitLock = false;
        gameObject.SetActive(false);
    }
}

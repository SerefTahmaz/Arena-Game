using _Main.Scripts;
using ArenaGame.Currency;
using ArenaGame.Utils;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class PurchasePopUpController : YesNoPopUpController, IPurchasePopUpController
{
    [SerializeField] private Color m_Color;

    private int m_Amount;

    public async UniTask<bool> Init(string itemToPurchaseName, int value)
    {
        m_Amount = value;
        var result = await base.Init(
            $"Purchase {itemToPurchaseName.ColorHtmlString(m_Color)} for {m_Amount.ToString().ColorHtmlString(m_Color)}");
        return result;
    }

    public override void HandleYes()
    {
        if (CurrencyManager.HasEnoughCurrency(m_Amount))
        {
            CurrencyManager.SpendCurrency(m_Amount);
            isSuccessfully = true;
        }
        else
        {
            var infoPopUp=GlobalFactory.InfoPopUpFactory.Create();
            infoPopUp.Init("Not <color=red>enough</color> currency!");
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

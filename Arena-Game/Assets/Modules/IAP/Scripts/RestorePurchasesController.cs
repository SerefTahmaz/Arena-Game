using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Firebase.Auth;
using UnityEngine;

public class RestorePurchasesController : MonoBehaviour
{
    [SerializeField] private cButton m_Button;
    
    // Start is called before the first frame update
    void Start()
    {
        m_Button.OnClickEvent.AddListener((() =>
        {
            RestorePurchasePopUp();
        }));
    }

    private async UniTask RestorePurchasePopUp()
    {
        m_Button.enabled = false;
        var insInfoPopUp = GlobalFactory.InfoPopUpFactory.Create();
        await insInfoPopUp.Init("Purchases restored!");
        m_Button.enabled = true;
    }
}

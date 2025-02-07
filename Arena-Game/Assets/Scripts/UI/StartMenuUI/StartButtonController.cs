using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class StartButtonController : MonoBehaviour
{
    [SerializeField] private cButton m_Button;
    [SerializeField] private UnityEvent m_OnClickEvent;
    [SerializeField] private Transform m_CenterPivotPoint;

    public UnityEvent OnClickEvent => m_OnClickEvent;

    // Start is called before the first frame update
    void Start()
    {
        m_Button.OnClickEvent.AddListener(HandleOnClicked);
    }
 
    private void HandleOnClicked()
    {
        if (!GameplayStatics.CheckInternetConnection())
        {
            var notWifiPopUpIns = GlobalFactory.NoWifiPopUpFactory.Create();
            notWifiPopUpIns.transform.position = m_CenterPivotPoint.position;
            return;
        }
        
        OnClickEvent.Invoke();
    }
}

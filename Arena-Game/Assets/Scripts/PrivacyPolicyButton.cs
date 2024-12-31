using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrivacyPolicyButton : MonoBehaviour
{
    [SerializeField] private cButton m_Button;
    
    // Start is called before the first frame update
    void Start()
    {
        m_Button.OnClickEvent.AddListener((() =>
        {
            Application.OpenURL("https://sites.google.com/view/lengendaryitemprivacypolicy/home");
        }));
    }
}

using System.Collections;
using System.Collections.Generic;
using ArenaGame;
using TMPro;
using UnityEngine;

public class WinsInfoView : MonoBehaviour
{
    [SerializeField] private TMP_Text m_Text;
    
    // Start is called before the first frame update
    void Start()
    {
        m_Text.text = ProfileGenerator.GetPlayerProfile().WinsCount.ToString();
    }
}

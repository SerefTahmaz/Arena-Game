using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCardController : MonoBehaviour
{
    [SerializeField] private ProfileCardController m_ProfileCardController;
    
    // Start is called before the first frame update
    void Start()
    {
        m_ProfileCardController.SetRandomProfile();
    }
}

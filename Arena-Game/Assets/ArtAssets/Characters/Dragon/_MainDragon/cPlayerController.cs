using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cPlayerController : MonoBehaviour
{
    [SerializeField] private float m_CharSpeed;
    [SerializeField] private float m_Rotate;

    [SerializeField] private Animator m_Anim;

    private bool m_IsAnimation = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(m_IsAnimation == false) transform.Translate(Vector3.forward * Time.deltaTime * m_CharSpeed);

        if (Input.GetMouseButtonDown(0) && m_IsAnimation == false)
        {
            m_Anim.applyRootMotion = true;
            m_Anim.SetTrigger("Attack");
            m_IsAnimation = true;
        }
    }

    public void OnAttackEnd()
    {
        m_IsAnimation = false;
        m_Anim.applyRootMotion = false;
    }
}

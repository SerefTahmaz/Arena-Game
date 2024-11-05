using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickController : MonoBehaviour
{
    [SerializeField] private Transform m_Mother;
    [SerializeField] private float m_DistanceToFollow;
    [SerializeField] private Animator m_Animator;

    private Vector3 targetOffset;
    
    // Start is called before the first frame update
    void Start()
    {
        m_Animator.SetFloat("RandomOffset", Random.value);
        targetOffset = Random.onUnitSphere;
        targetOffset.y = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, m_Mother.position+targetOffset) < m_DistanceToFollow)
        {
            m_Animator.SetFloat("Forward",Mathf.Lerp(m_Animator.GetFloat("Forward"),0,Time.deltaTime*10));
        }
        else
        {
            Vector3 dir = m_Mother.position+targetOffset - transform.position;
            dir.Normalize();
            var lookDir = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookDir, Time.deltaTime*5);
            transform.position = Vector3.Lerp(transform.position, m_Mother.position+targetOffset, Time.deltaTime * 2);
            
            m_Animator.SetFloat("Forward",Mathf.Lerp(m_Animator.GetFloat("Forward"),5,Time.deltaTime*10));
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ArenaGame.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

public class cDragonFoot : MonoBehaviour
{
    
    [SerializeField] private LayerMask m_LayerMask;

    [SerializeField] private float m_UpperLimit;
    [SerializeField] private float m_LowerLimit;
    [SerializeField] private float m_StepVolume;

    [SerializeField] private FootHelper m_LeftFront;
    [SerializeField] private FootHelper m_RightFront;
    [SerializeField] private FootHelper m_LeftRear;
    [SerializeField] private FootHelper m_RightRear;

    [SerializeField] private List<AudioClip> m_StepClips;

    [Serializable]
    public class FootHelper
    {
        public bool m_HasSound;
        public AudioSource m_AudioSource;
        public Transform m_Foot;
        public bool m_IsAbove;
        public ParticleSystem m_Dust;
    }
    
    private bool m_IsPlayingRustle = false;

    private void Update()
    {
       TestFoot(m_LeftFront);
       TestFoot(m_RightFront);
       TestFoot(m_LeftRear);
       TestFoot(m_RightRear);
    }

    public void TestFoot(FootHelper footHelper)
    {
        if (Physics.Raycast(footHelper.m_Foot.position, Vector3.down, out var hit, 2, m_LayerMask))
        {
            if (hit.distance > m_UpperLimit)
            {
                footHelper.m_IsAbove = true;
            }
            
            if (hit.distance < m_LowerLimit && footHelper.m_IsAbove)
            {
                footHelper.m_IsAbove = false;
                if(footHelper.m_HasSound) footHelper.m_AudioSource.PlayOneShot(m_StepClips.OrderBy((clip => Random.Range(0,1000))).FirstOrDefault());
                footHelper.m_AudioSource.volume = m_StepVolume;
                footHelper.m_Dust.PlayWithClear();
            }
        }
    }

}

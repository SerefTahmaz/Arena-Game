using System;
using System.Collections;
using System.Collections.Generic;
using ArenaGame.Utils;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Audio;

public abstract class SetMixerVolume : MonoBehaviour
{
    [SerializeField] private AudioMixer m_AudioMixer;
    [SerializeField] private string m_GroupName;
    [SerializeField] private cSlider m_Slider;
    
    public abstract float TargetValue { get; set; }
    public abstract Action TargetChangeEvent { get; set; }
    
    // Start is called before the first frame update
    void Start()
    {
        m_Slider.OnValueChangedEvent.AddListener(HandleValueChanged);
        
        TargetChangeEvent += HandleOnChange;
        HandleOnChange();
    }

    private void HandleOnChange()
    {
        m_Slider.OnValueChangedEvent.RemoveListener(HandleValueChanged);
        m_Slider.SetValue(TargetValue);
        HandleValueChanged(TargetValue);
        m_Slider.OnValueChangedEvent.AddListener(HandleValueChanged);
    }

    private void HandleValueChanged(float value)
    {
       
        var remapped = value.Remap(0, 1, 0.0001f, 1);
        m_AudioMixer.SetFloat(m_GroupName, Mathf.Log10(remapped)*20);
        
        TargetChangeEvent -= HandleOnChange;
        TargetValue = value;
        TargetChangeEvent += HandleOnChange;
        Debug.Log("Volume adjusted");
    }
}
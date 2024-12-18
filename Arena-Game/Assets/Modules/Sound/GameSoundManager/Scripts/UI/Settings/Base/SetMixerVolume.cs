using System;
using System.Collections;
using System.Collections.Generic;
using ArenaGame.UI;
using ArenaGame.Utils;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Audio;

public abstract class SetMixerVolume : MonoBehaviour
{
    [SerializeField] private AudioMixer m_AudioMixer;
    [SerializeField] private string m_GroupName;
    [SerializeField] private cSlider m_Slider;
    [SerializeField] private cMenuNode m_MenuNode;
    
    public abstract float TargetValue { get; set; }
    public abstract Action TargetChangeEvent { get; set; }
    
    // Start is called before the first frame update
    void Start()
    {
        m_Slider.OnValueChangedEvent.AddListener(HandleValueChanged);
        m_MenuNode.OnActivateEvent.AddListener(UpdateUI);
        UpdateUI();
    }
    
    private void HandleValueChanged(float value)
    {
        UpdateMixer(value);
        TargetValue = value;
        Debug.Log("Volume adjusted");
    }
    
    private void UpdateUI()
    {
        UpdateMixer(TargetValue);
        m_Slider.SetValue(TargetValue);
    }

    private void UpdateMixer(float value)
    {
        var remapped = value.Remap(0, 1, 0.0001f, 1);
        m_AudioMixer.SetFloat(m_GroupName, Mathf.Log10(remapped)*20);
    }
} 
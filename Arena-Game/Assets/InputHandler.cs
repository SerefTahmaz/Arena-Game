using System;
using System.Collections;
using System.Collections.Generic;
using DemoBlast.Utils;
using RootMotion;
using UnityEngine;

public class InputHandler : cSingleton<InputHandler>
{
    public static Vector2 m_JoystickValue;
    public static bool m_RKeyDownValue;
    public static bool m_SpaceKeyDownValue;
    public static bool m_Mouse0DownValue;
    public static bool m_Mouse0UpValue;
    public static bool m_Mouse1DownValue;
    public static bool m_Mouse1UpValue;
    public static bool m_EKeyDownValue;
    public static bool m_Alpha1KeyValue;
    public static bool m_Alpha2KeyValue;
    public static bool m_Alpha3KeyValue;
    public static bool m_Alpha4KeyValue;
    public static bool m_LeftShiftKeyDownValue;
    public static bool m_LeftShiftKeyUpValue;
    public static bool m_LeftControlKeyValue;
    public static bool m_FKeyValue;

    
    private bool m_SetMouse0DownValue;
    public void SetMouse0Down()
    {
        m_SetMouse0DownValue = true;
    }
    
    private bool m_SetRKeyValue;
    public void SetRKeyValue()
    {
        m_SetRKeyValue = true;
    }
    
    private bool m_SetSpaceKeyDownValue;
    public void SetSpaceKeyDownValue()
    {
        m_SetSpaceKeyDownValue = true;
    }
    
    private bool m_SetMouse0UpValue;
    public void SetMouse0UpValue()
    {
        m_SetMouse0UpValue = true;
    }
    
    private bool m_SetMouse1DownValue;
    public void SetMouse1DownValue()
    {
        m_SetMouse1DownValue = true;
    }
    
    private bool m_SetMouse1UpValue;
    public void SetMouse1UpValue()
    {
        m_SetMouse1UpValue = true;
    }
    
    private bool m_SetEKeyDownValue;
    public void SetEKeyDownValue()
    {
        m_SetEKeyDownValue = true;
    }
    
    private bool m_SetAlpha1KeyValue;
    public void SetAlpha1KeyValue()
    {
        m_SetAlpha1KeyValue = true;
    }
    
    private bool m_SetAlpha2KeyValue;
    public void SetAlpha2KeyValue()
    {
        m_SetAlpha2KeyValue = true;
    }
    
    private bool m_SetAlpha3KeyValue;
    public void SetAlpha3KeyValue()
    {
        m_SetAlpha3KeyValue = true;
    }
    
    private bool m_SetAlpha4KeyValue;
    public void SetAlpha4KeyValue()
    {
        m_SetAlpha4KeyValue = true;
    }
    
    private bool m_SetLeftShiftKeyDownValue;
    public void SetLeftShiftKeyDownValue()
    {
        m_SetLeftShiftKeyDownValue = true;
    }
    
    private bool m_SetLeftControlKeyValue;
    public void SetLeftControlKeyValue()
    {
        m_SetLeftControlKeyValue = true;
    }
    
    private bool m_SetLeftShiftKeyUpValue;
    public void SetLeftShiftKeyUpValue()
    {
        m_SetLeftShiftKeyUpValue = true;
    }
    

    private void Start()
    {
        StartCoroutine(LoopValues());
    }

    IEnumerator LoopValues()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            
            m_RKeyDownValue = false;
            if (m_SetRKeyValue)
            {
                m_RKeyDownValue = true;
                m_SetRKeyValue = false;
            }
            
            m_SpaceKeyDownValue = false;
            if (m_SetSpaceKeyDownValue)
            {
                m_SpaceKeyDownValue = true;
                m_SetSpaceKeyDownValue = false;
            }
            
            m_Mouse0DownValue = false;
            if (m_SetMouse0DownValue)
            {
                m_Mouse0DownValue = true;
                m_SetMouse0DownValue = false;
            }
            
            m_Mouse0UpValue = false;
            if (m_SetMouse0UpValue)
            {
                m_Mouse0UpValue = true;
                m_SetMouse0UpValue = false;
            }
            
            m_Mouse1DownValue = false;
            if (m_SetMouse1DownValue)
            {
                m_Mouse1DownValue = true;
                m_SetMouse1DownValue = false;
            }
            
            m_Mouse1UpValue = false;
            if (m_SetMouse1UpValue)
            {
                m_Mouse1UpValue = true;
                m_SetMouse1UpValue = false;
            }
            
            m_EKeyDownValue = false;
            if (m_SetEKeyDownValue)
            {
                m_EKeyDownValue = true;
                m_SetEKeyDownValue = false;
            }
            
            m_Alpha1KeyValue = false;
            if (m_SetAlpha1KeyValue)
            {
                m_Alpha1KeyValue = true;
                m_SetAlpha1KeyValue = false;
            }
            
            m_Alpha2KeyValue = false;
            if (m_SetAlpha2KeyValue)
            {
                m_Alpha2KeyValue = true;
                m_SetAlpha2KeyValue = false;
            }
            
            m_Alpha3KeyValue = false;
            if (m_SetAlpha3KeyValue)
            {
                m_Alpha3KeyValue = true;
                m_SetAlpha3KeyValue = false;
            }
            
            m_Alpha4KeyValue = false;
            if (m_SetAlpha4KeyValue)
            {
                m_Alpha4KeyValue = true;
                m_SetAlpha4KeyValue = false;
            }
            
            m_Alpha4KeyValue = false;
            if (m_SetAlpha4KeyValue)
            {
                m_Alpha4KeyValue = true;
                m_SetAlpha4KeyValue = false;
            }
            
            m_Alpha4KeyValue = false;
            if (m_SetAlpha4KeyValue)
            {
                m_Alpha4KeyValue = true;
                m_SetAlpha4KeyValue = false;
            }
            
            m_LeftControlKeyValue = false;
            if (m_SetLeftControlKeyValue)
            {
                m_LeftControlKeyValue = true;
                m_SetLeftControlKeyValue = false;
            }
            
            m_LeftShiftKeyDownValue = false;
            if (m_SetLeftShiftKeyDownValue)
            {
                m_LeftShiftKeyDownValue = true;
                m_SetLeftShiftKeyDownValue = false;
            }
            
            m_LeftShiftKeyUpValue = false;
            if (m_SetLeftShiftKeyUpValue)
            {
                m_LeftShiftKeyUpValue = true;
                m_SetLeftShiftKeyUpValue = false;
            }
        }
        
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetResolution : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    { 
        switch (DeviceTypeChecker.GetDeviceType())
        {
            case ENUM_Device_Type.Tablet:
                Screen.SetResolution(1366,1024, true);
                break;
            case ENUM_Device_Type.Phone:
                Screen.SetResolution(1792,828, true);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    } 

    // Update is called once per frame
    void Update()
    {
        
    }
}


public enum ENUM_Device_Type
{
    Tablet,
    Phone
}

public static class DeviceTypeChecker
{
    public static bool isTablet;
  
    private static float DeviceDiagonalSizeInInches()
    {
        float screenWidth = Screen.width / Screen.dpi;
        float screenHeight = Screen.height / Screen.dpi;
        float diagonalInches = Mathf.Sqrt(Mathf.Pow(screenWidth, 2) + Mathf.Pow(screenHeight, 2));

        return diagonalInches;
    }

    public static ENUM_Device_Type GetDeviceType()
    {
#if UNITY_IOS
    bool deviceIsIpad = UnityEngine.iOS.Device.generation.ToString().Contains("iPad");
            if (deviceIsIpad)
            {
                return ENUM_Device_Type.Tablet;
            }
            bool deviceIsIphone = UnityEngine.iOS.Device.generation.ToString().Contains("iPhone");
            if (deviceIsIphone)
            {
                return ENUM_Device_Type.Phone;
            }
#elif UNITY_ANDROID

        float aspectRatio = Mathf.Max(Screen.width, Screen.height) / Mathf.Min(Screen.width, Screen.height);
        bool isTablet = (DeviceDiagonalSizeInInches() > 6.5f && aspectRatio < 2f);

        if (isTablet)
        {
            return ENUM_Device_Type.Tablet;
        }
        else
        {
            return ENUM_Device_Type.Phone;
        }
#endif
    }
}
using System;
using ArenaGame.Managers.SaveManager;

public class SetSFXVolume : SetMixerVolume
{
    public override float TargetValue
    {
        get => UtilitySaveHandler.SaveData.m_SfxVolume;
        set
        {
            UtilitySaveHandler.SaveData.m_SfxVolume = value;
            UtilitySaveHandler.Save();
        }
    }

    public override Action TargetChangeEvent
    {
        get => UtilitySaveHandler.OnChanged;
        set =>  UtilitySaveHandler.OnChanged = value;
    }
}
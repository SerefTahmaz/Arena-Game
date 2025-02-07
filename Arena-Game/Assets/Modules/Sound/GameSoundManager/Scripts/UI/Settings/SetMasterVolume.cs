using System;
using ArenaGame.Managers.SaveManager;

public class SetMasterVolume : SetMixerVolume
{
    public override float TargetValue
    {
        get => UtilitySaveHandler.SaveData.m_MasterVolume;
        set
        {
            UtilitySaveHandler.SaveData.m_MasterVolume = value;
            UtilitySaveHandler.Save();
        }
    }

    public override Action TargetChangeEvent
    {
        get => UtilitySaveHandler.OnChanged;
        set =>  UtilitySaveHandler.OnChanged = value;
    }
}
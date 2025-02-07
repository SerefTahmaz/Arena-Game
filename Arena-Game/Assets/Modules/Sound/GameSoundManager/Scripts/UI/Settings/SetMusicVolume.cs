using System;
using ArenaGame.Managers.SaveManager;

public class SetMusicVolume : SetMixerVolume
{
    public override float TargetValue
    {
        get => UtilitySaveHandler.SaveData.m_MusicVolume;
        set
        {
            UtilitySaveHandler.SaveData.m_MusicVolume = value;
            UtilitySaveHandler.Save();
        }
    }

    public override Action TargetChangeEvent
    {
        get => UtilitySaveHandler.OnChanged;
        set =>  UtilitySaveHandler.OnChanged = value;
    }
} 
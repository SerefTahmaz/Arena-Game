using System;
using ArenaGame.Managers.SaveManager;

public class SetVoiceVolume : SetMixerVolume
{
    public override float TargetValue
    {
        get => UtilitySaveHandler.SaveData.m_VoiceVolume;
        set
        {
            UtilitySaveHandler.SaveData.m_VoiceVolume = value;
            UtilitySaveHandler.Save();
        }
    }

    public override Action TargetChangeEvent
    {
        get => UtilitySaveHandler.OnChanged;
        set =>  UtilitySaveHandler.OnChanged = value;
    }
}
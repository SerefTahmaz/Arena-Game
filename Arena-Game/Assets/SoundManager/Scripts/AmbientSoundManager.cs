using System.Collections.Generic;
using ArenaGame.Utils;
using AudioSystem;
using DefaultNamespace;
using DG.Tweening;
using STNest.Utils;
using UnityEngine;
using Object = System.Object;

public class AmbientSoundManager : cSingleton<AmbientSoundManager>
{
    [SerializeField] private SoundData m_SoundData;
    
    private Dictionary<SoundsHolder, AmbientPlayerWrapper> m_AmbientSounds = new Dictionary<SoundsHolder, AmbientPlayerWrapper>();
        
    public class AmbientPlayerWrapper
    {
        public ObjectLock m_ObjectLock = new ObjectLock();
        public SoundEmitter m_SoundEmitter;
        public Tween m_FadeInTween;
    }

    public void PlaySound(SoundsHolder soundsHolder, Object token)
    {
        if (!m_AmbientSounds.ContainsKey(soundsHolder))
        {
            m_AmbientSounds.Add(soundsHolder, new AmbientPlayerWrapper());
        }

        var ambientPlayerWrapper = m_AmbientSounds[soundsHolder];
        ambientPlayerWrapper.m_ObjectLock.ActivateLock(token);


        if (ambientPlayerWrapper.m_SoundEmitter == null)
        {
            m_SoundData.clip = soundsHolder.AudioClips.RandomItem();
            SoundBuilder soundBuilder = SoundManager.Instance.CreateSoundBuilder();
                
            var emitter = soundBuilder
                .WithPosition(transform.position)
                .WithParent(transform)
                .Play(m_SoundData);
            ambientPlayerWrapper.m_SoundEmitter = emitter;

            ambientPlayerWrapper.m_FadeInTween = DOVirtual.Float(0, 1, 1, value =>
            {
                ambientPlayerWrapper.m_SoundEmitter.SetVolume(value);
            });
        }
            
    }
        
    public void StopSound(SoundsHolder soundsHolder, Object token)
    {
        if (!m_AmbientSounds.ContainsKey(soundsHolder))
        {
            m_AmbientSounds.Add(soundsHolder, new AmbientPlayerWrapper());
        }

        var ambientPlayerWrapper = m_AmbientSounds[soundsHolder];
        ambientPlayerWrapper.m_ObjectLock.DeactivateLock(token);

        if (ambientPlayerWrapper.m_ObjectLock.LockState)
        {
            m_AmbientSounds.Remove(soundsHolder);

            ambientPlayerWrapper.m_FadeInTween.Kill();
            DOVirtual.Float(1, 0, 1, value =>
            {
                ambientPlayerWrapper.m_SoundEmitter.SetVolume(value);
            }).OnComplete((() =>
            {
                ambientPlayerWrapper.m_SoundEmitter.Stop();
            }));
        }
    }
}
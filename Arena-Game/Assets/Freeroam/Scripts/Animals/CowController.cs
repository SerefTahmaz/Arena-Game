using System;
using System.Collections;
using System.Collections.Generic;
using ArenaGame.Utils;
using AudioSystem;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class CowController : MonoBehaviour
{
    [SerializeField] private BoxCollider m_PatrolVolume;
    [SerializeField] private float m_Speed;
    [SerializeField] private Animator m_CowAnimator;
    [SerializeField] private Vector2 m_PickingPointDelay;
    
    // Start is called before the first frame update
    void Start()
    {
        m_CowAnimator.SetFloat("RandomOffset", Random.value);
        RandomPatrol();
    }

    private async UniTask RandomPatrol()
    {
        while (true)
        {
            await UniTask.WaitForSeconds(Random.Range(m_PickingPointDelay.x, m_PickingPointDelay.y), cancellationToken: this.GetCancellationTokenOnDestroy());
            await PickAPoint();
        }
    }
 
    public async UniTask PickAPoint()
    {
        var point = m_PatrolVolume.bounds.RandomPointInBounds();
        point.y = transform.position.y;
        var dir = point - transform.position;
        dir.y = 0;

        List<UniTask> tasks = new List<UniTask>();
        
        var rotT = transform.DORotateQuaternion(Quaternion.LookRotation(dir), 1).ToUniTask(cancellationToken:this.GetCancellationTokenOnDestroy());
        var moveT = transform.DOMove(point, m_Speed).SetSpeedBased().SetEase(Ease.Linear).SetUpdate(UpdateType.Fixed).ToUniTask(cancellationToken:this.GetCancellationTokenOnDestroy());
        var animT = DOVirtual.Float(0, 1, 0.25f, value =>
        {
            m_CowAnimator.SetFloat("Forward",value);
        }).SetEase(Ease.Linear).ToUniTask(cancellationToken:this.GetCancellationTokenOnDestroy());
        
        tasks.Add(rotT);
        tasks.Add(moveT);
        tasks.Add(animT);
        await UniTask.WhenAll(tasks);
        await UniTask.WaitForSeconds(0.01f, cancellationToken: this.GetCancellationTokenOnDestroy());
        await DOVirtual.Float(1, 0, 0.25f, value =>
        {
            m_CowAnimator.SetFloat("Forward",value);
        }).SetEase(Ease.Linear).ToUniTask(cancellationToken: this.GetCancellationTokenOnDestroy());
    }
}

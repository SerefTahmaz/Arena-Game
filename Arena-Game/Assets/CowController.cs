using System.Collections;
using System.Collections.Generic;
using ArenaGame.Utils;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

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
            await UniTask.WaitForSeconds(Random.Range(m_PickingPointDelay.x, m_PickingPointDelay.y));
            await PickAPoint();
        }
    }

    public async UniTask PickAPoint()
    {
        var point = m_PatrolVolume.bounds.RandomPointInBounds();
        Debug.Log(point);
        point.y = transform.position.y;
        var dir = point - transform.position;
        dir.y = 0;

        List<UniTask> tasks = new List<UniTask>();
        
        var rotT = transform.DORotateQuaternion(Quaternion.LookRotation(dir), 1).ToUniTask();
        var moveT = transform.DOMove(point, m_Speed).SetSpeedBased().SetEase(Ease.Linear).SetUpdate(UpdateType.Fixed).ToUniTask();
        var animT = DOVirtual.Float(0, 1, 0.25f, value =>
        {
            m_CowAnimator.SetFloat("Forward",value);
        }).SetEase(Ease.Linear).ToUniTask();
        
        tasks.Add(rotT);
        tasks.Add(moveT);
        tasks.Add(animT);
        await UniTask.WhenAll(tasks);
        await DOVirtual.Float(1, 0, 0.25f, value =>
        {
            m_CowAnimator.SetFloat("Forward",value);
        }).SetEase(Ease.Linear).ToUniTask();
    }
}

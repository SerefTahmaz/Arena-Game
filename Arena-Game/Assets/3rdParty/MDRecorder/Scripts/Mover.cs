using UnityEngine;
using DG.Tweening;

public class Mover : MonoBehaviour
{
    public float duration;
    public Vector3 distance;

    void Start()
    {
        transform.DOMove(transform.position + distance, duration).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }
}

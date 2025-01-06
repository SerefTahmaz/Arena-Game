using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RandomColor : MonoBehaviour
{
    void Start()
    {
        GetComponent<MeshRenderer>().material.DOColor(Random.ColorHSV(0, 1, 0.8f, 1, 0.9f, 1), Random.Range(0.75f, 1.25f)).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }
}

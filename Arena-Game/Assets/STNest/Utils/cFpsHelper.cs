using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class cFpsHelper : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private int m_WindowSize;

    private Queue<float> m_FpsWindow = new Queue<float>();

    // Update is called once per frame
    void Update()
    {
        var currentFps = (int)(1 / Time.deltaTime);

        if (m_FpsWindow.Count >= m_WindowSize)
        {
            m_FpsWindow.Dequeue();
        }
        else
        {
            m_FpsWindow.Enqueue(currentFps);
        }

        float averageFps = 0;

        foreach (var VARIABLE in m_FpsWindow)
        {
            averageFps += VARIABLE;
        }

        averageFps /= Math.Max(m_FpsWindow.Count,1);

        if(Time.frameCount % 60 == 0) _text.text = Mathf.CeilToInt(averageFps).ToString();
    }
}

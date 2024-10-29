using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ArenaGame.UI;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using Dialogue;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class DialogController : MonoBehaviour, IPlayerDialogOptionHandler
{
    [SerializeField] private PlayerDialogOptionController m_PlayerDialogOptionControllerPrefab;
    [SerializeField] private Transform m_PlayerOptionLayout;
    [SerializeField] private TMP_Text m_NpcDialogText;
    [SerializeField] private UniversalRenderPipelineAsset m_DialogURPSettings;
    [SerializeField] private cView m_View;
    [SerializeField] private Transform m_CameraPivot;

    private RenderPipelineAsset m_PreviousURPSetting;

    private List<PlayerDialogOptionController> m_InsOptions = new List<PlayerDialogOptionController>();
    
    public DialogueGraph DialogueGraph { get; private set; }

    private bool m_IsDialogShowing { get; set; }

    public async UniTask Init(DialogueGraph dialogueGraph, Transform dialogFocusPoint)
    {
        DialogueGraph = dialogueGraph;
        RestartDialog();
        SetQualitySetting();

        m_CameraPivot.transform.position = dialogFocusPoint.position;
        m_CameraPivot.transform.forward = dialogFocusPoint.forward;
        
        GameplayStatics.SetPlayerVisibility(false);

        m_IsDialogShowing = true;
        await UniTask.WaitWhile((() => m_IsDialogShowing));
    }

    private void SetQualitySetting()
    {
        m_PreviousURPSetting = QualitySettings.renderPipeline;
        QualitySettings.renderPipeline = m_DialogURPSettings;
    }

    private void RestartDialog()
    {
        DialogueGraph.Restart();
        DialogueGraph.SkipStart();
        UpdateUI();
    }

    private void UpdateUI()
    {
        var options = DialogueGraph.current.answers.ToList();

        m_NpcDialogText.text = DialogueGraph.current.text;

        foreach (var optionController in m_InsOptions)
        {
            Destroy(optionController.gameObject);
        }
        m_InsOptions.Clear();
        
        for (var index = 0; index < options.Count; index++)
        {
            var option = options[index];
            var insOption = Instantiate(m_PlayerDialogOptionControllerPrefab, m_PlayerOptionLayout);
            insOption.Init(option.text,index, this);
            m_InsOptions.Add(insOption);
        }
    }

    public void SetAnswer(int answerIndex)
    {
        var isDialogEnded = !DialogueGraph.current.HasAnswerGotOutput(answerIndex);
        if (isDialogEnded)
        {
            ReverseQuality();
            GameplayStatics.SetPlayerVisibility(true);
            m_IsDialogShowing = false;
            Destroy(gameObject);
        }
        else
        {
            DialogueGraph.AnswerQuestion(answerIndex);
            UpdateUI();
        }
    }

    private void ReverseQuality()
    {
        QualitySettings.renderPipeline = m_PreviousURPSetting;
    }

    public void HandleOptionSelection(PlayerDialogOptionController optionController)
    {
        SetAnswer(optionController.OptionIndex);
    }

    public static DialogController CreateInstanceDialog()
    {
        var ins = Instantiate(PrefabList.Get().DialogControllerPrefab);
        return ins;
    }

    public void SetVisibility(bool isVisible)
    {
        if(isVisible) m_View.Activate(true);
        else m_View.Deactivate(true);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(DialogController))]
public class DialogControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Click"))
        {
        }
    }
}
#endif
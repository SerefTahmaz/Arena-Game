using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ArenaGame.UI;
using AudioSystem;
using Cinemachine;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using Dialogue;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
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
    [SerializeField] private CinemachineVirtualCamera m_Camera;
    [SerializeField] private Transform m_CameraPivot;
    [SerializeField] private GameObject m_EventSystem;
    [SerializeField] private SoundData m_SoundData;

    private RenderPipelineAsset m_PreviousURPSetting;

    private List<PlayerDialogOptionController> m_InsOptions = new List<PlayerDialogOptionController>();
    
    public DialogueGraph DialogueGraph { get; private set; }

    private bool m_IsDialogShowing { get; set; }

    public async UniTask Init(DialogueGraph dialogueGraph, Transform dialogFocusPoint)
    {
        DialogueGraph = dialogueGraph;
        ClearRuntimeData();
        RestartDialog();
        SetQualitySetting();

        m_CameraPivot.position = dialogFocusPoint.position;
        m_CameraPivot.forward = dialogFocusPoint.forward;
        m_Camera.LookAt = dialogFocusPoint;
        
        GameplayStatics.SetPlayerVisibility(false);

        if (FindObjectsOfType<EventSystem>().Length > 1)
        {
            Destroy(m_EventSystem);
        }

        m_IsDialogShowing = true;
        await UniTask.WaitWhile((() => m_IsDialogShowing));
        ClearRuntimeData();
    }

    private void ClearRuntimeData()
    {
        foreach (var VARIABLE in DialogueGraph.nodes)
        {
            if (VARIABLE is Chat chat)
            {
                chat.isAlreadyShown = false;
            }
        }
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

        if(!DialogueGraph.current.isAlreadyShown) HandleNpcVoice(DialogueGraph.current);

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
        DialogueGraph.current.isAlreadyShown = true;
    }

    private void HandleNpcVoice(Chat dialogueGraphCurrent)
    {
        Debug.Log("HandleNpcVoice");
        AudioClip clipToPlay = null;
        if (dialogueGraphCurrent.voiceClip != null)
        {
            clipToPlay = dialogueGraphCurrent.voiceClip;
        }
        else if (dialogueGraphCurrent.randomClips != null)
        {
            clipToPlay = dialogueGraphCurrent.randomClips.GetClip();
        }
        else
        {
            Debug.Log("Null ref");
        } 

        if (clipToPlay != null)
        {
            Debug.Log("Playing voice");
            m_SoundData.clip = clipToPlay;
            SoundBuilder soundBuilder = SoundManager.Instance.CreateSoundBuilder();
            soundBuilder
                .WithPosition(transform.position)
                .Play(m_SoundData);
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
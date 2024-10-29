using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using Dialogue;
using TMPro;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class DialogController : MonoBehaviour, IPlayerDialogOptionHandler
{
    [SerializeField] private PlayerDialogOptionController m_PlayerDialogOptionControllerPrefab;
    [SerializeField] private Transform m_PlayerOptionLayout;
    [SerializeField] private TMP_Text m_NpcDialogText;

    private List<PlayerDialogOptionController> m_InsOptions = new List<PlayerDialogOptionController>();
    
    public DialogueGraph DialogueGraph { get; private set; }

    public void Init(DialogueGraph dialogueGraph)
    {
        DialogueGraph = dialogueGraph;
        RestartDialog();
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
        if (!DialogueGraph.current.HasAnswerGotOutput(answerIndex))
        {
            RestartDialog();
        }
        else
        {
            DialogueGraph.AnswerQuestion(answerIndex);
            UpdateUI();
        }
    }

    public void HandleOptionSelection(PlayerDialogOptionController optionController)
    {
        SetAnswer(optionController.OptionIndex);
    }

    public static DialogController CreateInstanceDialog(DialogueGraph dialogueGraph)
    {
        var ins = Instantiate(PrefabList.Get().DialogControllerPrefab);
        ins.Init(dialogueGraph);
        return ins;
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
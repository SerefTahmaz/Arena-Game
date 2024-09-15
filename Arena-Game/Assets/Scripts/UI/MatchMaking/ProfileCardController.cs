using System.Collections;
using System.Collections.Generic;
using ArenaGame;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ProfileCardController : MonoBehaviour
{
    [SerializeField] private TMP_Text m_ProfileName;
    [SerializeField] private RawImage m_ProfilePicture;
    [SerializeField] private TMP_Text m_ExpPoint;
    [SerializeField] private Texture2D m_DefaultProfilePic;

    public void SetRandomProfile()
    {
        var randomProfile = ProfileGenerator.GetRandomProfile();
        SetProfile(randomProfile);
    }

    public void SetProfile(PlayerDataModel playerDataModel)
    {
        m_ProfileName.text = playerDataModel.Name;
        m_ProfilePicture.texture = playerDataModel.ProfilePicture;
        m_ExpPoint.text = playerDataModel.ExpPoint.ToString();
    }

    public void SetSearchingProfile()
    {
        m_ProfileName.text = "SEARCHING...";
        m_ProfilePicture.texture = m_DefaultProfilePic;
        m_ExpPoint.text = "-";
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(ProfileCardController))]
public class ProfileCardControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Click"))
        {
            (target as ProfileCardController).SetRandomProfile();
        }
    }
}
#endif
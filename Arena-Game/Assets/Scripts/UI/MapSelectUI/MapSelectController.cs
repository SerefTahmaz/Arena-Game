using System.Collections;
using System.Collections.Generic;
using ArenaGame.Managers.SaveManager;
using ArenaGame.UI;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MapSelectController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private cButton m_SelectButton;
    [SerializeField] private cButton m_ReturnButton;
    [SerializeField] private cView m_View;
    
    [SerializeField] private cButton m_NextMapButton;
    [SerializeField] private cButton m_PreviousMapButton;

    [Header("UnlockLayer")]
    [SerializeField] private GameObject m_UnlockLayer;
    [SerializeField] private Image m_PreviewImage;
    [SerializeField] private TMP_Text m_MapNameText;
    [SerializeField] private TMP_Text m_MapIndexText;
    [SerializeField] private TMP_Text m_RewardText;
    [SerializeField] private TMP_Text m_LoseText;

    [Header("Lock")]
    [SerializeField] private GameObject m_LockedLayer;
    [SerializeField] private TMP_Text m_RequiredExpPointText;
    [SerializeField] private TMP_Text m_LockMapNameText;
    
    private int CurrentMapIndex;

    private List<MapSO> Maps => MapListSO.Get().MapSOs;
    
    // Start is called before the first frame update
    void Start()
    {
        m_SelectButton.OnClickEvent.AddListener(HandleMapSelectCLicked);
        
        m_View.OnActivateEvent.AddListener(RefreshUI);
        
        m_NextMapButton.OnClickEvent.AddListener(HandleNextMapButtonClicked);
        m_PreviousMapButton.OnClickEvent.AddListener(HandlePreviousMapButtonClicked);
    }

    private void HandlePreviousMapButtonClicked()
    {
        CurrentMapIndex--;
        CurrentMapIndex = Mathf.Max(0, CurrentMapIndex);
        
        RefreshUI();
    }

    private void HandleNextMapButtonClicked()
    {
        var maxLevelCount = Maps.Count;
        CurrentMapIndex++;
        CurrentMapIndex = Mathf.Min(CurrentMapIndex, maxLevelCount-1);
        
        RefreshUI();
    }

    private void RefreshUI()
    {
        var mapSO = Maps[CurrentMapIndex];

        m_MapNameText.text = mapSO.MapName.ToUpper();
        m_MapIndexText.text = $"ARENA {CurrentMapIndex+1}";
        m_PreviewImage.sprite = mapSO.PreviewIcon;
        m_RewardText.text = $"{mapSO.RewardCurrency.ToString()}<sprite=0>";
        m_LoseText.text = $"{mapSO.LoseCurrency.ToString()}<sprite=0>";

        m_RequiredExpPointText.text = $"EARN {mapSO.RequiredExp} <sprite=0> TO UNLOCK";
        m_LockMapNameText.text = mapSO.MapName.ToUpper();
        
        var maxLevelCount = Maps.Count;
        var isLastLevel = CurrentMapIndex == maxLevelCount - 1;
        m_NextMapButton.gameObject.SetActive(!isLastLevel); 
        
        var isFirstLevel = CurrentMapIndex == 0;
        m_PreviousMapButton.gameObject.SetActive(!isFirstLevel);
        
        UserSaveHandler.Load();
        var experiencePoint = UserSaveHandler.SaveData.m_ExperiencePoint;

        var isUnlocked = experiencePoint >= mapSO.RequiredExp;
        
        m_LockedLayer.SetActive(!isUnlocked);
        m_UnlockLayer.SetActive(isUnlocked);
    }

    private void HandleMapSelectCLicked()
    {
        UserSaveHandler.Load();
        var savaData = UserSaveHandler.SaveData;
        savaData.m_CurrentMap = CurrentMapIndex;
        UserSaveHandler.Save();
        
        m_ReturnButton.OnClickEvent.Invoke();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DemoBlast.Utils;
using RootMotion;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraManager : cSingleton<CameraManager>
{
    [SerializeField] private GameObject _gameplayCam;
    [SerializeField] private InputOnMouseDown _gameplayCamInput;
    [SerializeField] private GameObject m_focusCam;
    [SerializeField] private CinemachineFreeLook m_CinemachineFreeLook;

    private List<GameObject> cams = new List<GameObject>();
    private bool _isEscaped = false;
    
    private Action<CameraType> m_OnCameraChange = delegate(CameraType type) {  };
    private CameraType m_CurrentCam = CameraType.Gameplay;
    
    public enum CameraType
    {
        Gameplay,
        Focus,
        NpcCam
    }

    public Action<CameraType> OnCameraChange
    {
        get => m_OnCameraChange;
        set => m_OnCameraChange = value;
    }

    public CameraType CurrentCam => m_CurrentCam;

    private void Awake()
    {
        cMobileInputManager._onFocusEvent += () =>
        {
            if (_gameplayCam.activeSelf)
            {
                SetCamera(CameraType.Focus);
            }
            else
            {
                SetCamera(CameraType.Gameplay);
            }
        };
    }

    private void Start()
    {
        cams.Add(_gameplayCam);
        cams.Add(m_focusCam);
    }

    public void SetCamera(CameraType cam)
    {
        foreach (var VARIABLE in cams)
        {
            VARIABLE.SetActive(false);
        }
        
        cams[(int)cam].SetActive(true);

        m_CurrentCam = cam;
        OnCameraChange.Invoke(cam);
    }
    
    public void EnableGameplayCam()
    {
        SetCamera(CameraType.Gameplay);
    }

    public void EnableFocusCam()
    {
        SetCamera(CameraType.Focus);
    }

    public void StopCameraMovement()
    {
        _gameplayCamInput.StopCamMovement();
    }
    
    public void StartCameraMovement()
    {
        _gameplayCamInput.StartCamMovement();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !_isEscaped)
        {
            StopCameraMovement();
            _isEscaped = true;
        }
        
        if (Input.GetMouseButtonDown(0)&& _isEscaped)
        {
            _isEscaped = false;
            StartCameraMovement();
        }
    }

    public void OnPlayerSpawn()
    {
        var instanceOwnerPlayer = cGameManager.Instance.m_OwnerPlayer;
        _gameplayCam.GetComponent<CinemachineFreeLook>().Follow = instanceOwnerPlayer;
        _gameplayCam.GetComponent<CinemachineFreeLook>().LookAt = instanceOwnerPlayer;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        EnableGameplayCam();
    }

    public GameObject GetCam(CameraType cam)
    {
        return cams[(int)cam];
    }
}



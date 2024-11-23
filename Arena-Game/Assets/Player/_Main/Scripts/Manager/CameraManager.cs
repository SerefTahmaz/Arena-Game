using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using ArenaGame.Utils;
using DefaultNamespace;
using DG.Tweening;
using RootMotion;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraManager : cSingleton<CameraManager>
{
    [SerializeField] private GameCamera _gameplayCam;
    [SerializeField] private InputOnMouseDown _gameplayCamInput;
    [SerializeField] private GameCamera m_focusCam;
    [SerializeField] private CinemachineFreeLook m_CinemachineFreeLook;

    private GameCamera m_CurrentCam;
    
    [Serializable]
    public class GameCamera
    {
        public GameObject m_Cam;
        public cCamShake m_CameraShake;

        public void SetActive(bool value)
        {
            m_Cam.SetActive(value);
        }
    }

    private List<GameCamera> cams = new List<GameCamera>();
    private bool _isEscaped = false;
    
    private Action<CameraType> m_OnCameraChange = delegate(CameraType type) {  };
    private CameraType m_CurrentCamType = CameraType.Gameplay;
    
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

    public CameraType CurrentCamType => m_CurrentCamType;

    private void Awake()
    {
        // cMobileInputManager._onFocusEvent += () =>
        // {
        //     if (_gameplayCam.activeSelf)
        //     {
        //         SetCamera(CameraType.Focus);
        //     }
        //     else
        //     {
        //         SetCamera(CameraType.Gameplay);
        //     }
        // };
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

        m_CurrentCamType = cam;
        m_CurrentCam = cams[(int)cam];
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

    public void SetInput(bool value)
    {
        m_CinemachineFreeLook.enabled = value;
        // m_focusCam.SetActive(value);
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
        var instanceOwnerPlayer = GameplayStatics.OwnerPlayer;

        _gameplayCam.m_Cam.GetComponent<CinemachineFreeLook>().LookAt = instanceOwnerPlayer;
        _gameplayCam.m_Cam.GetComponent<CinemachineFreeLook>().Follow = instanceOwnerPlayer;
        _gameplayCam.m_Cam.GetComponent<CinemachineFreeLook>().ForceCameraPosition(
            -instanceOwnerPlayer.forward * 5 + instanceOwnerPlayer.position + Vector3.up*2
            , instanceOwnerPlayer.rotation);
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        EnableGameplayCam();
    }

    public void FixLook()
    {
        var instanceOwnerPlayer = GameplayStatics.OwnerPlayer;
        _gameplayCam.m_Cam.GetComponent<CinemachineFreeLook>().ForceCameraPosition(
            -instanceOwnerPlayer.forward * 5 + instanceOwnerPlayer.position + Vector3.up*2
            , instanceOwnerPlayer.rotation);
    }

    public GameObject GetCam(CameraType cam)
    {
        return cams[(int)cam].m_Cam;
    }

    public void ShakeCamera(int intensity, int freq, float time)
    {
        m_CurrentCam.m_CameraShake.ShakeCamera(intensity,freq,time);
    }
}



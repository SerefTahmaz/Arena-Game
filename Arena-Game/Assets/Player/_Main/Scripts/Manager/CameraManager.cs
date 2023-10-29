using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject _gameplayCam;
    [SerializeField] private InputOnMouseDown _gameplayCamInput;
    [SerializeField] private CinemachineFreeLookZoom _cinemachineFreeLookZoom;
    

    private List<GameObject> cams = new List<GameObject>();
    private bool _isEscaped = false;

    private void Awake()
    {
        cPlayerManager.Instance.m_OwnerPlayerSpawn += transform1 =>
        {
            _gameplayCam.GetComponent<CinemachineFreeLook>().Follow = transform1;
            _gameplayCam.GetComponent<CinemachineFreeLook>().LookAt = transform1;
        };
    }

    private void Start()
    {
        cams.Add(_gameplayCam);
        Cursor.visible = false;
    }

    public void SetCamera(CameraType cam)
    {
        foreach (var VARIABLE in cams)
        {
            VARIABLE.SetActive(false);
        }
        
        cams[(int)cam].SetActive(true);
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

    public enum CameraType
    {
        Gameplay,
        Player,
        NpcCam
    }
}



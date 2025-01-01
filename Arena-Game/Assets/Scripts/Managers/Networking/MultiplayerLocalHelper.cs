using System;
using ArenaGame.Utils;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using UI.EndScreen;
using Unity.Netcode;
using UnityEngine;

public class MultiplayerLocalHelper : cSingleton<MultiplayerLocalHelper>
{
    [SerializeField] private MultiplayerNetworkHelper m_MultiplayerNetworkHelper;
 
    private bool m_WaitingGameStart;

    public MultiplayerNetworkHelper NetworkHelper => m_MultiplayerNetworkHelper;
    public Action OnMultiplayerGameStarted { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        cPlayerManager.Instance.m_OwnerPlayerSpawn += OnOwnerPlayerSpawn;
        NetworkManager.Singleton.OnClientConnectedCallback += SingletonOnOnClientConnectedCallback;
    }

    private void SingletonOnOnClientConnectedCallback(ulong obj)
    {
        HandleConnection();
    }

    private async UniTask HandleConnection()
    {
        var loadingToken = new object();
        LoadingScreen.Instance.ShowPage(loadingToken,true);
        await UniTask.WaitUntil((() => NetworkHelper.m_IsGameStarted.Value));
        LoadingScreen.Instance.HidePage(loadingToken);
    }

    private void OnOwnerPlayerSpawn(cCharacter ownerPlayer)
    {
        if(cUIManager.Instance)  LoadingScreen.Instance.ShowPage(this);
        if (NetworkHelper.m_IsGameStarted.Value)
        {
            StartGame();
        }
        else
        {
            m_WaitingGameStart = true;
        }
    }
    
    private void StartGame()
    {
        if(cUIManager.Instance)  LoadingScreen.Instance.HidePage(this);
        InputManager.Instance.SetInput(true);
        CameraManager.Instance.SetInput(true);
        CameraManager.Instance.FixLook();
        WinScreenUIController.Instance.RewardExp = NetworkHelper.m_RewardExp.Value;
        if(NetworkHelper.m_DrawSwordOnStart.Value) GameplayStatics.OwnerPlayer.GetComponent<cPlayerStateMachineV2>().DrawSword();
        OnMultiplayerGameStarted?.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_WaitingGameStart)
        {
            if (NetworkHelper.m_IsGameStarted.Value)
            {
                StartGame();
                m_WaitingGameStart = false;
            }
        }
    }

    public void SetGameStarted(bool value, int rewardExpAmount,bool drawSword = true)
    {
        NetworkHelper.m_DrawSwordOnStart.Value = drawSword;
        NetworkHelper.m_RewardExp.Value = rewardExpAmount;
        NetworkHelper.m_IsGameStarted.Value = value;
    }
}

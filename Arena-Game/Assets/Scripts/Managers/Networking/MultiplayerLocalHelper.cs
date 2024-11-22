using ArenaGame.Utils;
using DefaultNamespace;
using UnityEngine;

public class MultiplayerLocalHelper : cSingleton<MultiplayerLocalHelper>
{
    [SerializeField] private MultiplayerNetworkHelper m_MultiplayerNetworkHelper;
 
    private bool m_WaitingGameStart;

    public MultiplayerNetworkHelper NetworkHelper => m_MultiplayerNetworkHelper;

    // Start is called before the first frame update
    void Start()
    {
        cPlayerManager.Instance.m_OwnerPlayerSpawn += OnOwnerPlayerSpawn;
    }

    private void OnOwnerPlayerSpawn(cCharacter ownerPlayer)
    {
        if(cUIManager.Instance) cUIManager.Instance.ShowPage(Page.Loading,this);
        if (NetworkHelper.m_IsGameStarted.Value)
        {
            StartGame();
        }
        else
        {
            m_WaitingGameStart = true;
        }
    }
    
    public void StartGame()
    {
        if(cUIManager.Instance) cUIManager.Instance.HidePage(Page.Loading,this);
        InputManager.Instance.SetInput(true);
        CameraManager.Instance.SetInput(true);
        CameraManager.Instance.FixLook();
        GameplayStatics.OwnerPlayer.GetComponent<cPlayerStateMachineV2>().DrawSword();
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
}

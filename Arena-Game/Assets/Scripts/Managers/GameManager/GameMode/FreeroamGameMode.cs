using ArenaGame.Managers.SaveManager;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Gameplay.Character;
using Unity.Netcode;
using UnityEngine;

public class FreeroamGameMode : MonoBehaviour,IGameModeHandler
{
    [SerializeField] private ProjectSceneManager m_ProjectSceneManager;
    [SerializeField] private Transform m_PlayerStartPoint;
    
    private bool m_IsActive;

    public void StartGame()
    {
        cGameManager.Instance.StartGameClient();
        cGameManager.Instance.m_OnMainMenuButton += OnMainMenuButton;

        m_IsActive = true;
            
        LoopStart();
    }

    private void LoopStart()
    {
        LoopStartAsync();
    }

    private async UniTask LoopStartAsync()
    {
        cUIManager.Instance.ShowPage(Page.Gameplay,this);
        cUIManager.Instance.HidePage(Page.Scoreboard,this);
        LoadingScreen.Instance.ShowPage(this);
        cPlayerManager.Instance.DestroyPlayers();
        
        UserSaveHandler.Load();
        await MapManager.Instance.LoadFreeroamLevel();
        
        foreach (var VARIABLE in NetworkManager.Singleton.ConnectedClients)
        {
            var player = OnClientConnected(VARIABLE.Key).transform;
        }

        DOVirtual.DelayedCall(2, () =>
        {
            if (m_IsActive)
            {
                LoadingScreen.Instance.HidePage(this);
                MultiplayerLocalHelper.Instance.SetGameStarted(true,0,false);
                var playerSM = cGameManager.Instance.m_OwnerPlayer.GetComponent<cPlayerStateMachineV2>();
            } 
        });
    }
    
    private GameObject OnClientConnected(ulong obj)
    {
        Vector3 pos;
        GameObject go;
        pos = m_PlayerStartPoint.position;
        Vector3 dir = m_PlayerStartPoint.forward;
        var lookRot = Quaternion.LookRotation(dir.normalized);
        go = cPlayerManager.Instance.SpawnPlayer(pos, lookRot, obj);
        go.GetComponent<HumanCharacter>().CharacterNetworkController.m_TeamId.Value = 10;
        return go;
    }
    
    private void OnMainMenuButton()
    {
        if (m_IsActive)
        {
            OnGameEnd();
        }
    }

    private void OnGameEnd()
    {
        m_IsActive = false;
        cGameManager.Instance.m_OnMainMenuButton -= OnMainMenuButton;
        cUIManager.Instance.HidePage(Page.Gameplay,this);
        cUIManager.Instance.ShowPage(Page.Scoreboard,this);
        Debug.Log("GameEnded!!!!!!!!!!");
    }
}
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public class RoomPanel : UIBInder
{
    [SerializeField] PlayerEntry[] _playerEntries;
    [SerializeField] Button _startButton;


    private void OnEnable()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        UpdatePlayers();

        PlayerNumbering.OnPlayerNumberingChanged += UpdatePlayers;

        //게임 씬 들어가기 전에 초기화해줘야할 변수들.
        PhotonNetwork.LocalPlayer.SetReady(false);
        PhotonNetwork.LocalPlayer.SetLoad(false);
        GameSceneManager.Instance.CurPlayerCount = GameSceneManager.Instance.OriginPlayerCount;
        for(int i = 0; i < GameSceneManager.Instance.CurUnitCounts.Length; i++)
        {
            GameSceneManager.Instance.CurUnitCounts[i] = 0;
        }

        GameSceneManager.Instance.IsFinish = false;
        GameSceneManager.Instance.IsLoad = false;
        ObjectPool.Instance.SetPool();
    }

    private void OnDisable()
    {
        PlayerNumbering.OnPlayerNumberingChanged -= UpdatePlayers;
        PhotonNetwork.AutomaticallySyncScene = false;
    }

    public void UpdatePlayers()
    {
        foreach (PlayerEntry entry in _playerEntries)
        {
            entry.SetEmpty();
        }

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.GetPlayerNumber() == -1)
                continue;

            int number = player.GetPlayerNumber();
            _playerEntries[number].SetPlayer(player);
        }

        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            _startButton.interactable = CheckAllReady();
        }
        else
        {
            _startButton.interactable = false;
        }
    }

    public void EnterPlayer(Player newPlayer)
    {
        Debug.Log($"{newPlayer.NickName} 입장!");
        UpdatePlayers();
    }

    public void ExitPlayer(Player otherPlayer)
    {
        Debug.Log($"{otherPlayer.NickName} 퇴장!");
        UpdatePlayers();
    }

    public void UpdatePlayerProperty(Player targetPlayer, Hashtable properties)
    {
        // 레디 커스텀 프로퍼티를 변경한 경우면 READY 키가 있음
        if (properties.ContainsKey(CustomProperty.READY))
        {
            UpdatePlayers();
        }
    }

    private bool CheckAllReady()
    {
        if (PhotonNetwork.PlayerList.Length < 4)
            return false;

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.GetReady() == false)
                return false;
        }

        return true;
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel("SceneOJH");
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
}

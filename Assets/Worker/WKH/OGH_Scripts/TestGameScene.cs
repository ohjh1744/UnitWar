using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using static Photon.Pun.UtilityScripts.PlayerNumbering;

public class TestGameScene : MonoBehaviourPunCallbacks
{

    public const string RoomName = "TestRoom";

    private Coroutine spawnRoutine;
    void Start()
    {
        PhotonNetwork.LocalPlayer.NickName = $"Player {Random.Range(1000, 10000)}";
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 4;
        options.IsVisible = false;

        PhotonNetwork.JoinOrCreateRoom(RoomName, options, TypedLobby.Default);
    }
    
    public override void OnJoinedRoom()
    {
        StartCoroutine(StartDelayRoutine());
    }

    IEnumerator StartDelayRoutine()
    {
        yield return new WaitForSeconds(1f);
        TestGameStart();
    }
    public void TestGameStart()
    {
        Debug.Log("게임 시작");

        //테스트용 게임 시작 부분
        Debug.Log($"플레이어 넘버 : {PhotonNetwork.LocalPlayer.GetPlayerNumber()}");

        if (PhotonNetwork.IsMasterClient == false)
        {
            return;
        }

        //방장만 진행할 수 있는 코드
        ObjectPool.Instance.Init(5);
    }

    //방장이 바뀌게 되면 새로운 방장이 돌려줌 
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (newMasterClient.IsLocal)
        {
        }
    }
}
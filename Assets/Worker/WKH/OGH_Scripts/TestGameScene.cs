using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using static Photon.Pun.UtilityScripts.PlayerNumbering;


public enum EGameState {Ready ,Start, Running ,End } 
public class TestGameScene : MonoBehaviourPunCallbacks
{

    public const string RoomName = "TestRoom";

    private Coroutine _spawnUnitroutine;

    [SerializeField] private EGameState _gameState;

    [SerializeField] private float[] _time;                   // Unit별 생성 주기 체크.

    [SerializeField] private float[] _spawnTime;              // Unit별 Spawn Time 설정.

    [SerializeField] private int[] _unitCounts;            // Unit별 현재 개수.

    [SerializeField] Transform[] _spawnPos;                   // 종족별 기본 Spawn 위치

    [SerializeField] UnitSpawner _spawner;
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

        _spawnUnitroutine = StartCoroutine(SpawnUnit());

    }
    IEnumerator SpawnUnit()
    {
        while (true)
        {
            _time[(int)EUnit.Zealot] += 1f;
            _time[(int)EUnit.DarkTemplar] += 1f;
            _time[(int)EUnit.Juggling] += 1f;
            _time[(int)EUnit.Ultralisk] += 1f;

            if (_time[(int)EUnit.Zealot] >= _spawnTime[(int)EUnit.Zealot])
            {
                _spawner.Spawn((int)EUnit.Zealot, _unitCounts[(int)EUnit.Zealot], _spawnPos[(int)EUnit.Zealot].position);
                _unitCounts[(int)EUnit.Zealot]++;
                _time[(int)EUnit.Zealot] = 0;
            }

            if (_time[(int)EUnit.DarkTemplar] >= _spawnTime[(int)EUnit.DarkTemplar])
            {
                _spawner.Spawn((int)EUnit.DarkTemplar, _unitCounts[(int)EUnit.DarkTemplar], _spawnPos[(int)EUnit.DarkTemplar].position);
                _unitCounts[(int)EUnit.DarkTemplar]++;
                _time[(int)EUnit.DarkTemplar] = 0;
            }

            if (_time[(int)EUnit.Juggling] >= _spawnTime[(int)EUnit.Juggling])
            {
                _spawner.Spawn((int)EUnit.Juggling, _unitCounts[(int)EUnit.Juggling], _spawnPos[(int)EUnit.Juggling].position);
                _unitCounts[(int)EUnit.Juggling]++;
                _time[(int)EUnit.Juggling] = 0;
            }

            if (_time[(int)EUnit.Ultralisk] >= _spawnTime[(int)EUnit.Ultralisk])
            {
                _spawner.Spawn((int)EUnit.Ultralisk, _unitCounts[(int)EUnit.Ultralisk], _spawnPos[(int)EUnit.Ultralisk].position);
                _unitCounts[(int)EUnit.Ultralisk]++;
                _time[(int)EUnit.Ultralisk] = 0;
            }
            yield return new WaitForSeconds(1f);
        }
    }


    //방장이 바뀌게 되면 새로운 방장이 돌려줌 
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (newMasterClient.IsLocal)
        {
            if(_spawnUnitroutine != null)
            {
                StopCoroutine(_spawnUnitroutine);
            }
            _spawnUnitroutine = StartCoroutine(SpawnUnit());

        }
    }
}
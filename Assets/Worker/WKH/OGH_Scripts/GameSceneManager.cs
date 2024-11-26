using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using UnityEngine;

public class GameSceneManager : MonoBehaviourPunCallbacks
{
    public const string RoomName = "TestRoom";

    private Coroutine _spawnUnitroutine;

    [SerializeField] private EGameState _gameState;

    [SerializeField] private int _playerCount;                          // 시작하기 위한 참여 플레이어 수

    [SerializeField] private UnitSpawner _spawner;                      // 스포너(커맨드센터)

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

    IEnumerator WaitPlayerEnter()
    {
        WaitForSeconds delay = new WaitForSeconds(1);
        while (PhotonNetwork.PlayerList.Length < _playerCount)
        {
            Debug.Log($"현재 플레이어 수 : {PhotonNetwork.PlayerList.Length}");
            yield return delay;
        }
        Debug.Log("모든 플레이어가 접속했습니다. 게임시작 준비 중");
        StartCoroutine(StartDelayRoutine());
    }

    public override void OnJoinedRoom()
    {
        StartCoroutine(WaitPlayerEnter());
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

        // TODO : playercontroller 클라이언트마다 배치 다르게 설정하는 부분
        SetPlayerController();
        if (PhotonNetwork.IsMasterClient == false)
        {
            return;
        }
        //방장만 진행할 수 있는 코드


    }
    /// <summary>
    /// PlayerController를 클라이언트마다 다르게 생성하는 메서드
    /// </summary>
    private void SetPlayerController()
    {
        if (PhotonNetwork.LocalPlayer.IsLocal)
        {
            switch (PhotonNetwork.LocalPlayer.GetPlayerNumber())
            {
                case 0:
                    GameObject playerController_zealot = PhotonNetwork.Instantiate("Prefabs/PlayerController_Zealot", Vector3.zero, Quaternion.identity);
                    break;
                case 1:
                    GameObject playerController_DarkTempler = PhotonNetwork.Instantiate("Prefabs/PlayerController_DarkTempler", Vector3.zero, Quaternion.identity);
                    break;
                case 2:
                    GameObject playerController_Zergling = PhotonNetwork.Instantiate("Prefabs/PlayerController_Zergling", Vector3.zero, Quaternion.identity);
                    break;
                case 3:
                    GameObject playerController_Ultrarisk = PhotonNetwork.Instantiate("Prefabs/PlayerController_Ultrarisk", Vector3.zero, Quaternion.identity);
                    break;
                default:
                    break;
            }
        }

    }
}
